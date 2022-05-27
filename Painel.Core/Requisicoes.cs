using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Painel.Core
{
    public class Requisicoes
    {
        public static string GET(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream s = response.GetResponseStream())
                using (TextReader textReader = new StreamReader(s, Encoding.UTF8))
                {
                    return textReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string POST(Encoding enc, string url, string jsonContent)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                Encoding encoding = Encoding.UTF8;
                Byte[] byteArray = encoding.GetBytes(jsonContent);

                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream s = response.GetResponseStream())
                using (TextReader textReader = new StreamReader(s, enc))
                {
                    var x = textReader.ReadToEnd();
                    return x;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
