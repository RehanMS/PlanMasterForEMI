using PLanMasterForEMI.Models;
using PLanMasterForEMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PLanMasterForEMI.Controllers
{
    public class HomeController : Controller
    {
        private PlanMasterRepository planRepository;
        public ActionResult PlanMaster()
        {

            return View();
        }
        [HttpPost]
        public ActionResult PostPlanMaster(CreateSchemeViewModel model)
        {
            bool result = false;
            planRepository = new PlanMasterRepository();
            if(ModelState.IsValid)
            {
                result = planRepository.SavePlanMaster(model);
                if(result)
                {
                    return RedirectToAction("GenerateEMISchedule");
                    //ViewBag.Result = "Saved Successfully";
                }
                else
                {
                    ViewBag.Result = "Failed";
                }
                return View("PlanMaster");
            }
            else
            {
                ViewBag.Result = "Please  enter input";
                return View("PlanMaster");
            }
            
        }
        public ActionResult GenerateEMISchedule()
        {
            var data = TempData["EMISchedule"];
            if (data == null)
            {
                return View(new List<EMISchedule>());
            }
            else
            {
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult GetTenureROI(string planName)
        {
            planRepository = new PlanMasterRepository();
            EMISchedule eMISchedule = planRepository.GetEMISheduleData(planName);
            if (eMISchedule != null)
            {
                return Json(eMISchedule,JsonRequestBehavior.AllowGet);
            }
            return View(new List<EMISchedule>());
        }
        [HttpPost]
        public ActionResult CalculateEMI(EMISchedule eMISchedule)
        {

            if (eMISchedule.LoanAmount > 0 && eMISchedule.Tenure > 0 && eMISchedule.ROI > 0 && eMISchedule.LoanDate > Convert.ToDateTime("01 - 01 - 0001 00:00:00"))
            {
                double x = eMISchedule.ROI / 100;
                double y = x * eMISchedule.LoanAmount;
                double z = eMISchedule.LoanAmount + y;
                double emiAmount = z / eMISchedule.Tenure;
                planRepository = new PlanMasterRepository();
                eMISchedule.EMIAmount = emiAmount;
                planRepository.SaveGenerateEmiData(eMISchedule);
                return Json(emiAmount, JsonRequestBehavior.AllowGet);
            }
            return View(new List<EMISchedule>());
        }
        public ActionResult GenerateSchedule()
        {
            List<EMISchedule> eMISchedules = new List<EMISchedule>();
            planRepository = new PlanMasterRepository();
            eMISchedules=planRepository.GenerateSchedule();
            TempData["EMISchedule"] = eMISchedules;
            return RedirectToAction("GenerateEMISchedule", "Home");
        }
    }
}