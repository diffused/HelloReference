using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ServiceStack.OrmLite;

namespace HelloReference
{
    public interface IRepository
    {
        IDbConnectionFactory DbFactory { get; set; }
        Greeting GetGreeting();
    }

    public class Repository : IRepository
    {
        public IDbConnectionFactory DbFactory { get; set; }        

        public Greeting GetGreeting()
        {
            using (var db = DbFactory.Open())
            {
                return db.First<Greeting>(a => a.Greet == "Howdy");
            }            
        }
    }
}