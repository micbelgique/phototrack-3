using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Trasys.PhotoTrack.BackEnd.Tools
{
    public static class HtmlHelperExtensions
    {
        public static string GravatarUrl(this HtmlHelper context, string emailAddress, string defaultImage)
        {
            string hash = string.Empty;
            string defaultUrl = string.Concat("{0}{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath, defaultImage);
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, emailAddress);
            }
            return string.Format("http://www.gravatar.com/avatar/{0}?d={1}&s=64", hash, defaultUrl, 64);
        }


        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}