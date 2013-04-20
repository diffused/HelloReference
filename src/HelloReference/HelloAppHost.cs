using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Funq;
using HelloReference.ServiceInterfaces;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;



namespace HelloReference
{
    /// <summary>
    /// asp.net/iis hosting uses AppHostBase.
    /// 
    /// To do self-contained integration testing, a self hosted app 
    /// deriving from AppHostHttpListenerBase is needed. 
    /// Run as Administrator in order to spawn SelfHelloAppHost
    /// 
    /// Useful reference: http://rossipedia.com/blog/2013/02/27/integration-testing-with-servicestack/
    /// </summary>

    public class WebHelloAppHost : AppHostBase
    {
        IAppHostConfig _appHostCommon;

        public WebHelloAppHost(IAppHostConfig appHostCommon)
            : base("Hello WS", typeof(HelloService).Assembly)

        {
            _appHostCommon = appHostCommon;
        }

        public override void Configure(Container container)
        {
            _appHostCommon.Init(container);
        }
    }

    public class SelfHelloAppHost : AppHostHttpListenerBase
    {
        IAppHostConfig _appHostCommon;

        public SelfHelloAppHost(IAppHostConfig appHostCommon)
            : base("Hello WS", typeof(HelloService).Assembly)
        {
            _appHostCommon = appHostCommon;
        }

        public override void Configure(Container container)
        {
            _appHostCommon.Init(container);  
        }
    }

    
    
    /// <summary>
    /// Injectable common config settings
    /// </summary>
    public interface IAppHostConfig
    {
        void Init(Container container);
        EndpointHostConfig GetConfig();
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
                EnableFeatures = Feature.All.Remove(Feature.Xml | Feature.Soap),
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
        
        //public static IEnumerable<IPlugin> GetPlugins()
        //{
        //    yield return new ValidationFeature();
        //}
    }



}