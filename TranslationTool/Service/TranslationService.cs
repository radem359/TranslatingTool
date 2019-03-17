using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TranslationTool.Models;

namespace TranslationTool.Service
{
    public class TranslationService
    {

        public Translation WebServiceTranslate(string SerbianTranslation)
        {
            /*
             string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", translation.SerbianTranslation, "sr|en");
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string result = webClient.DownloadString(url);
            result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
            result = result.Substring(result.IndexOf(">") + 1);
            result = result.Substring(0, result.IndexOf("</span>"));

            translation.EnglishTranslation = result.Trim();
             
            TranslationClient client = TranslationClient.Create();
            var result = client.TranslateText(translation.SerbianTranslation, "en", "sr");
            translation.EnglishTranslation = result.ToString();*/

            string fromCulture = "sr";
            string toCulture = "en";

            //normalize the culture in case something like en - us was passed
            // retrieve only en since Google doesn't support sub-locales
            string[] tokens = fromCulture.Split('-');
            if (tokens.Length > 1)
                fromCulture = tokens[0];

            //normalize ToCulture
            tokens = toCulture.Split('-');
            if (tokens.Length > 1)
                toCulture = tokens[0];

            string url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                       HttpUtility.UrlEncode(SerbianTranslation), fromCulture, toCulture);

            //Retrieve Translation with HTTP GET call
            string html = null;
            try
            {
                WebClient web = new WebClient();

                //    MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
                web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

                //Make sure we have response encoding to UTF-8
                web.Encoding = Encoding.UTF8;
                html = web.DownloadString(url);
            }
            catch (Exception ex)
            {
                //    this.ErrorMessage = Westwind.Globalization.Resources.Resources.ConnectionFailed + ": " +
                //                       ex.GetBaseException().Message;
                return null;
            }

            //Extract out trans":"...[Extracted]...","from the JSON string
            string result = Regex.Match(html, "trans\":(\".*?\"),\"", RegexOptions.IgnoreCase).Groups[1].Value;

            if (string.IsNullOrEmpty(result))
            {
                //this.ErrorMessage = Westwind.Globalization.Resources.Resources.InvalidSearchResult;
                return null;
            }

            Translation translation = new Translation();
            translation.SerbianTranslation = SerbianTranslation;

            translation.EnglishTranslation = result;

            DateTime currentDateAndTime = DateTime.Now;

            translation.SavedDateAndTime = currentDateAndTime;
            translation.LastTimeCalledDateAndTime = currentDateAndTime;

            return translation;
        }

    }
}