using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert.Models
{
    class ActionWithNoDir:Action
    {
        public ActionWithNoDir()
        {
            dir = 0;
            expectedGain = -9;
            maxLoss = -9;
        }
    }
}
