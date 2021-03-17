using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert.Models
{
    class State
    {
        public int dir;
        public int position;
        public State (int inDir,int inposition)
        {
            dir = inDir;
            position = inposition;
        }
    }
}
