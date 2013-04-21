using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Funq;
using HelloReference.ServiceInterfaces;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

namespace HelloReference
{
    /// <summary>
    /// Injectable common config settings
    /// </summary>
    public interface IAppHostConfig
    {
        void Init(Container container);
        EndpointHostConfig GetConfig();
        IEnumerable<IPlugin> GetPlugins();
    }

    /// <summary>
    /// Concrete config gets passed during Global.asax Application_Start
    /// </summary>
    public class AppHostConfig : IAppHostConfig
    {
        public void Init(Container container)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["HelloDb"].ConnectionString;
            var dbFactory = new OrmLiteConnectionFactory(
                 connectionString,
                 true,
                 SqlServerDialect.Provider);

            initDb(dbFactory);


            container.RegisterAutoWiredAs<Repository, IRepository>().ReusedWithin(ReuseScope.None);

            container.RegisterAutoWired<HelloService>().ReusedWithin(ReuseScope.None);

            container.Register<IDbConnectionFactory>(dbFactory);
        }

        public EndpointHostConfig GetConfig()
        {
            return new EndpointHostConfig
            {
                EnableFeatures = Feature.All.Remove(
                    Feature.Xml | Feature.Soap | Feature.Csv | Feature.Jsv
                    ),
                DebugMode = true,
                DefaultContentType = ContentType.Json
            };
        }

        void initDb(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                db.DropAndCreateTable<Greeting>();
                db.Insert<Greeting>(new Greeting { Greet = "Howdy" });
            }
        }

        public IEnumerable<IPlugin> GetPlugins()
        {
            return new List<IPlugin>();
            
            // for example:

            //return new List<IPlugin> {
            //    new ServiceStack.ServiceInterface.Validation.ValidationFeature(),
            //    new ServiceStack.MetadataFeature()
            //};
        }
    }
}