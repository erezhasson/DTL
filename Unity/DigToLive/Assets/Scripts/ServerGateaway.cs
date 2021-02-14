using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerGateway
{
     const int HttpStatusCodeOK = (int)System.Net.HttpStatusCode.OK;

     const string c_Baseuri = "http://localhost:55239";
     //const string c_Baseuri = "http://digtolive.hopto.org:55239";

     public static IEnumerator HttpPostWrapper(string p_Controller,
                string p_OP, string p_jsnInData,
               System.Action<Newtonsoft.Json.Linq.JToken, string> callbackOnFinish)
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
               string ServerResponseData = www.downloadHandler.text;

               Newtonsoft.Json.Linq.JContainer OServerData =
               Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(ServerResponseData);
               string StatusCode = (string)OServerData["StatusCode"];
               if (StatusCode != HttpStatusCodeOK.ToString())
               {
                    string p_jsnError = (string)OServerData["ErrorDescription"];
                    callbackOnFinish(null, p_jsnError);
               }
               else
               {
                    Newtonsoft.Json.Linq.JToken jsnDataOut = OServerData["jsnDataOut"];
                    callbackOnFinish(jsnDataOut, null);
               }
          }

     }
}
