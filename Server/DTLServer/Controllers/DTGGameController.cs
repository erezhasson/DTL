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
                       return "{\"StatusCode\":\""+ HttpStatusCodeOK.ToString()+ "\",\"jsnDataOut\":{ \"EnergyStarSize\":" + newIndexValue.ToString() + ",\"EnergyStarAnimation\":\""+EnergyStarAnimation+"\"}}";
   
                    case "Set_ResetEnergyStarSize":
                        clsApp.g_index = Double.Parse((string)OInData["EnergyStarSize"]);
                        return "{\"StatusCode\":\"" + HttpStatusCodeOK.ToString() + "\",\"jsnDataOut\":{ }}";

                }
                string ErrorDesc = $"DTLGameController.PostOP op = { op }, jsnInData={jsnInData},error.Message =op { op } not supported";
                return "{\"StatusCode\":\"" + HttpStatusCodeInternalServerError.ToString() + "\",\"ErrorDescription\":" + ErrorDesc + "}";

                 }
            catch (Exception e)
            {
                string ErrorDesc = $"DTLGameController.PostOP op = { op }, jsnInData={jsnInData},error.Message = { e.Message }";
                ErrorDesc = ErrorDesc.Replace("\"", "'");
                return "{\"StatusCode\":\"" + HttpStatusCodeInternalServerError.ToString() + "\",\"ErrorDescription\":\"" + ErrorDesc + "\"}";

            }


        }

    }
}
