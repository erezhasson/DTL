using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class FortniteAPI : MonoBehaviour
{
    private const string URL = "https://localhost:5001/weatherforecast";
    public float m_size = 0 ;
    
    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }

    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            //request.SetRequestHeader("X-RapidAPI-Host", HOST);
            //request.SetRequestHeader("X-RapidAPI-Key", API_KEY);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonString=request.downloadHandler.text;
                HttpDataModel HttpDataModelSent = JsonUtility.FromJson<HttpDataModel>(jsonString);
                m_size = HttpDataModelSent.size;

            }
        }
    }
}
[System.Serializable]
public class HttpDataModel
{
  
 
    public float  size;

    

  

}