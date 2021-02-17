using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStar : MonoBehaviour
{
     private ParticleSystem m_StarPS;
     private ParticleSystem.ShapeModule energyShape;
     public float m_lastSize, m_Size = radiusFctr;
     private const float radiusFctr = 5, MaxSize = 10 * radiusFctr;

     public float StarSize
     {
          get
          {
               return m_Size;
          }

          set
          {
               m_Size = (value + 1) * radiusFctr;
          }
     }

     public float LogicalStarSize
     {
          get
          {
               return m_Size;
          }
     }

     // Start is called before the first frame update
     void Start()
     {
          m_StarPS = GetComponent<ParticleSystem>();
          energyShape = m_StarPS.shape;
          m_lastSize = m_Size;
     }

     // Update is called once per frame
     void Update()
     {
          if (OutOfRange())
          {
               if (m_Size > radiusFctr)
               {
                    explode();
               }

               else
               {
                    shrink();
               }

               m_Size = m_lastSize;
          }

          energyShape.radius = m_Size;
          m_lastSize = m_Size;
          Debug.Log(m_Size);
     }

     private float getStarSize()
     {
          return Random.Range(0f, 100f);
     }

     private bool OutOfRange()
     {
          return m_Size > MaxSize || m_Size < radiusFctr;
     }

     private void explode()
     {
          float explodingTime = 2f;

          while (m_StarPS.startSpeed < 20)
          {
               m_StarPS.startSpeed += 0.5f;
          }

          while (m_StarPS.startSpeed > 0)
          {
               m_StarPS.startSpeed -= 0.5f;
          }
     }

     private void shrink()
     {
          while (m_Size >= 0)
          {
               m_Size = m_Size >= 0.001f ? m_Size - 0.001f : 0;
          }

          GetComponent<Renderer>().enabled = false;
     }
}
