using DTLExpert.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTLExpert
{

    class clsJobEvaluation
    {
        public double[] inSizes;
        private  clsDTLAdvisor DTLAdvisor = new clsDTLAdvisor() ;

        public void Go()
        {
            State currState = null;
            double TotalGain = 0;
            foreach (int Size in inSizes)
            {
                if (currState == null)
                    currState = new OrbitState(Size, TotalGain);


                if (currState is OrbitState)
                {
                    if (Size == -9)
                        continue;

                    currState.StarSize = Size;
                    FromOrbitToAdvice _FromOrbitToAdvice =
                        DTLAdvisor.AdviceFromOrbitTo((OrbitState)currState);
                    if (_FromOrbitToAdvice is FromOrbitToWaitAdvice)
                    {
                        //Do nothing
                    }

                    if (_FromOrbitToAdvice is FromOrbitToPoitionAdvice)
                    {
                        FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = (FromOrbitToPoitionAdvice)_FromOrbitToAdvice;
                        int newDir = _FromOrbitToPoitionAdvice.dir;
                        int newReturnn = _FromOrbitToPoitionAdvice.returnn;
                        int newAbort = _FromOrbitToPoitionAdvice.abort;

                        PositionState newState = new PositionState(newDir, Size, newReturnn, newAbort, TotalGain);
                        currState = newState;
                    }
                    continue;

                }

                if (currState is PositionState)
                {

                    PositionState _PositionState = (PositionState)currState;
                    double prevStarSize = currState.StarSize;
                    currState.StarSize = Size;


                    if (Size == -9 || Size <= 0 || Size >= 100)
                    {
                        if (Size != -9)
                        {
                            _PositionState.PositionGain += _PositionState.dir * (Size - prevStarSize);
                            TotalGain += _PositionState.PositionGain;
                        }
                        OrbitState newState = new OrbitState(Size, TotalGain);
                        currState = newState;


                        continue;
                    }



                    FromPositionToAdvice _FromPositionToAdvice =
                        DTLAdvisor.AdviceFromPositionTo((PositionState)currState);
                    if (_FromPositionToAdvice is FromPositionToHoldAdvice)
                    {
                        _PositionState.PositionGain += _PositionState.dir * (Size - prevStarSize);
                    }

                    if (_FromPositionToAdvice is FromPositionToAbortAdvice)
                    {
                        FromPositionToAbortAdvice _FromPositionToAbortAdvice = (FromPositionToAbortAdvice)_FromPositionToAdvice;
                        _PositionState.PositionGain += _PositionState.dir * (Size - prevStarSize);
                        TotalGain += _PositionState.PositionGain;
                        OrbitState newState = new OrbitState(Size, TotalGain);
                        currState = newState;
                    }
                    continue;

                }


            }
        }
     }
}
