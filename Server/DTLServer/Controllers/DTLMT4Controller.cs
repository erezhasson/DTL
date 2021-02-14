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
    public class DTLMT4Controller : Controller
    {
     

        private readonly ILogger<DTLMT4Controller> _logger;

        public DTLMT4Controller(ILogger<DTLMT4Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
 
            return "DTLMT4 controller is up and ready...";

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
     
                    case "Set_Price":
                        clsApp.g_price = Double.Parse((string)OInData["Price"]);
                        return "{\"StatusCode\":\"" + HttpStatusCodeOK.ToString() + "\",\"jsnDataOut\":{ }}";

                }
                string ErrorDesc = $"DTLMT4Controller.PostOP op = { op }, jsnInData={jsnInData},error.Message =op { op } not supported";
                return "{\"StatusCode\":\"" + HttpStatusCodeInternalServerError.ToString() + "\",\"ErrorDescription\":" + ErrorDesc + "}";

                 }
            catch (Exception e)
            {
                string ErrorDesc = $"DTLMT4Controller.PostOP op = { op }, jsnInData={jsnInData},error.Message = { e.Message }";
                ErrorDesc = ErrorDesc.Replace("\"", "'");
                return "{\"StatusCode\":\"" + HttpStatusCodeInternalServerError.ToString() + "\",\"ErrorDescription\":\"" + ErrorDesc + "\"}";

            }


        }

    }
}
