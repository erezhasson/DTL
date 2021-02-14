using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebTester
{
    public partial class frmDTLMT4ControllerTester : Form
    {
  
        public frmDTLMT4ControllerTester()
        {
            InitializeComponent();
        }
        //const string c_Baseuri = "http://localhost:55239";
        const string c_Baseuri = "http://digtolive.hopto.org:55239/DTLGame";

        private void btnGet_Status_Click(object sender, EventArgs e)
        {
             Newtonsoft.Json.Linq.JToken jsnServerData = null;
            string ServerError = "";
            if (HttpPostWrapper( p_Controller: "DTLMT4",
                p_OP: "Get_Status",
                p_jsnInData: "",
                out jsnServerData, out ServerError))
            {
                if (jsnServerData != null)
                {
                    txtEnergyStarSize.Text = (string)jsnServerData["EnergyStarSize"];
                    txtEnergyStarAnimation.Text = (string)jsnServerData["EnergyStarAnimation"];
                }


            }
            else
                MessageBox.Show("Error:" + ServerError);



        }

        private void btnSet_Price_Click(object sender, EventArgs e)
        {
             Newtonsoft.Json.Linq.JToken ServerData = null;
            string  ServerError="";
            if (HttpPostWrapper( p_Controller: "DTLMT4",
                p_OP: "Set_Price",
                p_jsnInData: "{\"Price\":\"" + txtNewPrice.Text+"\"}",
                out ServerData, out ServerError))
                txtNewPrice.Text = "Price set succeded";
            else
                MessageBox.Show("Error:" + ServerError);

        }
   
        public bool HttpPostWrapper( string p_Controller,
            string p_OP,string p_jsnInData,
            out Newtonsoft.Json.Linq.JToken p_jsnOutData,out string p_jsnError)
        {
            int HttpStatusCodeOK = (int)HttpStatusCode.OK;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(c_Baseuri);

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("op", p_OP));
                keyValues.Add(new KeyValuePair<string, string>("jsnInData", p_jsnInData));
                var dataForHTTP = new FormUrlEncodedContent(keyValues);

                p_jsnOutData = "";
                p_jsnError = "";

                var response = client.PostAsync(p_Controller, dataForHTTP).Result;
                if (response.IsSuccessStatusCode)
                {
                    string ServerResponseData = response.Content.ReadAsStringAsync().Result;
                    Newtonsoft.Json.Linq.JContainer OServerData = 
                     Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(ServerResponseData);
                    string StatusCode=(string)OServerData["StatusCode"];
                    if (StatusCode != HttpStatusCodeOK.ToString())
                    {
                        p_jsnError = (string)OServerData["ErrorDescription"];
                        return false;
                    }

                    Newtonsoft.Json.Linq.JToken jsnDataOut = OServerData.Last;
                    p_jsnOutData = jsnDataOut.First; ;
                    return true;
                }
                else
                {
                    p_jsnError = "{'Source:':'HttpPostWrapper','Message':' Http post failed with StatusCode:' " +
                        response.StatusCode +" and ReasonPhrase:" + response.ReasonPhrase+"}";
                    return false;
                }
     
            }
        }


    }
}
