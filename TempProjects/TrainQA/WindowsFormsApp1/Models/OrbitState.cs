using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert.Models
{
    class OrbitState : State
    {
        public OrbitState(int inStarSize, double inTotalGain)

        {
            dir = 0;
            StarSize = inStarSize;
            TotalGain = inTotalGain;

        }
    }
}
