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


                FromOrbitToAdvice _BestFromOrbitToAdvice;
                if (dir == 0)
                    _BestFromOrbitToAdvice = new FromOrbitToWaitAdvice();
                else
                    _BestFromOrbitToAdvice = new FromOrbitToPoitionAdvice();

                _BestFromOrbitToAdvice.dir = dir;
                if (dir != 0)
                {
                    ((FromOrbitToPoitionAdvice)_BestFromOrbitToAdvice).returnn = Returnn;
                    ((FromOrbitToPoitionAdvice)_BestFromOrbitToAdvice).abort = abort;
                }
                _BestFromOrbitToAdvice.expectedGain = dGain;
                _BestFromOrbitToAdvice.maxLoss = dMaxloss;



   
                FromOrbitToAdvices[position] = _BestFromOrbitToAdvice;

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
            double CurrStarSize = inOrbitState.StarSize;
            FromOrbitToAdvice _FromOrbitToAdvice = null;
            if (CurrStarSize>=100 || CurrStarSize<=0)
            {
                _FromOrbitToAdvice = new FromOrbitToWaitAdvice();
                ((FromOrbitToWaitAdvice)_FromOrbitToAdvice).expectedGain = 0;
                ((FromOrbitToWaitAdvice)_FromOrbitToAdvice).maxLoss = 0;
             }
            else
             _FromOrbitToAdvice = FromOrbitToAdvices[ StaticFunctions.dTOi( CurrStarSize)];

            if (_FromOrbitToAdvice != null)
            {
                if (_FromOrbitToAdvice is FromOrbitToPoitionAdvice &&
                    _FromOrbitToAdvice.expectedGain <= 0)
                    _FromOrbitToAdvice = null; //Do not take advise


                if (_FromOrbitToAdvice is FromOrbitToWaitAdvice )
                    {
                    //Do nothing. just return _FromOrbitToAdvice
                }
            }


            return _FromOrbitToAdvice;

        }
        public FromPositionToAdvice AdviceFromPositionTo(PositionState inState)
        {
            FromPositionToAdvice _FromPositionToAdvice = null;

            double CurrStarSize = inState.StarSize;
            int CurrDir = inState.dir;

            if (CurrStarSize >= 100 || CurrStarSize <= 0)
            {
                _FromPositionToAdvice = new FromPositionToAbortAdvice();
                ((FromPositionToAbortAdvice)_FromPositionToAdvice).expectedGain = 0;
                ((FromPositionToAbortAdvice)_FromPositionToAdvice).maxLoss = 0;
            }
            else
                _FromPositionToAdvice = FromPositionToAdvices[StaticFunctions.dTOi(CurrStarSize),
                            StaticFunctions.DirToArrayIndex(CurrDir)];


  
            if (_FromPositionToAdvice != null)
            {
                if (_FromPositionToAdvice is FromPositionToAbortAdvice)
                {
                    //Do nothing
                    //just return _FromPositionToAdvice
                }
  
                if (_FromPositionToAdvice is FromPositionToHoldAdvice  && 
                    _FromPositionToAdvice.expectedGain <= 0)
                    _FromPositionToAdvice = null; //Do not take advise

            }


            return _FromPositionToAdvice;

        }
    }

}
