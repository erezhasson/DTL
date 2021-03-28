using DTLExpert.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTLExpert
{

    class clsJobEvaluation
    {
        public double[] inSizes;
        private  clsDTLAdvisor DTLAdvisor = new clsDTLAdvisor() ;
        List<State> States = new List<State>();

        public void Go()
        {
            evaluateAll();
            PrintEvaluationsFull();
            PrintEvaluationsShort();
        }

        private void evaluateAll()
        {
            State currState = null;
            foreach (double Size in inSizes)
            {
                if (currState == null)
                {
                    currState = new OrbitState(Size, 0);
                    States.Add(currState);
                }

                currState= evaluate(currState, Size);
                if (currState is OrbitState)
                {
                    OrbitState _OrbitState = (OrbitState)currState;
                    States.Add(StaticFunctions.Clone(_OrbitState));
                }

                if (currState is PositionState)
                {
                    PositionState _PositionState = (PositionState)currState;
                    States.Add(StaticFunctions.Clone(_PositionState));
                }
           }

        }

        public State evaluate (State currState, double StarSize)
        {
            double prevStarSize = currState.StarSize;
            currState.StarSize = StarSize;
            if (currState is OrbitState)
            {
                if (StarSize == -9)
                    return currState;

                FromOrbitToAdvice _FromOrbitToAdvice =
                    DTLAdvisor.AdviceFromOrbitTo((OrbitState)currState);
                if (_FromOrbitToAdvice is FromOrbitToWaitAdvice)
                {
                    //Do nothing
                    return currState;
                }

                if (_FromOrbitToAdvice is FromOrbitToPoitionAdvice)
                {
                    FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = (FromOrbitToPoitionAdvice)_FromOrbitToAdvice;
                    int newDir = _FromOrbitToPoitionAdvice.dir;
                    int newReturnn = _FromOrbitToPoitionAdvice.returnn;
                    int newAbort = _FromOrbitToPoitionAdvice.abort;

                    PositionState newState = new PositionState(newDir, StarSize, 
                        newReturnn, newAbort, currState.TotalGain-2.5);
                    currState = newState;
                }
                return currState;

            }

            if (currState is PositionState)
            {

                PositionState _PositionState = (PositionState)currState;
                currState.StarSize = StarSize;
                int dir = _PositionState.dir;
                int abort = _PositionState.abort;
                int returnn = _PositionState.returnn;



                if (StarSize == -9 || StarSize <= 0 || StarSize >= 100
                    || dir * (StarSize - returnn) >= 0 || dir * (StarSize - abort) <= 0)
                {
                    if (StarSize != -9)
                    {
                        _PositionState.PositionGain += _PositionState.dir * (StarSize - prevStarSize);
                        _PositionState.TotalGain += _PositionState.PositionGain-2.5;
                    }
                    OrbitState newState = new OrbitState(StarSize, _PositionState.TotalGain);
                    currState = newState;


                    return currState;
                }



                FromPositionToAdvice _FromPositionToAdvice =
                    DTLAdvisor.AdviceFromPositionTo((PositionState)currState);

                if (_FromPositionToAdvice ==null ||_FromPositionToAdvice is FromPositionToHoldAdvice )
                {
                    _PositionState.PositionGain += _PositionState.dir * (StarSize - prevStarSize);
                }

                if (_FromPositionToAdvice != null && _FromPositionToAdvice is FromPositionToAbortAdvice)
                {
                    FromPositionToAbortAdvice _FromPositionToAbortAdvice = (FromPositionToAbortAdvice)_FromPositionToAdvice;
                    _PositionState.PositionGain += _PositionState.dir * (StarSize - prevStarSize);
                    _PositionState.TotalGain += _PositionState.PositionGain;
                    OrbitState newState = new OrbitState(StarSize, _PositionState.TotalGain);
                    currState = newState;
                }

            }

            return currState;



        }

        private void PrintEvaluationsFull()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\EvaluateFull.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "StartSize", "PositionDir", "Dir", "Returnn",
                    "Abort",  "TotalGain"));

                foreach (State _State in States)
                {
                    if (_State is OrbitState)
                    {
                        OrbitState _OrbitState = (OrbitState)_State;
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},",
                            _OrbitState.StarSize, -9, _OrbitState.dir,
                            -9, -9,
                            _OrbitState.TotalGain));

                    }
                    if (_State is PositionState)
                    {
                        PositionState _PositionState = (PositionState)_State;
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},",
                            _PositionState.StarSize, _PositionState.Position, _PositionState.dir,
                            _PositionState.returnn, _PositionState.abort,
                            _PositionState.TotalGain));

                    }
                }
            }
        }
        private void PrintEvaluationsShort()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\EvaluateShort.csv";
            int prevDir = -9;
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "StartSize", "PositionDir", "Dir", "Returnn",
                    "Abort", "TotalGain"));

                foreach (State _State in States)
                {
                    if (_State.dir == prevDir)
                        continue;
                    prevDir = _State.dir;
                    if (_State is OrbitState)
                    {
                        OrbitState _OrbitState = (OrbitState)_State;
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},",
                            _OrbitState.StarSize, -9, _OrbitState.dir,
                            -9, -9,
                            _OrbitState.TotalGain));

                    }
                    if (_State is PositionState)
                    {
                        PositionState _PositionState = (PositionState)_State;
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},",
                            _PositionState.StarSize, _PositionState.Position, _PositionState.dir,
                            _PositionState.returnn, _PositionState.abort,
                            _PositionState.TotalGain));

                    }
                }
            }
        }

    }
}
