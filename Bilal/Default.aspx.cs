using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bilal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           



            try
            {
                // Post
                string strurl = String.Format("https://integrasi.delapancommerce.com/v1/test-new-employee");
            
            
            HttpWebRequest requestObjPost = (HttpWebRequest)WebRequest.Create(strurl);
            requestObjPost.Credentials = new NetworkCredential("linc-test", "123456");

            string SignatureKey = "879sdg78dsfg56sd4g7987eswg76";
            requestObjPost.ContentType = "application/json";
            
            string urldata = "/v1/test-new-employee";
            string postdata = "\"Email\":\"bilal.hardi@gmail.com\"";
            string postdataHash = MD5Hash(postdata);
            string RawSignature = requestObjPost.Method.ToString() + "\n" + postdataHash + "\n" +
                                    requestObjPost.ContentType.ToString() +
                                "\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n" + urldata;
            string FinalSignature = CreateToken(RawSignature, SignatureKey);

            // Header
            requestObjPost.Method = "POST";

         
            requestObjPost.Accept= "application/json";
            requestObjPost.Headers.Add("API-Key", "ojh545we4t5254sdgfsaefstg65478");
            requestObjPost.Headers.Add("Signature", FinalSignature);
            requestObjPost.Headers.Add("Signature-Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //

           

            
            using (var StreamWriter = new StreamWriter (requestObjPost.GetRequestStream()))
            {
                StreamWriter.Write(postdata);
                StreamWriter.Flush();
                StreamWriter.Close();

                var httpresponse =  (HttpWebResponse)requestObjPost.GetResponse();
                using (var StreamReader = new StreamReader(httpresponse.GetResponseStream()))
                {
                    var result2 = StreamReader.ReadToEnd();
                }

            }
            }
            catch (System.Net.WebException)
            {
                //Print(e.Message);
            }
        }

        // MD5
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        //HMAC-SHA-256
        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}