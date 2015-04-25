using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MeetPub.DataObjects;
using MeetPub.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace MeetPub.Controllers
{
    public class PubController : TableController<Pub>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Pub>(context, Request, Services);
        }

        // GET tables/Pub
        public IQueryable<PubDTO> GetAllPub()
        {
            var pubs = Query().Include("Assistance");
            return pubs.Select(x => new PubDTO
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Going = x.Assitances.Count()
            });
        }

        // GET tables/Pub/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PubDTO> GetPub(string id)
        {
            var result = Lookup(id).Queryable.Select(x => new PubDTO
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Going = x.Assitances.Count()
            });

            return SingleResult<PubDTO>.Create(result);
        }

        // PATCH tables/Pub/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Pub> PatchPub(string id, Delta<Pub> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Pub/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PostPub(Pub item)
        {
            Pub current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Pub/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePub(string id)
        {
             return DeleteAsync(id);
        }
    }
}