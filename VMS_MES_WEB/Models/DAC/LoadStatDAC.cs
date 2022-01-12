using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace VMS_MES_WEB.Models.DAC
{
    public class LoadStatDAC : IDisposable
    {
        SqlConnection conn;
        DataTable dt = new DataTable();
        public LoadStatDAC()
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["local"].ConnectionString);
            conn.Open();
        }
        public void Dispose()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public List<LoadStat> GetLoadStat()
        {
            string sql = @"select LINE_ID,EQP_GROUP, TARGET_DATE, ROUND(avg(BUSY),2) as BUSY from

(select LINE_ID, CASE WHEN SUBSTRING(EQP_ID,4,1) =1 THEN 'PRESS'

WHEN SUBSTRING(EQP_ID,4,1) =2 THEN 'PAINT'

WHEN SUBSTRING(EQP_ID,4,1) =3 THEN 'FINISH' END as EQP_GROUP, TARGET_DATE, BUSY from LOAD_STAT) a group by LINE_ID,EQP_GROUP,TARGET_DATE order by EQP_GROUP,TARGET_DATE";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    List<LoadStat> list = Helper.DataReaderMapToList<LoadStat>(cmd.ExecuteReader());
                    Dispose();
                    return list;
                }
            }
            catch (Exception err)
            {
                return null;
            }
        }

        public DataTable GetLoadStatByStep(string dtFrom, string dtTo)
        {
            string sql = "SP_PivotLoadStat";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_StartDT", dtFrom);
                    cmd.Parameters.AddWithValue("@P_EndDT", dtTo);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                    //return DataTableToJsonObj(dt);
                }
            }
            catch (Exception err)
            {
                return null;
            }
        }

        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}