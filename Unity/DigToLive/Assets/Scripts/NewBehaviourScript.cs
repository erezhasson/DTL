using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
     public GameObject Arch;

     // Start is called before the first frame update
     void Start()
     {
          Arch.GetComponent<Renderer>().material.color =new Color(1.0f, 1.0f, 1.0f, 1.0f);
     }

     // Update is called once per frame
     void Update()
     {

     }
}