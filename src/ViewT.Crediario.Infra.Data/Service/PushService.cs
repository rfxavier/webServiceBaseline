using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using ViewT.Crediario.Domain.Main.Interfaces;

namespace ViewT.Crediario.Infra.Data.Service
{
    public class PushService : IPushService
    {
        public void SendPush(string deviceToken, string title, string body)
        {
            try
            {
                
                Uri FireBasePushNotificationsURL = new Uri(ConfigurationManager.AppSettings["PushUri.Condominio"]);
                string ServerKey = ConfigurationManager.AppSettings["ServerKey.Condominio"];

                string[] deviceTokenArr = new string[] { deviceToken };
                var data = new { action = "Play" };

                if (deviceTokenArr.Length > 0)
                {
                    var messageInformation = new
                    {
                        notification = new
                        {
                            title = title,
                            text = body
                        },
                        data = data,
                        registration_ids = deviceToken
                    };

                    string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                    var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                    request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                    request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                    HttpResponseMessage result;
                    using (var client = new HttpClient())
                    {
                        result = client.SendAsync(request).Result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
