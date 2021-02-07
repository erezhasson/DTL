using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager_script : MonoBehaviour
{
    GameObject GOinpErrorMessage;
    InputField inpEnergyStarSize, inpEnergyStarAnimation, inpErrorMessage, inpSet_ResetEnergyStarSize;

    // Start is called before the first frame update
    void Start()
    {

  
        inpEnergyStarSize = (InputField)GameObject.Find("inpEnergyStarSize").GetComponent<InputField>();
        inpEnergyStarAnimation = (InputField)GameObject.Find("inpEnergyStarAnimation").GetComponent<InputField>();
        inpErrorMessage = (InputField)GameObject.Find("inpErrorMessage").GetComponent<InputField>();
        inpSet_ResetEnergyStarSize = (InputField)GameObject.Find("inpSet_ResetEnergyStarSize").GetComponent<InputField>();

        GOinpErrorMessage = inpErrorMessage.gameObject; //Needed for hide the input field
        GOinpErrorMessage.SetActive(false); // false to hide, true to show;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void btnGet_Status_Clicked()
    {
        GOinpErrorMessage.SetActive(false); // false to hide, true to show;
        inpEnergyStarSize.text = "";
        inpEnergyStarAnimation.text = "";
        StartCoroutine(ServerGateway.HttpPostWrapper(p_Controller: "DTLGame",
            p_OP: "Get_Status",
            p_jsnInData: "", OnDTLGame_Get_Status_Complete));

    }
    public void OnDTLGame_Get_Status_Complete(Newtonsoft.Json.Linq.JToken p_jsnOutData, string p_jsnError)
    {
        if (p_jsnError != null)
        {
            inpErrorMessage.text = p_jsnError;
            GOinpErrorMessage.SetActive(true); // false to hide, true to show;
        }

        else
        {
            inpEnergyStarSize.text = (string)p_jsnOutData["EnergyStarSize"];
            inpEnergyStarAnimation.text = (string)p_jsnOutData["EnergyStarAnimation"];

        }

    }
    public void btnResetEnergyStarSize_Clicked()
    {
        GOinpErrorMessage.SetActive(false); // false to hide, true to show;
        StartCoroutine(ServerGateway.HttpPostWrapper(p_Controller: "DTLGame",
          p_OP: "Set_ResetEnergyStarSize",
          p_jsnInData: "{\"EnergyStarSize\":\"" + inpSet_ResetEnergyStarSize.text + "\"}",
         OnResetEnergyStarSize_Complete));

    }
    public void OnResetEnergyStarSize_Complete(Newtonsoft.Json.Linq.JToken p_jsnOutData, string p_jsnError)
    {
        if (p_jsnError != null)
        {
            inpErrorMessage.text = p_jsnError;
            GOinpErrorMessage.SetActive(true); // false to hide, true to show;
        }

        else
        {
            inpSet_ResetEnergyStarSize.text = "Ok";
        }

    }

}
