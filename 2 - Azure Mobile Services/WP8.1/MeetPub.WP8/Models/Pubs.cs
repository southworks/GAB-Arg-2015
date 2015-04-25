namespace BarMeetUp.Models
{
    using System.Collections.ObjectModel;

    public class MainPageViewModel
    {
        public Pubs Pubs { get; set; }
    }

    public class Pub
    {
        // Default constructor is required for usage as sample data 
        // in the WPF and Silverlight Designer.
        public Pub()
        {
            
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Going { get; set; }
    }

    public class Pubs : ObservableCollection<Pub>
    {
        // Default constructor is required for usage in the WPF Designer.
        public Pubs() { }
    }
}
