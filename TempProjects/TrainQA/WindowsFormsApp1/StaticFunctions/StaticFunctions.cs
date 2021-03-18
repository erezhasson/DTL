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
    }
}
