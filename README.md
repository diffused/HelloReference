#HelloReference

A simple and focused reference project to demonstrate a ServiceStack API hosted in ASP.Net/IIS talking to SqlServer, wired up for unit and integration testing. 

One GET endpoint: `/hello/{string}`

    GET /hello/nachos   
    
returns 

    Howdy, nachos

##Overview
- `HelloReference.HelloAppHost.WebHelloAppHost` enables ASP.Net/IIS hosting.
- `HelloReference.HelloAppHost.SelfHelloAppHost` provides a self hosting instance that can be initialized during an integration test 
 
`IAppHostConfig` defines a commmon configuration interface that can be passed into the constructors of `WebHelloAppHost` and `SelfHelloAppHost`

##Reading a database 
The `ServiceStack.Service` interface exposes a readonly `IDbConnection`.

So, DB functionality is provided using an `IRepository` property on `HelloService`

    public class HelloService : Service
    {
        public IRepository Repository { get; set; } // autowired
    }

ServiceStack's IOC auto wires the dependencies as defined in `AppHostConfig.Init()`.


##Setup

This project uses a standad Sql Server Database connecting with Integrated Security. 

    Data Source=.;Initial Catalog=HelloDb;Integrated Security=True;MultipleActiveResultSets=True
    
If you have Sql Server, then in Server Explorer simply create a new Database. 

##Testing
Xunit, FluentAssertations and MOQ are used. 

###Unit testing
`HelloServiceUnitTests` provides a simple demonstration test using `ServiceStack.ServiceInterface.Testing.MockRequestContext`

in Tests.HelloServiceUnitTests

    var mockRespository = new Mock<IRepository>();
    mockRespository.Setup(a => a.GetGreeting())
        .Returns(new Greeting { Greet = "Hiya" });
    
    var service = new HelloService { 
        RequestContext = mockRequestContext,
        Repository = mockRespository.Object
    };

###Integration testing
`Tests.HelloAppHostFixture` sets up a self-hosted `SelfHelloAppHost` service instance. 
An `IAppHostConfig` is passed in and the app is started.

in Tests.HelloAppHostFixture

    _appHost = new SelfHelloAppHost(new IntegrationTestAppHostConfig());
    _appHost.Init();
    _appHost.Start(Config.AbsoluteBaseUri);

Ensure `Stop()` is called when you are done. 

In Tests.HelloAppHostFixture.Dispose (Fixture teardown)

    _appHost.Stop();



##Stuff
The intention was to gather the basics of writing testable code with ServiceStack into a focused reference project. 

Hopefully it helps. Let me know if I've got it all wrong. 

