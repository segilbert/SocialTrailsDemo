//
using System;
using System.Collections.Generic;
using System.Configuration;
//
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;
//
using Spring.Social.Twitter.Api;
using Spring.Social.Twitter.Api.Impl;

namespace SocialTrailsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }


        static private void Run()
        {
            string consumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"]; // The application's consumer key
            string consumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"]; // The application's consumer secret
            string accessToken = ConfigurationManager.AppSettings["twitterOAuthAccessToken"]; // The access token granted after OAuth authorization
            string accessTokenSecret = ConfigurationManager.AppSettings["twitterOAuthAccessTokenSecret"]; // The access token secret granted after OAuth authorization

            ITwitter twitter = new TwitterTemplate(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            using (var store = new EmbeddableDocumentStore
            {
                DataDirectory = "~/App_Data/Database",
                RunInMemory = true,
                UseEmbeddedHttpServer = true,
            })
            {
                NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(9090);
                store.Initialize();
                store.HttpServer.SystemConfiguration.AllowLocalAccessWithoutAuthorization = true;

                Console.Write("RavenDb Embedded Document Store Initialized Successfully.");

                PerformInitialTimelineLoad(store, twitter, "frozenbytes");

                Console.ReadKey();
            }
        }

        private static void PerformInitialTimelineLoad(IDocumentStore store, ITwitter ctx, string psScreenName)
        {
            using (IDocumentSession s = store.OpenSession())
            {
                IList<Tweet> tweets = GetUserTimeLine(ctx, psScreenName);

                foreach (var tweet in tweets)
                {
                    Console.WriteLine("{0} - Message -> {1}", tweet.User.ScreenName, tweet.Text);
                    s.Store(tweet);
                }

                s.SaveChanges();
            }
        }

        private static IList<Tweet> GetUserTimeLine(ITwitter pxTwitterCtx, string psScreenName)
        {
            ulong statusesCount = 1400;
            int i = Convert.ToInt32(statusesCount);

            IList<Tweet> twits = new List<Tweet>();

            while (i > 0)
            {
                // sinceId and maxId
                var task = pxTwitterCtx.TimelineOperations.GetUserTimelineAsync(psScreenName, 200);
                twits.AddRange(task.Result);
                i = i - 200;
            }

            return twits;
        }

    }
}
