namespace MeetPub.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.OData;
    using DataObjects;
    using Microsoft.WindowsAzure.Mobile.Service;
    using Microsoft.WindowsAzure.Mobile.Service.Notifications;
    using Microsoft.WindowsAzure.Mobile.Service.Security;
    using Models;

    [AuthorizeLevel(AuthorizationLevel.User)]
    public class AssistanceController : TableController<Assistance>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Assistance>(context, Request, Services);
        }

        // GET tables/Asistance
        //[QueryableExpandAttribute("Pub")]
        public IQueryable<Assistance> GetAllAsistance()
        {
            return Query().Include("Pub");
        }

        // GET tables/Asistance/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [QueryableExpandAttribute("Pub")]
        public SingleResult<Assistance> GetAsistance(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Asistance/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<Assistance> PatchAsistance(string id, Delta<Assistance> patch)
        {
            var result = await UpdateAsync(id, patch);

            await this.SendNotificationAsync(result);

            return result;
        }

        // POST tables/Asistance/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PostAsistance(Assistance item)
        {
            if (item.User == null)
            {
                var serviceUser = User as ServiceUser;
                if (serviceUser != null)
                {
                    item.User = serviceUser.Id;
                }
            }

            Assistance current = await InsertAsync(item);

            await this.SendNotificationAsync(current);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Asistance/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteAsistance(string id)
        {
            return DeleteAsync(id);
        }

        private async Task SendNotificationAsync(Assistance asistance)
        {
            var assistance = Query()
                .Where(a => a.PubID == asistance.PubID && a.User != null)
                .ToArray();

            if (assistance.Length > 0)
            {
                var message = string.Format("Cantidad de gente yendo a {0}",
                    assistance.Select(x => x.User).Distinct().Count());

                //var gmessage = new GooglePushMessage();
                //gmessage.Data.Add("message", message);
                //gmessage.Add("title", "Push Notification Sample");
                //gmessage.Add("msgcnt", "2");
                //await Services.Push.SendAsync(gmessage);

                var wmessage = new WindowsPushMessage
                {
                    XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                 @"<toast><visual><binding template=""ToastText01"">" +
                                 @"<text id=""1"">" + message + @"</text>" +
                                 @"</binding></visual></toast>"
                };
                await Services.Push.SendAsync(wmessage);
            }
        }
    }

    public class QueryableExpandAttribute : ActionFilterAttribute
    {
        private const string ODataExpandOption = "$expand=";

        public QueryableExpandAttribute(string expand)
        {
            this.AlwaysExpand = expand;
        }

        public string AlwaysExpand { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var query = request.RequestUri.Query.Substring(1);
            var parts = query.Split('&').ToList();
            var foundExpand = false;
            for (var i = 0; i < parts.Count; i++)
            {
                var segment = parts[i];
                if (!segment.StartsWith(ODataExpandOption, StringComparison.Ordinal))
                {
                    continue;
                }

                foundExpand = true;
                parts[i] += "," + this.AlwaysExpand;
                break;
            }

            if (!foundExpand)
            {
                parts.Add(ODataExpandOption + this.AlwaysExpand);
            }

            var modifiedRequestUri = new UriBuilder(request.RequestUri)
            {
                Query = string.Join("&", parts.Where(p => p.Length > 0))
            };

            request.RequestUri = modifiedRequestUri.Uri;
            base.OnActionExecuting(actionContext);
        }
    }
}