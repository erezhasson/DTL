using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;

namespace MT4Connector
{

    public partial class frmMT4Connector : Form
    {
        const string c_Baseuri = "http://digtolive.hopto.org:55239/DTLGame";
        FileSystemWatcher watcher = new FileSystemWatcher();


        bool bToServer = false, bToLog = false;
        public frmMT4Connector()
        {
            InitializeComponent();
            
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (chkToServer.Checked || chkToLog.Checked)
            {
                btnStart.Visible = false;
                bToServer = chkToServer.Checked;
                bToLog = chkToLog.Checked;
                chkToServer.Visible = false;
                chkToLog.Visible = false;
                InitializeFileWatcher();
            }
            else
                MessageBox.Show("Check one of the options below");

        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private  void InitializeFileWatcher()
        {
      

            // Create a new FileSystemWatcher and set its properties.
                watcher.Path = txtWorkingFolder.Text;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files 
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName;


            // Only watch DTL_FromMT4 files when created.
                 watcher.Filter = "DTL_FromMT4*.txt";
 
                // Add event handlers.
                //watcher.Changed += OnFileCreated;
                watcher.Created += OnFileCreated;
                //watcher.Deleted += OnFileCreated;
                //watcher.Renamed += OnChaOnFileCreatednged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

        }

        // Define the event handlers.
        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            string FileWithPath = e.FullPath;
            string FileName = Path.GetFileNameWithoutExtension(e.FullPath);
            ProcessDTL_FromMT4Data(FileName);
        }

        private void ProcessDTL_FromMT4Data(string strData)
        {
            string StrInData = strData.Replace("DTL_FromMT4_","");
            char ch = '_';
            string[] InData = StrInData.Split(ch);

            switch (InData[0]) //Type of data
            {
                case "Price":
                    string BidPrice = (InData[1]);
                    string AskPrice = (InData[2]);
                    //Display price
                    this.txtLastBidPrice.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        this.txtLastBidPrice.Text = BidPrice;
                    });
                    this.txtLastAskPrice.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        this.txtLastAskPrice.Text = AskPrice;
                    });
                    this.txtLastPriceTime.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        this.txtLastPriceTime.Text = DateTime.Now.TimeOfDay.ToString();
                    });

                    if (bToServer)
                    {
                        //Send to Server
                        Newtonsoft.Json.Linq.JToken ServerData = null;
                        string ServerError = "";
                        if (HttpPostWrapper(p_Controller: "DTLMT4",
                            p_OP: "Set_Price",
                            p_jsnInData: "{\"BidPrice\":\"" + BidPrice + "\"+\"AskPrice\":\"" + AskPrice + "\"}",
                            out ServerData, out ServerError))
                        {
                            // txtNewPrice.Text = "Price set succeded";
                        }
                        else
                            MessageBox.Show("Error:" + ServerError);
                    }

                    if (bToLog)
                    {

                        string path = "c:\\MT4DTLData\\" + DateTime.Now.ToString("yyyyMMdd") + ".dat";
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "," + BidPrice.ToString() + "," + AskPrice.ToString());
                        }

                        
                    }
                    break;
                default:
                    break;
            }

        }



            public bool HttpPostWrapper(string p_Controller,
          string p_OP, string p_jsnInData,
          out Newtonsoft.Json.Linq.JToken p_jsnOutData, out string p_jsnError)
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
                    string StatusCode = (string)OServerData["StatusCode"];
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
                        response.StatusCode + " and ReasonPhrase:" + response.ReasonPhrase + "}";
                    return false;
                }

            }
        }

        private void btnTestLogCreation_Click(object sender, EventArgs e)
        {
            string path = "c:\\MT4DTLData\\"+ DateTime.Now.ToString("yyyyMMdd")+".dat";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ","+ "1.2133");
            }
            MessageBox.Show("Done");

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                Newtonsoft.Json.Linq.JToken ServerData = null;
                string ServerError = "";
                if (HttpPostWrapper(p_Controller: "DTLMT4",
                    p_OP: "Set_Price",
                    p_jsnInData: "{\"Price\":\"" + "22" + "\"}",
                    out ServerData, out ServerError))
                {
                    MessageBox.Show("Price set succeded");
                }
                else
                    MessageBox.Show("Error:" + ServerError);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }

        }
    }
}
