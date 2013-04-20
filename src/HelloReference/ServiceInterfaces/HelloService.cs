using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelloReference.ServiceModels;
using ServiceStack.ServiceInterface;


namespace HelloReference.ServiceInterfaces
{
    public class HelloService : Service
    {
        public IRepository Repository { get; set; }

        public object Get(Hello request)
        {
            var greeting = Repository.GetGreeting();            
            
            return new HelloResponse { Result = greeting.Greet + ", " + request.Name };
        }
    }

    
}