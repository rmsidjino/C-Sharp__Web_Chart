using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VMS_MES_WEB.Models;
using VMS_MES_WEB.Models.DAC;

namespace VMS_MES_WEB.Controllers
{
    public class CapacityController : Controller
    {
        LoadStatDAC db = new LoadStatDAC();
        // GET: Capacity
        public ActionResult Index()
        {

            List<LoadStat> list = db.GetLoadStat();

            var statGroup = from stat in list
                            orderby stat.TARGET_DATE
                            group stat by stat.EQP_ID;

            int k = 1;
            StringBuilder sb = new StringBuilder();

            foreach (var EqpGroup in statGroup) //3번
            {
                List<int> amts = new List<int>();
                foreach (var loadStat in EqpGroup) //12번
                {
                    amts.Add((int)loadStat.BUSY);

                    if (k == 1)
                        sb.Append(loadStat.TARGET_DATE + "일,");
                }

                if (k == 1)
                {
                    ViewBag.Label1 = EqpGroup.Key;
                    ViewBag.data1 = "[" + string.Join(",", amts) + "]";
                }
                else if (k == 2)
                {
                    ViewBag.Label2 = EqpGroup.Key;
                    ViewBag.data2 = "[" + string.Join(",", amts) + "]";
                }
                else if (k == 3)
                {
                    ViewBag.Label3 = EqpGroup.Key;
                    ViewBag.data3 = "[" + string.Join(",", amts) + "]";
                }

                k++;
            }
            ViewBag.Labels = sb.ToString().TrimEnd(',');

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            TestData t = new TestData();
            List<columnsinfo> _col = new List<columnsinfo>();

            DataTable dt = db.GetLoadStatByStep("2019-11-07", "2019-11-14");
            DataTable dtCloned = dt.Clone();
            for(int j=0;j< dtCloned.Columns.Count - 2; j++)
            {
                dtCloned.Columns[j+2].DataType = typeof(string);
            }
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            foreach (DataRow dr in dtCloned.Rows)
            {
                for (int i = 2; i < dtCloned.Columns.Count; i++)
                {
                    dr[i] = string.Concat(Math.Round(Convert.ToDecimal(dr[i]), 2),"%");
                }
            }

            for (int i = 0; i <= dtCloned.Columns.Count - 1; i++)
            {
                _col.Add(new columnsinfo { title = dtCloned.Columns[i].ColumnName, data = dtCloned.Columns[i].ColumnName });
            }

            string col = (string)serializer.Serialize(_col);
            t.columns = col;


            var lst = dtCloned.AsEnumerable()
            .Select(r => r.Table.Columns.Cast<DataColumn>()
                    .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                   ).ToDictionary(z => z.Key, z => z.Value)
            ).ToList();

            string data = serializer.Serialize(lst);
            t.data = data;

            return View(t);
        }

        //public JsonResult LoadTable()
        //{
        //    DataTable dt = db.GetLoadStatByStep("2019-11-07", "2019-11-14");

        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}



        public ActionResult LoadTable()
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            TestData t = new TestData();
            List<columnsinfo> _col = new List<columnsinfo>();

            DataTable dt = db.GetLoadStatByStep("2019-11-07", "2019-11-14");
            

            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
            }

            string col = (string)serializer.Serialize(_col);
            t.columns = col;


            var lst = dt.AsEnumerable()
            .Select(r => r.Table.Columns.Cast<DataColumn>()
                    .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                   ).ToDictionary(z => z.Key, z => z.Value)
            ).ToList();

            string data = serializer.Serialize(lst);
            t.data = data;

            return PartialView(t);
        }

        public ActionResult graph()
        {
            List<LoadStat> list = db.GetLoadStat();
            List<DateTime> date = new List<DateTime>();
            int j = 0;
            for(int i =0; i < list.Count; i++)
            {
                if (!date.Contains(list[i].TARGET_DATE))
                {

                }
            }
            return View();
        }



        public class TestData
        {
            public string jsondata { get; set; }
            public string columns { get; set; }
            public string data { get; set; }
        }

        public class columnsinfo
        {
            public string title { get; set; }
            public string data { get; set; }
        }
    }
}