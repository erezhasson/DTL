using DTLExpert.Events;
using DTLExpert.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace DTLExpert
{
 
    public class clsJobTrain
    {
  
        public event JobTrainProgressTickDelegate JobTrainProgressTick;
        public double[] inSizes;
        double[,,,] Gain = new Double[99, 2, 101, 101]; //Position1-99, dir {-1,1}, Return 0-100, Abort 0-100
        int[,] SizeMovesCount = new int[100, 101]; //Position1-99, Position1-100
        int[,,] tmpLastCalcIndexInArray = new int[101, 101, 101]; //Position0-100, Return 0-100, Abort 0-100
        double[,] SizeMovesStatistics = new Double[100, 101]; //Position1-99, Position1-100
        FromOrbitToPoitionAdvice[] FromOrbitToPoitionAdvice = new FromOrbitToPoitionAdvice[100]; //Position1-99
        FromOrbitToWaitAdvice[] FromOrbitToWaitAdvice = new FromOrbitToWaitAdvice[100]; //Position1-99
        public FromOrbitToAdvice[] BestFromOrbitAdvice = new FromOrbitToAdvice[100]; //Position1-99
        public FromPositionToAdvice[,] BestFromPositionToAdvice = new FromPositionToAdvice[100,2]; //Position1-99, dir {-1,1}

        public void  Go(bool bRecalcGains)
        {
            /* Gain */
            InitializeGain();//Initilize Gain to 0
            InitializetmpLastcalcPositions();// Initilize Calc Position to -1
            if (bRecalcGains)
            {
                CalcAllGains();
                PrintGains();
                PrintCalcPositions();
            }
            else
                GetGainsData();

            /* MovesStatistics */
            InitializeSizeMovesCount();
            CalcSizeMovesCount();
            InitializeSizeMovesStatistics();
            CalcSizeMovesStatistics();
            PrintSizeMovesStatistics();

            /* FromOrbitToPoitionAdvice  FromOrbitToPoitionAdvice */
            InitializeFromOrbitToPoitionAdvice();
            SetFromOrbitToPoitionAdvice();
            PrintFromOrbitToPoitionAdvice();
            InitializeFromOrbitToWaitAdvice();
            SetFromOrbitToWaitAdvice();
            PrintFromOrbitToWaitAdvice();

            /* BestFromOrbitAdvice   BestFromPositionToAdvice*/

            InitializeBestFromOrbitAdvice();
            SetBestFromOrbitAdvice();
            PrintBestPositionFromOrbitTo();
            InitializeBestFromPositionToAdvice();
            SetBestFromPositionToAdvice();
            PrintBestFromPositionToAdvice();
        }

        private void GetGainsData()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\Gains.csv");
             int pos = -1;
            foreach (string line in lines)
            {
                if (pos == -1)
                {
                    pos++;
                    continue; //Skip header
                }
                char[] aCh = new char[1];
                aCh[0] =',';
                string[] aValues = line.Split(aCh);
                int position = int.Parse(aValues[0]);
                int dir = int.Parse(aValues[1]);
                int Returnn = int.Parse(aValues[2]);
                int abort = int.Parse(aValues[3]);
                double dGain = double.Parse(aValues[4]);

                SetGain(p_iPosition: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, dGain);
 
                pos++;
            }

        }

        private void InitializeGain()
        {
            for (int position = 1; position < 100; position++)
            {
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 100; Returnn++)
                    {
                        for (int abort = 0; abort <= 100; abort++)

                        {
                            SetGain(p_iPosition: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: 0);
                        }
                    }
                }
            }

        }
        private void InitializetmpLastcalcPositions()
        {
            for (int i = 0; i < 101; i++)
            {
                for (int j = 0; j < 101; j++)
                {
                    for (int k = 0; k < 101; k++)
                    {
                        tmpLastCalcIndexInArray[i, j, k] = -1;
                    }
                }
            }
        }
        private void CalcAllGains()
        {
            int ipbSize = 0;
            JobTrainProgressTick(0, inSizes.Length);
   
            for (int iSize = 0; iSize <= inSizes.Length - 1; iSize++)
            {
                int inewpbSize = 100 * iSize / (inSizes.Length - 1);
                if (inewpbSize > ipbSize + 1)
                {
                    ipbSize = inewpbSize;
 
                    JobTrainProgressTick(iSize + 1, inSizes.Length);
                }


  
                double dposition = inSizes[iSize];
                int iposition = dTOi(dposition);
                 for(int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 100; Returnn++)
                    {
                         for (int abort = 0; abort <= 100; abort++)
                        {
                            if ((dir == 1 && Returnn > iposition && abort < iposition)
                                || (dir == -1 && Returnn < iposition && abort > iposition))
                            {
                                if (iSize > tmpLastCalcIndexInArray[iposition, Returnn, abort])
                                {
                                    double GainToSet = CalcGain(p_dPosition: dposition, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_StartiSize: iSize + 1);
                                     AddToGain(p_dPosition: dposition, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: GainToSet);
                                }
                            }
                        }
                    }
                }
            }
        }
        private double CalcGain(double p_dPosition, int p_dir, int p_Returnn, int p_abort, int p_StartiSize)
        {
            int iposition = dTOi(p_dPosition);
            double dGain = 0;

            if (p_StartiSize > tmpLastCalcIndexInArray[iposition, p_Returnn, p_abort])
            {
                for (int iSize = p_StartiSize; iSize < inSizes.Length; iSize++)
                {
                    double dsize = inSizes[iSize];
                    bool bEndPoition = false;

                    if (dsize >= 100 || dsize <= 0) //Forced abort 
                        bEndPoition = true;

                    if (p_dir == 1 && p_Returnn <= dsize)
                        bEndPoition = true;

                    if (p_dir == 1 && dsize <= p_abort)
                        bEndPoition = true;

                    if (p_dir == -1 && dsize <= p_Returnn)
                        bEndPoition = true;

                    if (p_dir == -1 && dsize >= p_abort)
                        bEndPoition = true;

                    if (bEndPoition)
                    {
                        if (dsize != -9)
                            dGain = p_dir * (dsize - p_dPosition) - 5;

                        tmpLastCalcIndexInArray[iposition, p_Returnn, p_abort] = iSize + 1;
                        break;
                    }

                }
            }

            return dGain;
        }
        private void PrintGains()
        {

            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\Gains.csv";

            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", "Position", "Dir", "Return",
                                           "Abort", "Gain"));

                for (int position = 1; position < 100; position++)
                {
                    for (int dir = -1; dir < 2; dir += 2)
                    {
                        for (int Returnn = 0; Returnn <= 100; Returnn++)
                        {
                            for (int abort = 0; abort <= 100; abort++)

                            {
                                double dGain = GetGain(p_iPosition: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort);
                                if (dGain > 0)
                                    stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", position, dir, Returnn,
                                        abort, dGain));
                            }
                        }
                    }
                }
            }

  
        }

        private void PrintCalcPositions()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\CalcPositions.csv";
            using (var stream = File.CreateText(file))
            {

                stream.WriteLine(string.Format("{0},{1},{2},{3}", "iposition", "Returnn",
                  "abort", "tmpLastCalcIndexInArray[iposition, Returnn, abort]"));
                for (int iposition = 0; iposition < 101; iposition++)
                {
                    for (int Returnn = 0; Returnn < 101; Returnn++)
                    {
                        for (int abort = 0; abort < 101; abort++)
                        {
                            stream.WriteLine(string.Format("{0},{1},{2},{3}", iposition, Returnn,
                                 abort, tmpLastCalcIndexInArray[iposition, Returnn, abort]));
                        }
                        Debug.Print("\n");
                    }
                }
            }
        }
 
        private void InitializeSizeMovesCount()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                for (int SizeTo = 1; SizeTo < 100; SizeTo++)
                {
                    SizeMovesCount[SizeFrom, SizeTo] = 0;
                }
            }

        }
        private void CalcSizeMovesCount()
        {
            for (int indexSizeFrom = 0; indexSizeFrom < inSizes.Length - 1; indexSizeFrom++)
            {
                double dSizeFrom = inSizes[indexSizeFrom];
                double dSizeTo = inSizes[indexSizeFrom + 1];
                if (dSizeFrom != -9 && dSizeTo != -9 && dSizeFrom < 100 && dSizeFrom > 0)
                {
                    int iSizeFrom = dTOi(dSizeFrom);
                    int iSizeTo = dTOi(dSizeTo);
                    SizeMovesCount[iSizeFrom, iSizeTo] += 1;

                }
            }

        }


        private void InitializeSizeMovesStatistics()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                for (int SizeTo = 1; SizeTo < 100; SizeTo++)
                    SizeMovesStatistics[SizeFrom, SizeTo] = 0;
            }
        }
        private void CalcSizeMovesStatistics()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                //calc total
                int SumMoves = 0;
                for (int SizeTo = 1; SizeTo < 100; SizeTo++)
                {
                    SumMoves += SizeMovesCount[SizeFrom, SizeTo];
                }

                //calc statistics
                for (int SizeTo = 1; SizeTo < 100; SizeTo++)
                {

                    if (SumMoves > 0)
                        SizeMovesStatistics[SizeFrom, SizeTo] = (double)SizeMovesCount[SizeFrom, SizeTo] / SumMoves;
                    else
                        SizeMovesStatistics[SizeFrom, SizeTo] = 0;
                }
            }

        }
        private void PrintSizeMovesStatistics()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeMovesStatistics.csv";

            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2}", "SizeFrom", "SizeTo",
                    "SizeMovesStatistics"));
                for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                {
                    for (int SizeTo = 1; SizeTo < 101; SizeTo++)
                    {
                        Debug.Print(SizeFrom + "," + SizeTo + "," + SizeMovesStatistics[SizeFrom, SizeTo]);
                        stream.WriteLine(string.Format("{0},{1},{2}", SizeFrom, SizeTo,
                               SizeMovesStatistics[SizeFrom, SizeTo]));
                    }
                }
            }

        }
        private void InitializeFromOrbitToPoitionAdvice()
        {
            for (int position = 1; position < 100; position++)
                FromOrbitToPoitionAdvice[position] = null;
        }
        private void SetFromOrbitToPoitionAdvice()
        {
            for (int position = 1; position < 100; position++)
            {
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 100; Returnn++)
                    {
                        for (int abort = 0; abort <= 100; abort++)
                        {
                            double dGain = GetGain(p_iPosition: position, p_dir: dir, 
                                p_Returnn: Returnn, p_abort: abort);
  
                            double dLoss = dir * (abort - position) - 5;
                            FromOrbitToPoitionAdvice CurrentFromOrbitTo = FromOrbitToPoitionAdvice[position];
                            if (CurrentFromOrbitTo == null)
                            {
                                FromOrbitToPoitionAdvice newFromOrbitTo = new FromOrbitToPoitionAdvice();
                                FromOrbitToPoitionAdvice[position] = newFromOrbitTo;
                                newFromOrbitTo.dir = dir;
                                newFromOrbitTo.returnn = Returnn;
                                newFromOrbitTo.abort = abort;
                                newFromOrbitTo.expectedGain = dGain;
                                newFromOrbitTo.maxLoss = dLoss;
                            }
                            else
                            {
                                if (dGain > CurrentFromOrbitTo.expectedGain ||
                                    (dGain == CurrentFromOrbitTo.expectedGain && dLoss < CurrentFromOrbitTo.maxLoss))
                                {
                                    CurrentFromOrbitTo.dir = dir;
                                    CurrentFromOrbitTo.returnn = Returnn;
                                    CurrentFromOrbitTo.abort = abort;
                                    CurrentFromOrbitTo.expectedGain = dGain;
                                    CurrentFromOrbitTo.maxLoss = dLoss;

                                }

                            }
                        }
                    }
                }
            }
        }

        private void PrintFromOrbitToPoitionAdvice()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\FromOrbitToPoitionAdvice.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Size", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));
                for (int position = 1; position < 100; position++)
                {
                    FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = FromOrbitToPoitionAdvice[position];
                    if (_FromOrbitToPoitionAdvice != null)
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", position, _FromOrbitToPoitionAdvice.dir,
                            _FromOrbitToPoitionAdvice.returnn, _FromOrbitToPoitionAdvice.abort, _FromOrbitToPoitionAdvice.expectedGain,
                            _FromOrbitToPoitionAdvice.maxLoss));
                }

            }

        }

        private void InitializeFromOrbitToWaitAdvice()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                FromOrbitToWaitAdvice[SizeFrom] = null;
        }

        private void SetFromOrbitToWaitAdvice()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                double expectedLossWithNoDir = 0;
                double expectedGainWithNoDir = 0;

                bool bCanSet = true;

                for (int SizeTo = 1; SizeTo < 100; SizeTo++)
                {
                    double probabilityToMove = SizeMovesStatistics[SizeFrom, SizeTo];
                    if (probabilityToMove > 0)
                    {
                        bCanSet = FromOrbitToPoitionAdvice[SizeTo] != null;
                        if (!bCanSet)
                            break;
                        else
                        {
                            expectedLossWithNoDir += probabilityToMove * FromOrbitToPoitionAdvice[SizeTo].maxLoss;
                            expectedGainWithNoDir += probabilityToMove * FromOrbitToPoitionAdvice[SizeTo].expectedGain;
                        }
                    }

                }

                if (bCanSet)
                {
                    FromOrbitToWaitAdvice NoDirFromOrbitTo = new FromOrbitToWaitAdvice();
                    NoDirFromOrbitTo.maxLoss = expectedLossWithNoDir;
                    NoDirFromOrbitTo.expectedGain = expectedGainWithNoDir;
                    FromOrbitToWaitAdvice[SizeFrom] = NoDirFromOrbitTo;
                }
            }
        }
        private void PrintFromOrbitToWaitAdvice()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\FromOrbitToWaitAdvice.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3}", "Size", "Dir", "ExpectedGain", "MaxLoss"));
                for (int position = 1; position < 100; position++)
                {
                    FromOrbitToWaitAdvice _FromOrbitToWaitAdvice = FromOrbitToWaitAdvice[position];
                    if (_FromOrbitToWaitAdvice != null)
                        stream.WriteLine(string.Format("{0},{1},{2},{3}", position, _FromOrbitToWaitAdvice.dir,
                            _FromOrbitToWaitAdvice.expectedGain,
                            _FromOrbitToWaitAdvice.maxLoss));
                }

            }

        }
        private void InitializeBestFromOrbitAdvice()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                BestFromOrbitAdvice[SizeFrom] = null;
        }

        private void SetBestFromOrbitAdvice()
        {
            // if Wait advisse we lead to better gains than
            // position advise use wait
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                FromOrbitToAdvice _BestFromOrbitTo;
                FromOrbitToPoitionAdvice BestFromOrbitToAdvice = FromOrbitToPoitionAdvice[SizeFrom];
                FromOrbitToWaitAdvice BestFromOrbitToWaitAdvice = FromOrbitToWaitAdvice[SizeFrom];

                if (BestFromOrbitToAdvice != null)
                    _BestFromOrbitTo = BestFromOrbitToAdvice;
                else
                    _BestFromOrbitTo = BestFromOrbitToWaitAdvice;

                if (BestFromOrbitToAdvice != null && BestFromOrbitToWaitAdvice != null &&
                    BestFromOrbitToWaitAdvice.expectedGain >0 &&
                     BestFromOrbitToAdvice.expectedGain + BestFromOrbitToWaitAdvice.maxLoss <
                     BestFromOrbitToWaitAdvice.expectedGain + BestFromOrbitToWaitAdvice.maxLoss)
                {
                    _BestFromOrbitTo = BestFromOrbitToWaitAdvice;
                }

                BestFromOrbitAdvice[SizeFrom] = _BestFromOrbitTo;
            }
        }

        private void PrintBestPositionFromOrbitTo()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestPositionFromOrbitTo.csv";

            using (var stream = File.CreateText(file))
            {

                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Size", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));

                for (int size = 1; size < 100; size++)
                {
                    FromOrbitToAdvice BestFromOrbitTo = BestFromOrbitAdvice[size];

                    if (BestFromOrbitTo != null)
                    {
                        if (BestFromOrbitTo.dir != 0)
                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestFromOrbitTo.dir, ((FromOrbitToPoitionAdvice)BestFromOrbitTo).returnn,
                              ((FromOrbitToPoitionAdvice)BestFromOrbitTo).abort, BestFromOrbitTo.expectedGain,BestFromOrbitTo.maxLoss));

                          else
                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestFromOrbitTo.dir, -9,
                              -9, BestFromOrbitTo.expectedGain, BestFromOrbitTo.maxLoss));

                    }

                   
                }
            }
        }

        private void InitializeBestFromPositionToAdvice()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                for (int Dir=-1;Dir<2;Dir+=2)
                BestFromPositionToAdvice[SizeFrom,StaticFunctions.DirToArrayIndex(Dir)] = null;
        }

        private void SetBestFromPositionToAdvice()
        {
            //If FromOrbit-PoitionAdvice has Gain+5>0 && same dir ->hold position
            //Else abort

            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
 
                FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = FromOrbitToPoitionAdvice[SizeFrom];
    
                if (_FromOrbitToPoitionAdvice != null && _FromOrbitToPoitionAdvice.expectedGain+5>0)
                {
                    FromPositionToHoldAdvice _FromPositionToHoldAdvice = new FromPositionToHoldAdvice();
                    _FromPositionToHoldAdvice.dir = _FromOrbitToPoitionAdvice.dir;
                    _FromPositionToHoldAdvice.returnn = _FromOrbitToPoitionAdvice.returnn;
                    _FromPositionToHoldAdvice.abort = _FromOrbitToPoitionAdvice.abort;
                    _FromPositionToHoldAdvice.expectedGain = _FromOrbitToPoitionAdvice.expectedGain+5;
                    _FromPositionToHoldAdvice.maxLoss = _FromOrbitToPoitionAdvice.maxLoss + 5;
                    BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(_FromOrbitToPoitionAdvice.dir)] =
                        _FromPositionToHoldAdvice;
                    BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(-1*_FromOrbitToPoitionAdvice.dir)] =
                           new FromPositionToAbortAdvice();

                }
                else
                {
                    BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(_FromOrbitToPoitionAdvice.dir)] =
                         new FromPositionToAbortAdvice();
                    BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(-1 * _FromOrbitToPoitionAdvice.dir)] =
                           new FromPositionToAbortAdvice();
                }
             }
        }

        private void PrintBestFromPositionToAdvice()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestFromPositionToAdvice.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", "Size", "PositionDir", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));
                for (int size = 1; size < 100; size++)
                {
                    for (int Dir = -1; Dir < 2; Dir += 2)
                    {
                        FromPositionToAdvice _BestFromPositionTo = BestFromPositionToAdvice[size, StaticFunctions.DirToArrayIndex(Dir)];

                        if (_BestFromPositionTo != null)
                        {
                            if (_BestFromPositionTo is FromPositionToHoldAdvice) //

                                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", size,Dir, _BestFromPositionTo.dir, ((FromPositionToHoldAdvice)_BestFromPositionTo).returnn,
                                 ((FromPositionToHoldAdvice)_BestFromPositionTo).abort, _BestFromPositionTo.expectedGain, _BestFromPositionTo.maxLoss));
                            else
                                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", size,Dir, _BestFromPositionTo.dir, -9,
                                    -9, _BestFromPositionTo.expectedGain, _BestFromPositionTo.maxLoss));

                        }

                    }
                }
            }
        }
  

        private void SetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            Gain[StaticFunctions.PositionToArrayIndex(p_iPosition) , StaticFunctions.DirToArrayIndex(p_dir) / 2,
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)] = p_value;
        }

        private void AddToGain(double p_dPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            int iposition = dTOi(p_dPosition);

            Gain[StaticFunctions.PositionToArrayIndex(iposition), StaticFunctions.DirToArrayIndex(p_dir) / 2,
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)] += p_value;
        }

        private double GetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort)
        {
            return Gain[StaticFunctions.PositionToArrayIndex(p_iPosition), StaticFunctions. DirToArrayIndex(p_dir) / 2,
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)];
        }
        private int dTOi(double p_dValue)
        {
            int iValue = (int)Math.Floor(p_dValue);
            if (iValue < 0)
                iValue = 0;
            if (iValue > 100)
                iValue = 100;
            return iValue;


        }
    }


   
}


