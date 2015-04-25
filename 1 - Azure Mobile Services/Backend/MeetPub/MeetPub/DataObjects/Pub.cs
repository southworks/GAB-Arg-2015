namespace MeetPub.DataObjects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.WindowsAzure.Mobile.Service;

    public class Pub : EntityData
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public virtual ICollection<Assistance> Assitances { get; set; }
    }

    public class PubDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Going { get; set; }
    }
}