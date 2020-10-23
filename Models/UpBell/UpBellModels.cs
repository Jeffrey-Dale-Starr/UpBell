using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpBell.Models.UpBell
{
    public class UpBellSchedule
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string FullStartDateTime { get; set; }
        public string FullEndDateTime { get; set; }
    }

    public class UpBellResponse
    {
        public string Message { get; set; }
        public List<UpBellSchedule> Schedules { get; set; }
        public List<UpBellEvent> Events { get; set; }

        public UpBellResponse()
        {
            Schedules = new List<UpBellSchedule>();
            Events = new List<UpBellEvent>();
        }
    }

    public class UpBellEvent
    {
        public string summary { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public UpBellEventDate start { get; set; }
        public UpBellEventDate end { get; set; }
        public List<string> recurrence { get; set; }
        public UpBellReminder reminders { get; set; }

        public UpBellEvent()
        {
            recurrence = new List<string> { "RRULE:FREQ=DAILY;COUNT=1" };
            reminders = new UpBellReminder();
            start = new UpBellEventDate();
            end = new UpBellEventDate();
        }
    }

    public class UpBellEventDate
    {
        public string dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class UpBellReminder
    {
        public bool useDefault { get; set; }
        public List<UpBellReminderOverride> overrides { get; set; }

        public UpBellReminder()
        {
            useDefault = false;
            overrides = new List<UpBellReminderOverride>
            {
                new UpBellReminderOverride { method = "email", minutes = 24 * 60 },
                new UpBellReminderOverride { method = "popup", minutes = 10 }
            };
        }
    }

    public class UpBellReminderOverride
    {
        public string method { get; set; }
        public int minutes { get; set; }
    }
}