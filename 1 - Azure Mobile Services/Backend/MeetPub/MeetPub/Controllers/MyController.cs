namespace MeetPub.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using DataObjects;
    using Microsoft.WindowsAzure.Mobile.Service;
    using Microsoft.WindowsAzure.Mobile.Service.Security;

    [AuthorizeLevel(AuthorizationLevel.User)]
    public class MyController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/My
        public Settings Get()
        {
            var context = new Models.MobileServiceContext();
            var user = User as ServiceUser;

            var queryResults = from item in context.Assistances
                               where item.User == user.Id
                               select item;

            var pub = queryResults.FirstOrDefault(x => x.Date == DateTime.Today);

            return new Settings
            {
                PubId = pub != null ? pub.PubID : string.Empty,
                Times = pub != null ? queryResults.Count(x => x.PubID == pub.PubID) : 0
            };
        }

    }
}
