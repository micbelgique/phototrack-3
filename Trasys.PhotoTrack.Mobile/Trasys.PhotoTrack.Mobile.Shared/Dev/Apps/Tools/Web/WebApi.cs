using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Web
{
    public class WebApi
    {
        public WebApi(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        /// <summary>
        /// Gets or sets the Base URL to use with Get and Post methods.
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// Call the webAPI in get mode and returns the received result.
        /// </summary>
        /// <typeparam name="TDataReceived">Type of data to received</typeparam>
        /// <param name="api">WebAPI to call (api/Profile/Login)</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns></returns>
        public async Task<TDataReceived> Get<TDataReceived>(string api, params object[] args)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri serverBarUri = new Uri(this.BaseUrl);

                Trasys.Dev.Tools.Logger.Trace.Debug("WebApi.Get(\"{0}\"", String.Format(api, args));

                var response = await client.GetAsync(new Uri(serverBarUri, String.Format(api, args)));
                var jsonString = await response.Content.ReadAsStringAsync();

                Trasys.Dev.Tools.Logger.Trace.Debug("--> Response: {0}", jsonString);

                if (response.IsSuccessStatusCode)
                {
                    return JsonDeserialize<TDataReceived>(jsonString);
                }
                else
                {
                    return default(TDataReceived);
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return default(TDataReceived);
            }
        }

        /// <summary>
        /// Post some data to a specific WebAPI and returns the received result.
        /// </summary>
        /// <typeparam name="TDataToSend">Type of data to send</typeparam>
        /// <typeparam name="TDataReceived">Type of data to received</typeparam>
        /// <param name="api">WebAPI to call (api/Profile/Login)</param>
        /// <param name="data">Data to send</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns></returns>
        public async Task<TDataReceived> Post<TDataReceived, TDataToSend>(string api, TDataToSend data, params object[] args)
        {
            HttpClient client = new HttpClient();
            Uri serverBarUri = new Uri(this.BaseUrl);

            string jsonToSend = JsonSerialize<TDataToSend>(data);

            Trasys.Dev.Tools.Logger.Trace.Debug("WebApi.Post(\"{0}\", \"{1}\"", String.Format(api, args), jsonToSend);

            StringContent clientContent = new StringContent(jsonToSend, Encoding.Unicode, "application/json");
            var response = await client.PostAsync(new Uri(serverBarUri, String.Format(api, args)), clientContent);
            var jsonString = await response.Content.ReadAsStringAsync();

            Trasys.Dev.Tools.Logger.Trace.Debug("--> Response: {0}", jsonString);

            if (response.IsSuccessStatusCode)
            {
                return JsonDeserialize<TDataReceived>(jsonString);
            }
            else
            {
                return default(TDataReceived);
            }

        }

        /// <summary>
        /// Serializes the specified object to an associated JSON string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public string JsonSerialize<T>(T data)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
            ser.WriteObject(stream1, data);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Deserializes the specified JSON string to a typed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T JsonDeserialize<T>(string json)
        {

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            //stream.Position = 0;

            try
            {
                stream.Position = 0;
                var settings = new DataContractJsonSerializerSettings() { DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss") };
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), settings);
                return (T)ser.ReadObject(stream);
            }
            catch (SerializationException ex)
            {
                // To bypass the problem of DateTime deserialization
                stream.Position = 0;
                var settings = new DataContractJsonSerializerSettings() { DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fff") };
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), settings);
                return (T)ser.ReadObject(stream);
            }

        }

    }
}
