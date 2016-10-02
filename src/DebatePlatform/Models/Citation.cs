using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DebatePlatform.Models
{
    [Table("Citations")]
    public class Citation
    {
        [Key]
        public int CitationId { get; set; }
        public int ArgumentId { get; set; }
        public string UserId { get; set; }

        public string Creator { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public string URL { get; set; }
        public string Date { get; set; }
        public string Institution { get; set; }
        public string Description { get; set; }

        public string Text { get; set; }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

        public static List<Citation> SearchPage(string subject, int page)
        {
            var client = new RestClient("http://api.dp.la/v2/");
            var request = new RestRequest("items?q="+subject+"&page="+page.ToString()+"&api_key=e397af2521d5bcc456af094aaa42d525", Method.GET);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Citation> result = new List<Citation>() { };
            for(var i=0; i<10; i++)
            {
                Regex rgx = new Regex("[\"{}()\\]\\[]");
                var doc = jsonResponse["docs"][i];
                Citation newCite = new Citation();
                var temp = doc["isShownAt"];
                newCite.URL = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["sourceResource"]["title"];
                newCite.Title = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["dataProvider"];
                newCite.Institution = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["sourceResource"]["description"];
                newCite.Description = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["sourceResource"]["creator"];
                newCite.Creator = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["sourceResource"]["date"];
                newCite.Date = temp == null ? null : rgx.Replace(temp.ToString(), "");
                temp = doc["sourceResource"]["format"];
                newCite.Format = temp == null ? null : rgx.Replace(temp.ToString(), "");

                result.Add(newCite);
            }
            return result;
        }
    }
}
