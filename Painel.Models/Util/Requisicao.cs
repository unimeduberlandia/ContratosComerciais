using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.Util
{
    public static class Requisicao
    {
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
            catch (WebException ex)
            {
                throw;
            }
        }
    }
}
