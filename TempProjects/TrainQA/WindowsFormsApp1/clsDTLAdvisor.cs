using DTLExpert.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTLExpert
{
    class clsDTLAdvisor
    {
        FromOrbitToAdvice[] FromOrbitToAdvices = new FromOrbitToAdvice[100]; //Position1-99
        FromPositionToAdvice[,] FromPositionToAdvices = new FromPositionToAdvice[100,2]; //Position1-99,Dir {-1,1}

        public clsDTLAdvisor()
        {
            //Get trained FromOrbitTos
            GetBestPositionFromOrbitTo();
            GetBestFromPositionToAdvice();


        }
        private void GetBestPositionFromOrbitTo()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestPositionFromOrbitTo.csv");
            int pos = -1;
            foreach (string line in lines)
            {
                if (pos == -1)
                {
                    pos++;
                    continue; //Skip header
                }
                char[] aCh = new char[1];
                aCh[0] = ',';
                string[] aValues = line.Split(aCh);
                int position = int.Parse(aValues[0]);
                int dir = int.Parse(aValues[1]);
                int Returnn = int.Parse(aValues[2]);
                int abort = int.Parse(aValues[3]);
                double dGain = double.Parse(aValues[4]);
                double dMaxloss = double.Parse(aValues[5]);

                FromOrbitToPoitionAdvice _BestPositionFromOrbitTo = new FromOrbitToPoitionAdvice();
                _BestPositionFromOrbitTo.dir = dir;
                _BestPositionFromOrbitTo.returnn = Returnn;
                _BestPositionFromOrbitTo.abort = abort;
                _BestPositionFromOrbitTo.expectedGain = dGain;
                _BestPositionFromOrbitTo.maxLoss = dMaxloss;

                FromOrbitToAdvices[position] = _BestPositionFromOrbitTo;

            }

        }
        private void GetBestFromPositionToAdvice()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestFromPositionToAdvice.csv");
            int pos = -1;
            foreach (string line in lines)
            {
                if (pos == -1)
                {
                    pos++;
                    continue; //Skip header
                }
                char[] aCh = new char[1];
                aCh[0] = ',';
                string[] aValues = line.Split(aCh);
                int position = int.Parse(aValues[0]);
                int PositionDir = int.Parse(aValues[1]);
                int dir = int.Parse(aValues[2]);
                int Returnn = int.Parse(aValues[3]);
                int abort = int.Parse(aValues[4]);
                double dGain = double.Parse(aValues[5]);
                double dMaxloss = double.Parse(aValues[6]);

                FromPositionToAdvice _BestFromPositionToAdvice;
                if (dir==0)
                    _BestFromPositionToAdvice = new FromPositionToAbortAdvice();
                else
                    _BestFromPositionToAdvice = new FromPositionToHoldAdvice();

                _BestFromPositionToAdvice.dir = dir;
                if (dir != 0)
                {
                    ((FromPositionToHoldAdvice)_BestFromPositionToAdvice).returnn = Returnn;
                    ((FromPositionToHoldAdvice)_BestFromPositionToAdvice).abort = abort;
                }
                _BestFromPositionToAdvice.expectedGain = dGain;
                _BestFromPositionToAdvice.maxLoss = dMaxloss;

                FromPositionToAdvices[position, StaticFunctions.DirToArrayIndex(PositionDir)] = _BestFromPositionToAdvice;

            }

        }

        public FromOrbitToAdvice AdviceFromOrbitTo(OrbitState inOrbitState)
        {
            FromOrbitToAdvice _FromOrbitToAdvice = null;
            int CurrStarSize = inOrbitState.StarSize;
             FromOrbitToAdvice BestPositionFromOrbitTo = FromOrbitToAdvices[CurrStarSize];

            if (BestPositionFromOrbitTo != null)
            {
                if (BestPositionFromOrbitTo is FromOrbitToPoitionAdvice && 
                    BestPositionFromOrbitTo.expectedGain > 0)
                    _FromOrbitToAdvice = BestPositionFromOrbitTo;

                if (BestPositionFromOrbitTo is FromOrbitToWaitAdvice )
                    _FromOrbitToAdvice = BestPositionFromOrbitTo;

            }


            return _FromOrbitToAdvice;

        }
        public FromPositionToAdvice AdviceFromPositionTo(PositionState inState)
        {
            FromPositionToAdvice _FromPositionToAdvice = null;

            int CurrStarSize = inState.StarSize;
            int CurrDir = inState.dir;
            FromPositionToAdvice BestPositionFromPositionTo = FromPositionToAdvices[CurrStarSize,
                StaticFunctions.DirToArrayIndex(CurrDir)];

            if (BestPositionFromPositionTo != null)
            {
                if (BestPositionFromPositionTo is FromPositionToAbortAdvice)
                    _FromPositionToAdvice =  BestPositionFromPositionTo;

                if (BestPositionFromPositionTo is FromPositionToHoldAdvice  && 
                    BestPositionFromPositionTo.expectedGain > 0)
                    _FromPositionToAdvice = BestPositionFromPositionTo;

            }


            return _FromPositionToAdvice;

        }
    }

}
