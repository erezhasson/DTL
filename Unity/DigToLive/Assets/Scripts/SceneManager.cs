using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
     public EnergyStar star;
     // Start is called before the first frame update
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {
          StartCoroutine(ServerGateway.HttpPostWrapper(p_Controller: "DTLGame",
            p_OP: "Get_Status",
            p_jsnInData: "", OnDTLGame_Get_Status_Complete));
     }

     public void OnDTLGame_Get_Status_Complete(Newtonsoft.Json.Linq.JToken p_jsnOutData, string p_jsnError)
     {
          if (p_jsnError == null)
          {
               star.StarSize = (float)p_jsnOutData["EnergyStarSize"];
          }
     }
}
