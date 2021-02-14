using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStar : MonoBehaviour
{
     private ParticleSystem.ShapeModule energyShape;
     public float m_logicalSize, m_physicalSize;
     private bool m_isExpanding;
     private const float radiusFctr = 5;

     public float StarSize
     {
          get
          {
               return m_logicalSize;
          }

          set
          {
               m_logicalSize = value;
          }
     }

     // Start is called before the first frame update
     void Start()
     {
          energyShape = GetComponent<ParticleSystem>().shape;
     }

     // Update is called once per frame
     void Update()
     {
          if (m_physicalSize >= 10 * radiusFctr)
          {
               explode();
          }

          else if (m_physicalSize <= radiusFctr && m_logicalSize <= radiusFctr)
          {
               shrink();
          }

          if (m_physicalSize != m_logicalSize)
          {
               resizeStar(m_logicalSize > m_physicalSize);
          }

          if (m_physicalSize == 20f)
          {
               m_logicalSize = 0;
          }

     }
     /*
     public void btnGet_Status_Clicked()
     {
          StartCoroutine(ServerGateway.HttpPostWrapper(p_Controller: "DTLGame",
              p_OP: "Get_Status",
              p_jsnInData: "", OnDTLGame_Get_Status_Complete));

     }
     public void OnDTLGame_Get_Status_Complete(Newtonsoft.Json.Linq.JToken p_jsnOutData, string p_jsnError)
     {
          if (p_jsnError != null)
          {
          }

          else
          {
               m_logicalSize = float.Parse((string)p_jsnOutData["EnergyStarSize"]);
          }

     }
     */
     private void resizeStar(bool isExpanding)
     {
          float resizeFctr;

          if (isExpanding)
          {
               resizeFctr = m_physicalSize <= m_logicalSize - 0.01f ? 0.01f : m_logicalSize - m_physicalSize;
          }

          else
          {
               resizeFctr = m_physicalSize >= m_logicalSize + 0.01f ? -0.01f : m_physicalSize - m_logicalSize;
          }

          energyShape.radius += resizeFctr;
          m_physicalSize = energyShape.radius;
          Debug.Log(m_physicalSize);
     }

     private void explode()
     {
          
     }

     private void shrink()
     {
          while (m_physicalSize >= 0)
          {
               m_physicalSize = m_physicalSize >= 0.05f ? m_physicalSize - 0.05f : 0;
               energyShape.radius = m_physicalSize;
          }

          m_physicalSize = 0;
     }
}
