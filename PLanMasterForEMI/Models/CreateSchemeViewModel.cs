using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLanMasterForEMI.Models
{
    public class CreateSchemeViewModel
    {
        public int SchemeId { get; set; }
        public string PlanName { get; set; }
        [Required]
        public int Tenure { get; set; }
        [Required]
        public string ROI { get; set; }
    }
}