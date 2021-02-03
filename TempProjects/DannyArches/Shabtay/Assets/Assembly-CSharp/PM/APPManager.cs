using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APPManager : MonoBehaviour
{
     // Start is called before the first frame update
    public Canvas MainCanvas;
    private GameObject m_GOPrice;
    private FortniteAPI HttpAPI = new FortniteAPI();
    void Start()
    {
        //Draw Ruler
        for (int i = 1; i <= 10; i++)
        {
            GameObject GOCircle = Circle.CreateGameObject(p_Name: "Ruler" + i.ToString());
            GOCircle.GetComponent<SpriteRenderer>().sprite = Circle.CreateSprite(p_Radius: 0.1f*i, p_stroke: "#000");
            GOCircle.transform.SetParent(MainCanvas.transform, false);
        }

        // Create GameObject for price
        m_GOPrice = Circle.CreateGameObject(p_Name: "Price");
        m_GOPrice.transform.SetParent(MainCanvas.transform, false);



    }

    // Update is called once per frame
    void Update()
    {
        //HttpAPI.GenerateRequest();
        //float price = HttpAPI.m_size;
        //if (m_GOPrice != null)
        //    m_GOPrice.GetComponent<SpriteRenderer>().sprite = Circle.CreateSprite(p_Radius: price, p_stroke: "#0f0");

    }
}
