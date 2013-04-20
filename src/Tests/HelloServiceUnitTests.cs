using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using ServiceStack.ServiceInterface.Testing;
using ServiceStack.ServiceInterface;
using HelloReference;
using Funq;
using ServiceStack.Text;
using ServiceStack.Service;
using ServiceStack.WebHost.Endpoints.Support;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Common.Web;
using HelloReference.ServiceModels;
using HelloReference.ServiceInterfaces;
using Moq;

using System.Data;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Common;



namespace Tests
{
    
    public class HelloServiceUnitTests
    {
        [Fact]
        public void Any_returns_greeting_with_name()
        {
            var request = new Hello { Name = "integration name" };

            var mockRequestContext = new MockRequestContext();

            var mockRespository = new Mock<IRepository>();
            mockRespository.Setup(a => a.GetGreeting())
                .Returns(new Greeting { Greet = "Hiya" });
            
            var service = new HelloService { 
                RequestContext = mockRequestContext,
                Repository = mockRespository.Object
            };
            

            HelloResponse response = (HelloResponse)service.Get(request);

            response.Result.Should().Be("Hiya, " + request.Name); 

        }        
    }

    
}


