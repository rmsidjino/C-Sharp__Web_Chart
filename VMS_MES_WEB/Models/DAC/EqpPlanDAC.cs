using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace VMS_MES_WEB.Models.DAC
{
    public class EqpPlanDAC
    {
        SqlConnection conn;
        public EqpPlanDAC()
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

        public List<EqpStep> GetEqpPlan()
        {
            string sql = @"select EQP_ID as [name], LOT_ID as id, STEP_ID, DATEDIFF_BIG(MILLISECOND,'1970-01-01 00:00:00.000', (START_TIME)) as [start], DATEDIFF_BIG(MILLISECOND,'1970-01-01 00:00:00.000', (END_TIME)) as [end], MACHINE_STATE
from EQP_PLAN
order by EQP_ID,START_TIME";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    List<EqpStep> list = Helper.DataReaderMapToList<EqpStep>(cmd.ExecuteReader());
                    return list;
                }
            }
            catch (Exception err)
            {
                return null;
            }
        }
    }
}