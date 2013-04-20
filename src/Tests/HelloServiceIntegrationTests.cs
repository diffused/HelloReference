using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloReference;
using Funq;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Testing;
using ServiceStack.WebHost.Endpoints.Support;
using ServiceStack.Text;
using Xunit;
using FluentAssertions;
using HelloReference.ServiceModels;

namespace Tests
{
    public class HelloAppHostFixture : IDisposable
    {
        protected SelfHelloAppHost _appHost;

        public HelloAppHostFixture()
        {
            _appHost = new SelfHelloAppHost(new IntegrationTestAppHostConfig());
            _appHost.Init();
            _appHost.Start(Config.AbsoluteBaseUri);
        }

        public void Dispose()
        {
            _appHost.Stop();
        }
    }

    public class HelloServiceIntegrationTests : IUseFixture<HelloAppHostFixture>
    {
        public void SetFixture(HelloAppHostFixture data)
        {
        }
        

        JsonServiceClient newClient()
        {
            return new JsonServiceClient(Config.AbsoluteBaseUri);
        }

        [Fact]
        public void Get_Returns_Greeting_With_Name()
        {
            var request = new Hello { Name = "integration name" };

            var client = newClient();
            var response = client.Get(request);

            response.Result.Should().Be("Howdy, " + request.Name); 
        }

        [Fact]
        public void Second_Get()
        {
            var client = newClient();
            HelloResponse response = client.Get(new Hello { Name = "integration name" });
        }        
    }

    
}
