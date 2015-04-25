using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MeetPub.DataObjects;
using MeetPub.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MeetPub
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            Random random = new Random();
            var list = new List<Pub>
            {
                new Pub
                {
                    Id = "00ed9acb-7dbd-44a7-9b68-bb64b22b0154",
                    Name = "Chabres Bar de Tragos",
                    Location = "Location",
                    Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "00ed9acb-7dbd-44a7-9b68-bb64b22b0154",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "00ed9acb-7dbd-44a7-9b68-bb64b22b0154",
                           User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "00ed9acb-7dbd-44a7-9b68-bb64b22b0154",
                           User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        }
                    }
                },
                new Pub
                {
                    Id = "358db632-c5df-4183-9d33-0afdbc33a4fb",
                    Name = "The Kilkenny",
                    Location = "Location",
                    Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "358db632-c5df-4183-9d33-0afdbc33a4fb",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        }
                    }
                },
                new Pub
                {
                    Id = "3e37276e-49de-4baf-8fe1-5a1b2606c065",
                    Name = "Gibraltar",
                    Location = "Location",
                     Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "3e37276e-49de-4baf-8fe1-5a1b2606c065",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "3e37276e-49de-4baf-8fe1-5a1b2606c065",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "3e37276e-49de-4baf-8fe1-5a1b2606c065",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        }
                    }
                },
                new Pub
                {
                    Id = "564e5184-de94-41e0-aa04-73722dc209b0",
                    Name = "Viejitos",
                    Location = "Location",
                     Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "564e5184-de94-41e0-aa04-73722dc209b0",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "564e5184-de94-41e0-aa04-73722dc209b0",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "564e5184-de94-41e0-aa04-73722dc209b0",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        }
                    }
                },
                new Pub
                {
                    Id = "734d2b9f-83e3-47f8-b2c0-6d4104dad2b2",
                    Name = "Puerta Roja",
                    Location = "Location",
                     Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID ="734d2b9f-83e3-47f8-b2c0-6d4104dad2b2",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "734d2b9f-83e3-47f8-b2c0-6d4104dad2b2",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today.AddDays(-1)
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID ="734d2b9f-83e3-47f8-b2c0-6d4104dad2b2",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today.AddDays(-2)
                        }
                    }
                },
                new Pub
                {
                    Id = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                    Name = "Doppelganger Bar",
                    Location = "Location",
                     Assitances = new List<Assistance>()
                    {
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                         new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        },
                        new Assistance
                        {
                            Id = Guid.NewGuid().ToString(),
                            PubID = "ae1b239c-ed31-43f1-a461-49d8871ef87d",
                            User = string.Format("{0}:{1}", "Fake", random.Next(1000, 9999)),
                            Date = DateTime.Today
                        }
                    }
                }
            };

            context.Set<Pub>().AddRange(list);

            base.Seed(context);
        }
    }
}

