using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLanMasterForEMI.Models
{
    public class EMISchedule
    {
        public int EMIScheduleId { get; set; }
        public double Tenure { get; set; }
        public double ROI { get; set; }
        public double LoanAmount { get; set; }
        public DateTime LoanDate { get; set; }
        public double? EMIAmount { get; set; }
    }
}