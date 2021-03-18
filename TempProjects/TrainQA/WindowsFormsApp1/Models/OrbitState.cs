using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert.Models
{
    class PositionState
    {
        public int dir;
        public int position;
        public int returnn;
        public int abort;

        public PositionState (int inDir,int inposition)
        {
            dir = inDir;
            position = inposition;
        }
    }
}
