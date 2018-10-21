using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YandexTranslator
{
    interface ICache
    {
        string GetValue(string str);
        void AddText(string strSearch, string strResult);
    }
    class TextCache : ICache
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        public void AddText(string strSearch, string strResult)
        {
            result.Add(strSearch, strResult);
        }
        public string GetValue(string str)
        {
            if (result.ContainsKey(str))
            {
                return result[str];
            }
            return null;
        }
    }

    class TextSearchAPI
    {

        readonly string url = @"https://translate.yandex.net/api/v1.5/tr.json/translate";
        readonly string key = "trnsl.1.1.20181021T174622Z.b2c74b939e79f6e9.6b771cef0e997690aed1e0f7073dd7f14f89e1f4";
        ICache cache = new TextCache();
        public string Search(string text)
        {
            string result = cache.GetValue(text);
            if (result == null)
            {
                Console.WriteLine("Reading data from API...");
                var webClient = new WebClient();
                try
                {
                    var ApiResult = webClient.DownloadString($"{url}?key={key}&lang=en-ru&text={text}");
                    var data = JsonConvert.DeserializeObject(ApiResult);
                    result = data[0]["text"].ToString();
                    cache.AddText(text, result);
                }
                catch (Exception)
                {
                    throw new Exception("Translation not found");
                }
            }
            else
                Console.WriteLine("Reading data from cache");
            return result;
        }
        class Program
        {
            static void Main(string[] args)
            {
                string text = Console.ReadLine();
                TextSearchAPI textSearch = new TextSearchAPI();
                textSearch.Search(text);
            }
        }
    }
}
