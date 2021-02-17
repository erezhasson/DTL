using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
     public EnergyStar m_EnergyStar;
     private float updatingTime = 2f;

     // Start is called before the first frame update
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {
          updatingTime -= Time.deltaTime;

          if (updatingTime <= 0)
          {
               StartCoroutine(ServerGateway.HttpPostWrapper(p_Controller: "DTLGame",
            p_OP: "Get_Status",
            p_jsnInData: "", OnDTLGame_Get_Status_Complete));
               updatingTime = 2f;
          }
          
     }

     public void OnDTLGame_Get_Status_Complete(string p_jsnOutData, string p_jsnError)
     {
          if (p_jsnError == null)
          {
               mdStatusDataFromServer mdStatusData = JsonUtility.FromJson<mdStatusDataFromServer>(p_jsnOutData);

               m_EnergyStar.StarSize = float.Parse(mdStatusData.EnergyStarSize);
          }    
     }
}

public class mdStatusDataFromServer
{
     public string EnergyStarSize;
     public string EnergyStarAnimation;
}