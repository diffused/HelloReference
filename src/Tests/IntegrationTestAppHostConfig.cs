using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloReference;
using ServiceStack.WebHost.Endpoints;

namespace Tests
{
    public class IntegrationTestAppHostConfig : AppHostConfig
    {
        // reusing HelloReference.AppHostConfig 

        // but can implement own config here.     

        public IntegrationTestAppHostConfig()
        {
            // DataDirectory path connection string changes based on test runner. 
            // Force correct DataDirectory path 
            
        }
           
    }
}
