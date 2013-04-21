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
            this.SetConfig(_appHostCommon.GetConfig());
            this.Plugins.AddRange(_appHostCommon.GetPlugins());
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
            this.SetConfig(_appHostCommon.GetConfig());
            this.Plugins.AddRange(_appHostCommon.GetPlugins());
        }
    }

    
    
    



}