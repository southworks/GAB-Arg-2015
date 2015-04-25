using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureCampLab.Models
{
    public class SearchViewModel
    {
        public List<string> Facets { get; set; }
        public string Order { get; set; }
        public string Filter { get; set; }
        public string Text { get; set; }
    }
}