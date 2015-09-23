using System;
using System.Net;
using System.Windows;
using FlickrNet;

namespace FlickrUploader
{
    public class FlickrManager
    {
		public const string ApiKey = "85979c4b145042ed5e8bebb119a42b30";
		public const string SharedSecret = "a19fe8efa236ebb0";

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(ApiKey, SharedSecret);
            f.OAuthAccessToken = OAuthToken.Token;
            f.OAuthAccessTokenSecret = OAuthToken.TokenSecret;
            return f;
        }

        public static OAuthAccessToken OAuthToken
        {
            get
            {
                return Properties.Settings.Default.OAuthToken;
            }
            set
            {
                Properties.Settings.Default.OAuthToken = value;
                Properties.Settings.Default.Save();
            }
        }

    }
}
