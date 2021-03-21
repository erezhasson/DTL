

using System;
using System.Collections.Generic;
using System.Text;



namespace DTLExpert.Models
{
    class PositionState : State
    {
        public double Position;
        public int returnn;
        public int abort;
        public double PositionGain;


        public PositionState(int inDir, int inStarSize, int inReturnn, int inabort, double inTotalGain)
        {
            dir = inDir;
            StarSize = inStarSize;

            Position = inStarSize;
            returnn = inReturnn;
            abort = inabort;
            TotalGain = inTotalGain;

            PositionGain = 0;
        }
    }
}
