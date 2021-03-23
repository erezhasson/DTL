using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert
{
    public static class StaticFunctions
    {
        public static int PositionToArrayIndex(int p_iPosition)
        {
            return p_iPosition - 1;
        }
        public static int DirToArrayIndex(int p_dir)
        {
            return (p_dir + 1) / 2;
        }
        public static int ReturnnToArrayIndex(int p_Returnn)
        {
            return p_Returnn;
        }
        public static int AbortToArrayIndex(int p_abort)
        {
            return p_abort;
        }

        public static  int dTOi(double p_dValue)
        {
            int iValue = (int)Math.Floor(p_dValue);
            if (iValue < 0)
                iValue = 0;
            if (iValue > 100)
                iValue = 100;
            return iValue;


        }

        public static T Clone<T>( T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
