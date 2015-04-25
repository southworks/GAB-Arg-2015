namespace MeetPub.DataObjects
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.WindowsAzure.Mobile.Service;

    public class Assistance : EntityData
    {
        public string User { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        public string PubID { get; set; }

        public virtual Pub Pub { get; set; }

    }
}