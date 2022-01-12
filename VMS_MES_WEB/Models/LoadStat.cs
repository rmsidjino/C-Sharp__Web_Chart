using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS_MES_WEB.Models
{
    public class LoadStat
    {
        public string VERSION_NO { get; set; }
        public string LINE_ID { get; set; }
        public string EQP_ID { get; set; }
        public string EQP_GROUP { get; set; }
        public DateTime TARGET_DATE { get; set; }
        public decimal SETUP { get; set; }
        public decimal BUSY { get; set; }
        public decimal IDLE { get; set; }
        public decimal PERCENTAGE { get; set; }

    }
}