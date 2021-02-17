using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerGateway
{
     const int HttpStatusCodeOK = (int)System.Net.HttpStatusCode.OK;

     const string c_Baseuri = "http://localhost:55239";

     public static IEnumerator HttpPostWrapper(string p_Controller,
                 string p_OP, string p_jsnInData,
                System.Action<string, string> callbackOnFinish)
     {
          WWWForm form = new WWWForm();
          form.AddField("op", p_OP);
          form.AddField("jsnInData", p_jsnInData);

          UnityWebRequest www = UnityWebRequest.Post(c_Baseuri + "/" + p_Controller, form);
          yield return www.SendWebRequest();

          if (www.isNetworkError || www.isHttpError)
          {
               callbackOnFinish(null, www.error);

          }
          else
          {
               string strServerResponseData = www.downloadHandler.text;
               mdDataFromServer mdServerData = JsonUtility.FromJson<mdDataFromServer>(strServerResponseData);

               string StatusCode = mdServerData.StatusCode;
               if (StatusCode != HttpStatusCodeOK.ToString())
               {
                    string p_jsnError = mdServerData.jsnDataOut;
                    callbackOnFinish(null, p_jsnError);
               }
               else
               {
                    string jsnDataOut = mdServerData.jsnDataOut;

                    callbackOnFinish(jsnDataOut, null);
               }
          }
     }
}

public class mdDataFromServer
{
     public string StatusCode;
     public string jsnDataOut;
}