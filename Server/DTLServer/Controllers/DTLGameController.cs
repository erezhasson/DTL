using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace DTL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DTLGameController : Controller
    {
     

        private readonly ILogger<DTLGameController> _logger;

        public DTLGameController(ILogger<DTLGameController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
 
            return "DTLGame controller is up and ready...";

        }
        [HttpPost]
        public string PostOP( [FromForm] string op, [FromForm] string jsnInData)
        {
            int HttpStatusCodeOK = (int)HttpStatusCode.OK;
            int HttpStatusCodeInternalServerError = (int)HttpStatusCode.InternalServerError;
            try
            {
                Newtonsoft.Json.Linq.JContainer OInData = null;
                if (jsnInData != null)
                    OInData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsnInData);
                mdDataFromServerResponse mdServerData = new mdDataFromServerResponse();

                switch (op)
                {
                    case "Get_Status":
                        Random random = new Random();
                        int value = random.Next(0, 10);
                        double newIndexValue = clsApp.g_index;
                        string EnergyStarAnimation = "Resize";
                        newIndexValue += Convert.ToDouble(value) / 10 - 0.5;
                        if (newIndexValue > 10)
                        {
                            newIndexValue -= 5;
                            EnergyStarAnimation = "ExplodeAndResize";
                        }
                        if (newIndexValue < 0)
                        {
                            newIndexValue = +5;
                            EnergyStarAnimation = "ShrinkAndResize";
                        }
                        clsApp.g_index = newIndexValue;


                        mdGetStatusDataFromServerResponse mdStatusData = new mdGetStatusDataFromServerResponse();
                        mdStatusData.EnergyStarSize = newIndexValue.ToString();
                        mdStatusData.EnergyStarAnimation = EnergyStarAnimation;
                        string jsnStatusData= Newtonsoft.Json.JsonConvert.SerializeObject(mdStatusData);

                        mdServerData.StatusCode = HttpStatusCodeOK.ToString();
                        mdServerData.jsnDataOut = jsnStatusData;
                        return Newtonsoft.Json.JsonConvert.SerializeObject(mdServerData);

                    case "Set_ResetEnergyStarSize":
                        clsApp.g_index = Double.Parse((string)OInData["EnergyStarSize"]);

                        mdServerData.StatusCode = HttpStatusCodeOK.ToString();
                        mdServerData.jsnDataOut = "";
                        return Newtonsoft.Json.JsonConvert.SerializeObject(mdServerData);

   
                }
                throw new Exception($"Op { op } not supported");
  
 

               
                 }
            catch (Exception e)
            {
                mdErrorFromServerResponse mdErrorFromServer = new mdErrorFromServerResponse ();
                mdErrorFromServer.Function = "DTLGameController.PostOP";
                mdErrorFromServer.Op = op;
                mdErrorFromServer.jsnInData = jsnInData;
                mdErrorFromServer.ErrorMessage = e.Message;

                string jsnErrorData = Newtonsoft.Json.JsonConvert.SerializeObject(mdErrorFromServer);

                mdDataFromServerResponse mdServerDataError = new mdDataFromServerResponse();
                mdServerDataError.StatusCode = HttpStatusCodeInternalServerError.ToString();
                mdServerDataError.jsnDataOut = jsnErrorData;
                return Newtonsoft.Json.JsonConvert.SerializeObject(mdServerDataError);



   
            }


        }

    }
}
public class mdDataFromServerResponse
{
    public string StatusCode;
    public string jsnDataOut;
}

public class mdGetStatusDataFromServerResponse
{
    public string EnergyStarSize;
    public string EnergyStarAnimation;
}

public class mdErrorFromServerResponse
{
    public string Function;
    public string Op;
    public string jsnInData;
    public string ErrorMessage;
}
