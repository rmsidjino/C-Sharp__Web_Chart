using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace VMS_MES_WEB.Models
{
    //LINE_ID, EQP_ID, LOT_ID, PRODUCT_ID, PROCESS_ID, STEP_ID, PROCESS_QTY, START_TIME, END_TIME, MACHINE_STATE, DUE_DATE
    public class EqpPlan
    {
        public string LINE_ID { get; set; }
        public string EQP_ID { get; set; }
        public string LOT_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string STEP_ID { get; set; }
        public int PROCESS_QTY { get; set; }
        public DateTime START_TIME { get; set; }
        public DateTime END_TIME { get; set; }
        public string MACHINE_STATE { get; set; }
        public DateTime DUE_DATE { get; set; }


    }

    public class EqpData
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<EqpStep> period { get; set; }
        public string parent { get; set; }

    }

    public class EqpStep
    {
        public string id { get; set; }
        public Int64 start { get; set; }
        public Int64 end { get; set; }
        public string name { get; set; }
        public string STEP_ID { get; set; }
        public string MACHINE_STATE { get; set; }
    }

    public class UserModel
    {
        public TeaType SelectTeaType { get; set; }
    }

    public enum TeaType
    {
        Tea, Coffee, GreenTea, BlackTea
    }

}