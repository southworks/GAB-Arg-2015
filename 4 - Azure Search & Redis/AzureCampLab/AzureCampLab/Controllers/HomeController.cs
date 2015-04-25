using AzureCampLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AzureCampLab.Controllers
{
    public class HomeController : Controller
    {
        public const string ApiVersionString = "api-version=2014-07-31-Preview";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel vm)
        {
            
            var httpClient = new HttpClient();
            var _serviceUri = new Uri("https://mvpstream.search.windows.net");
            if (string.IsNullOrEmpty(vm.Text))
            {
                vm.Text = "";
            }
            string search = "search=" + Uri.EscapeDataString(vm.Text);
            string facets = "";
            if (vm.Facets != null)
            {
                foreach (var f in vm.Facets)
                {
                    facets += "&facet=" + f;
                }
            }
            string orderby = "";
            if (!string.IsNullOrEmpty(vm.Order))
            {
                orderby = "&$orderby=" + vm.Order;
            }
            string filter = "";
            if (!string.IsNullOrEmpty(vm.Filter))
            {
                orderby = "&$filter=Tipo eq '" + vm.Filter+"'";
            }
            string paging = "&$top=25";


            httpClient.DefaultRequestHeaders.Add("api-key", "A8CFC2E83B464B6E1C0F0B5B9D603E6B");

            Uri uri = new Uri(_serviceUri, "/indexes/entries/docs?" + search + facets + paging + filter + orderby);

            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);
            var din = AzureSearchHelper.DeserializeJson<dynamic>(response.Content.ReadAsStringAsync().Result);
            return Content(din.ToString());
        }

        [HttpPost]
        public ActionResult BuildUri(SearchViewModel vm)
        {
            var _serviceUri = new Uri("https://mvpstream.search.windows.net");
            if (string.IsNullOrEmpty(vm.Text))
            {
                vm.Text = "";
            }
            string search = "search=" + Uri.EscapeDataString(vm.Text);
            string facets = "";
            if (vm.Facets != null)
            {
                foreach (var f in vm.Facets)
                {
                    facets += "&facet=" + f;
                }
            }
            string orderby = "";
            if (!string.IsNullOrEmpty(vm.Order))
            {
                orderby = "&$orderby=" + vm.Order;
            }
            string filter = "";
            if (!string.IsNullOrEmpty(vm.Filter))
            {
                orderby = "&$filter=Tipo eq '" + vm.Filter + "'";
            }
            string paging = "&$top=25";


            Uri uri = new Uri(_serviceUri, "/indexes/entries/docs?" + search + facets + paging + filter + orderby);
            return Content(uri.ToString());
        }
    }
}