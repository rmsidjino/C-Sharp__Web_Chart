using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChartDirector;
using VMS_MES_WEB.Models;
using VMS_MES_WEB.Models.DAC;

namespace VMS_MES_WEB.Controllers
{
    [RoutePrefix("Gantt")]
    public class GanttController : Controller
    {
        EqpPlanDAC db = new EqpPlanDAC();
        List<EqpStep> list;
        ArrayList eqpPlan = new ArrayList();
        //
        // Default Action
        //
        public ActionResult Index()
        {
            
           
            SelectCategory();
            return PartialView();
        }

        //Get : https://localhost:44370/Gantt/LoadEqpPlan/PAINT/StarBucks/
        [HttpGet]
        [Route("LoadEqpPlan/{EQP}/{ProductID}")]
        public JsonResult LoadEqpPlan(string EQP = "", string ProductID = "")
        {
            list = db.GetEqpPlan();
            if (EQP != "")
            {
                if (ProductID != "")
                {
                    list = list.FindAll((x) => x.STEP_ID == EQP && x.name == ProductID);
                }
                else
                    list = list.FindAll((x) => x.STEP_ID == EQP);
            }
            else
            {
                if (ProductID != "")
                    list = list.FindAll((x) => x.name == ProductID);
            }

            var stepNum = list.GroupBy(o => new { o.STEP_ID })
               .Select(o => o.FirstOrDefault());
            var eqpNum = list.GroupBy(o => new { o.name })
               .Select(o => o.FirstOrDefault());

            foreach (var item in stepNum)
            {
                eqpPlan.Add(new
                {
                    id = item.STEP_ID,
                    name = item.STEP_ID

                });
                foreach (var eqp in eqpNum.Where((x) => x.STEP_ID == item.STEP_ID))
                {
                    var elist = list.FindAll(x => x.name == eqp.name);
                    ArrayList ePlan = new ArrayList();
                    for (int i = 0; i < elist.Count; i++)
                    {
                        ePlan.Add(new
                        {
                            id = elist[i].id,
                            start = elist[i].start,
                            end = elist[i].end

                        });
                    }
                    eqpPlan.Add(new
                    {
                        id = eqp.name,
                        name = eqp.name,
                        periods = ePlan,
                        parent = item.STEP_ID
                    });
                }
            }
            return Json(eqpPlan, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCategory()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Select", Value = "",Selected = true });

            items.Add(new SelectListItem { Text = "PRESS", Value = "PRESS" });

            items.Add(new SelectListItem { Text = "PAINT", Value = "PAINT",  });

            items.Add(new SelectListItem { Text = "FINISH", Value = "FINISH" });

            ViewBag.EQP = items;

            ViewBag.Product = items;

            return View();

        }

        public ActionResult Search()
        {

            return RedirectToAction("Index");

        }

        //public string ListToJsonObj(List<EqpPlan> list)
        //{
        //    StringBuilder JsonString = new StringBuilder();
        //    if (list != null && list.Count > 0)
        //    {
        //        JsonString.Append("[");
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            JsonString.Append("{");
        //            for (int j = 0; j < 3; j++)
        //            {
        //                if (j == 0)
        //                {
        //                    JsonString.Append("\"" + "id" + "\":" + "\"" + list[i].LOT_ID + "\",");
        //                }
        //                else if (j == 1)
        //                {
        //                    JsonString.Append("\"" + "start" + "\":" + "\"" + list[i].START_TIME + "\"");
        //                }
        //                else if (j == 2)
        //                {
        //                    JsonString.Append("\"" + "end" + "\":" + "\"" + list[i].END_TIME + "\"");
        //                }
        //            }
        //            if (i == ds.Tables[0].Rows.Count - 1)
        //            {
        //                JsonString.Append("}");
        //            }
        //            else
        //            {
        //                JsonString.Append("},");
        //            }
        //        }
        //        JsonString.Append("]");
        //        return JsonString.ToString();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

    }
}