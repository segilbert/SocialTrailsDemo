//
using System;
using System.Configuration;
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

            Console.WriteLine("Authenticated against Twitter Rest API using Spring.Net Application Framework Social Twitter Extensions.");
            Console.ReadKey();
        }


    }
}
