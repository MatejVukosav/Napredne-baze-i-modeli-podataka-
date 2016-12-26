using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using MongoDB.Driver;

namespace NBMP_1.projekt.Mongo
{



    public class MongoConfig
    {

        public MongoConfig()
        {
          


        }

        public static IMongoDatabase getMongoDatabase()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("newsdb");
            return database;
        }
    }
}
