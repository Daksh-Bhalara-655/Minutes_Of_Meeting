using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;

namespace Minutes_Of_Meeting.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Db_Connection _db;

        public DashboardController(Db_Connection db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            using (var conn = _db.CreateConnection())
            using (var cmd = new SqlCommand("sp_GetDashboardData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    // First result set: Stats (TotalMeetings, UpcomingMeetings, CompletedMeetings, CancelledMeetings)
                    if (reader.Read())
                    {
                        model.TotalMeetings = reader["TotalMeetings"] == DBNull.Value ? 0 : (int)reader["TotalMeetings"];
                        model.UpcomingMeetings = reader["UpcomingMeetings"] == DBNull.Value ? 0 : (int)reader["UpcomingMeetings"];
                        model.CompletedMeetings = reader["CompletedMeetings"] == DBNull.Value ? 0 : (int)reader["CompletedMeetings"];
                        model.CancelledMeetings = reader["CancelledMeetings"] == DBNull.Value ? 0 : (int)reader["CancelledMeetings"];
                    }

                    // Second result set: Meetings by Type
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            model.TypeData.Add(new ChartData
                            {
                                Label = reader["Label"] == DBNull.Value ? "" : reader["Label"].ToString(),
                                Value = reader["Value"] == DBNull.Value ? 0 : (int)reader["Value"]
                            });
                        }
                    }

                    // Third result set: Meetings by Department
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            model.DeptData.Add(new ChartData
                            {
                                Label = reader["Label"] == DBNull.Value ? "" : reader["Label"].ToString(),
                                Value = reader["Value"] == DBNull.Value ? 0 : (int)reader["Value"]
                            });
                        }
                    }

                    // Fourth result set: Monthly Trend
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            model.TrendData.Add(new TrendData
                            {
                                Month = reader["Month"] == DBNull.Value ? "" : reader["Month"].ToString(),
                                Scheduled = reader["Scheduled"] == DBNull.Value ? 0 : (int)reader["Scheduled"],
                                Completed = reader["Completed"] == DBNull.Value ? 0 : (int)reader["Completed"]
                            });
                        }
                    }
                }
            }

            return View(model);
        }
    }
}
