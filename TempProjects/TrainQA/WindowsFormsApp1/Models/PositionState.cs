using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert.Models
{
    class OrbitState
    {
        public int dir;
        public int position;
        public OrbitState(int inposition)
        {
            dir = 0;
            position = inposition;
        }
    }
}
