using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using UpBell.Models.UpBell;

namespace UpBell.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ProcessSchedules()
        {
            var response = new UpBellResponse();

            var f = Request.Files[0];

            var fileType = f.FileName.Substring(f.FileName.LastIndexOf('.') + 1).ToLower();

            if (fileType == "csv")
            {
                using (var reader = new StreamReader(f.InputStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        var s = new UpBellSchedule
                        {
                            Summary = values[0],
                            Location = values[1],
                            Description = values[2],
                            Date = values[3],
                            StartTime = values[4],
                            EndTime = values[5],
                            FullStartDateTime = $"{values[3]}T{values[4]}",
                            FullEndDateTime = $"{values[3]}T{values[5]}"
                        };

                        if (s.Summary != "Summary")
                        {
                            response.Schedules.Add(s);
                        }
                    }
                }
            }
            else
            {
                string json;

                using (StreamReader reader = new StreamReader(f.InputStream))
                {
                    json = reader.ReadToEnd();
                }
                response.Schedules = JsonConvert.DeserializeObject<List<UpBellSchedule>>(json);

                foreach (var s in response.Schedules)
                {
                    s.FullStartDateTime = $"{s.Date}T{s.StartTime}";
                    s.FullEndDateTime = $"{s.Date}T{s.EndTime}";
                }
            }

            var lstEvent = new List<UpBellEvent>();

            foreach (var s in response.Schedules)
            {
                var e = new UpBellEvent();
                e.description = s.Description;
                e.location = s.Location;
                e.start.dateTime = s.FullStartDateTime;
                e.start.timeZone = "America/Chicago";
                e.end.dateTime = s.FullEndDateTime;
                e.end.timeZone = "America/Chicago";
                e.summary = s.Summary;
                lstEvent.Add(e);
            }

            response.Message = "Success";
            response.Events = lstEvent;

            return Json(response); ;
        }
    }
}