
using PLanMasterForEMI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PLanMasterForEMI.Repository
{
    public class PlanMasterRepository
    {
        string connectionString = "Data Source=.;Initial Catalog=GTechDB;Integrated Security=true;";
        public bool SavePlanMaster(CreateSchemeViewModel model)
        {
            //string query = "INSERT INTO CreateScheme VALUES (@PlanName,@Tenure,@ROI); SELECT SCOPE_IDENTITY();";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertSchemeData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PlanName", model.PlanName);
            cmd.Parameters.AddWithValue("@Tenure", model.Tenure);
            cmd.Parameters.AddWithValue("@ROI", model.ROI);
            con.Open();
            int id = cmd.ExecuteNonQuery();
            con.Close();
            if (id > 0)
            {
                model.SchemeId = id;
                return true;
            }
            else
            {
                return false;
            }            
        }
        public EMISchedule GetEMISheduleData(string PlanName)
        {
            EMISchedule eMISchedule = new EMISchedule();
            string query = "Select top 1 * from CreateScheme where PlanName=@planName order by 1 desc";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PlanName", PlanName);
           
            con.Open();
            var data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                if (data.Read())
                {
                    eMISchedule.Tenure = Convert.ToDouble(data[2]);
                    eMISchedule.ROI = Convert.ToDouble(data[3]);
                }
            }
            con.Close();
            if (eMISchedule != null)
            {
                return eMISchedule;
            }
            return null;  
        }

        public void SaveGenerateEmiData(EMISchedule eMISchedule)
        {
            //string query = "INSERT INTO EMISchedule VALUES (@Tenure,@ROI,@LoanAmount,@LoanDate,@EMIAmount); SELECT SCOPE_IDENTITY();";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("InsertEmiLoanData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Tenure", eMISchedule.Tenure);
            cmd.Parameters.AddWithValue("@ROI", eMISchedule.ROI);
            cmd.Parameters.AddWithValue("@LoanAmount", eMISchedule.LoanAmount);
            cmd.Parameters.AddWithValue("@LoanDate", eMISchedule.LoanDate);
            cmd.Parameters.AddWithValue("@EMIAmount", eMISchedule.EMIAmount);
            con.Open();
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            if (id > 0)
            {
                eMISchedule.EMIScheduleId = id;
            }
        }

        public List<EMISchedule> GenerateSchedule()
        {
            List<EMISchedule> eMISchedules = new List<EMISchedule>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetAllLoanData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            var data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                while(data.Read())
                {
                    EMISchedule eMISchedule = new EMISchedule();
                    eMISchedule.EMIScheduleId = Convert.ToInt32(data["EMIScheduleId"]);
                    eMISchedule.LoanDate = Convert.ToDateTime(data["LoanDate"]).AddDays(30);
                    eMISchedule.EMIAmount = data["EMIAmount"] != null ? Convert.ToDouble(data["EMIAmount"]) : 0;
                    eMISchedules.Add(eMISchedule);
                }
            }
            con.Close();
            if (eMISchedules != null)
            {
                return eMISchedules;
            }
            return null;
        }
    }
}