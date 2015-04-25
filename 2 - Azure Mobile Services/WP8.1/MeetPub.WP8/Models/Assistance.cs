namespace MeetPub.WP8.Models
{
    using System;

    using BarMeetUp.Models;

    public class Assistance
    {
        public string Id { get; set; }

        public string PubID { get; set; }

        public virtual Pub Pub { get; set; }

        public DateTime Date { get; set; }
    }
}