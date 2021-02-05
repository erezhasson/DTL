using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStar : MonoBehaviour
{
     public Transform m_StarTrans;
     public int m_Size;
     private float starRadius = 10;

     // Start is called before the first frame update
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {
          drawStar();
     }

     private void drawStar()
     {
          m_StarTrans.localScale = new Vector3(m_Size + starRadius * 2, m_Size + starRadius * 2, m_Size + starRadius * 2);
     }
}
