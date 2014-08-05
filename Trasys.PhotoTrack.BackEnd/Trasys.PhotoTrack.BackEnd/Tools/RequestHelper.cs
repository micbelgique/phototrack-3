using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Trasys.PhotoTrack.BackEnd.Tools
{
    public class RequestHelper<T> where T : class
    {
        public T GetRequest(string serviceUrl)
        {
            using (var client = new HttpClient())
            {

                string baseUrl = Properties.Settings.Default.DataServicesLayerUrl;
                client.BaseAddress = new Uri(baseUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(serviceUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var yourcustomobjects = response.Content.ReadAsAsync<T>().Result;
                    return yourcustomobjects;
                }
                else
                {
                    return null;
                }
            }
        }
        public T GetRequestToken(string serviceUrl, string token)
        {
            serviceUrl = string.Format("{0}/{1}", token, serviceUrl);
            return GetRequest(serviceUrl);
        }
        public bool PostRequestTokenBool(string serviceUrl, T parameters, string token)
        {
            serviceUrl = string.Format("{0}/{1}", token, serviceUrl);
            return PostRequestBoolean(serviceUrl, parameters);
        }

        public bool PostRequestBoolean(string serviceUrl, T parameters)
        {
            using (var client = new HttpClient())
            {

                string baseUrl = Properties.Settings.Default.DataServicesLayerUrl;
                client.BaseAddress = new Uri(baseUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(serviceUrl, parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string tmp = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<bool>(tmp);
                    return result;
                }
                else
                {
                    return false;
                }
            }
        }

    }

    /// <summary>
    /// Class allow to send data in order to retrieve data
    /// </summary>
    /// <typeparam name="T">Object sended</typeparam>
    /// <typeparam name="U">Object wanted</typeparam>
    public class RequestHelper<T, U>
        where T : class
        where U : class
    {

        public U PostRequest(string serviceUrl, T parameters)
        {
            using (var client = new HttpClient())
            {

                string baseUrl = Properties.Settings.Default.DataServicesLayerUrl;
                client.BaseAddress = new Uri(baseUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(serviceUrl, parameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    string tmp = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<U>(tmp);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public U PostRequestToken(string serviceUrl, T parameters, string token)
        {
            serviceUrl = string.Format("{0}/{1}", token, serviceUrl);
            return PostRequest(serviceUrl, parameters);
        }



    }
}