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
        public double[] inSizeSamples;
        double[,,,] Gain = new Double[99, 2, 101, 101]; //Position1-99, dir {-1,1}, Return 0-100, Abort 0-100
        int[,] SizeMovesCount = new int[100, 101]; //StarSize1-99, StartSize1-100
        int[,,] tmpLastCalcIndexInArray = new int[101, 101, 101]; //Position0-100, Return 0-100, Abort 0-100
        double[,] SizeMovesStatistics = new Double[100, 101]; //StarSize1-99, StarSize1-100
        FromOrbitToPoitionAdvice[] FromOrbitToPoitionAdvice = new FromOrbitToPoitionAdvice[100]; //StarSize1-99
        FromOrbitToWaitAdvice[] FromOrbitToWaitAdvice = new FromOrbitToWaitAdvice[100]; //StarSize1-99
        public FromOrbitToAdvice[] BestFromOrbitAdvice = new FromOrbitToAdvice[100]; //StarSize1-99
        public FromPositionToAdvice[,] BestFromPositionToAdvice = new FromPositionToAdvice[100, 2]; //StarSize1-99, dir {-1,1}

        public void Go(bool bRecalcGains)
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
                aCh[0] = ',';
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
            JobTrainProgressTick(0, inSizeSamples.Length);

            for (int iSizeSampleIndex = 0; iSizeSampleIndex <= inSizeSamples.Length - 1; iSizeSampleIndex++)
            {
                //Advance progress event
                int inewpbSize = 100 * iSizeSampleIndex / (inSizeSamples.Length - 1);
                if (inewpbSize > ipbSize + 1)
                {
                    ipbSize = inewpbSize;

                    JobTrainProgressTick(iSizeSampleIndex + 1, inSizeSamples.Length);
                }



                double dSize = inSizeSamples[iSizeSampleIndex];
                int iSize = StaticFunctions.dTOi(dSize);
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 100; Returnn++)
                    {
                        for (int abort = 0; abort <= 100; abort++)
                        {
                            if (IsValidCombination(dir, Returnn, abort, iSize))
                           {
                                if (iSizeSampleIndex > tmpLastCalcIndexInArray[iSize, Returnn, abort])
                                {
                                    double GainToSet = CalcGain(p_dPosition: dSize, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_StartiSize: iSizeSampleIndex + 1);
                                    AddToGain(p_iSize: iSize, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: GainToSet);
                                }
                            }
                        }
                    }
                }
            }
        }
        private double CalcGain(double p_dPosition, int p_dir, int p_Returnn, int p_abort, int p_StartiSize)
        {
            int iposition = StaticFunctions.dTOi(p_dPosition);
            double dGain = 0;

            if (p_StartiSize > tmpLastCalcIndexInArray[iposition, p_Returnn, p_abort])
            {
                for (int iSize = p_StartiSize; iSize < inSizeSamples.Length; iSize++)
                {
                    double dsize = inSizeSamples[iSize];
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
                                if (IsValidCombination(dir, Returnn, abort, position))
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
            for (int indexSizeFrom = 0; indexSizeFrom < inSizeSamples.Length - 1; indexSizeFrom++)
            {
                double dSizeFrom = inSizeSamples[indexSizeFrom];
                double dSizeTo = inSizeSamples[indexSizeFrom + 1];
                if (dSizeFrom != -9 && dSizeTo != -9 && dSizeFrom < 100 && dSizeFrom > 0)
                {
                    int iSizeFrom = StaticFunctions.dTOi(dSizeFrom);
                    int iSizeTo = StaticFunctions.dTOi(dSizeTo);
                    if (iSizeFrom != iSizeTo)
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
            for (int size = 1; size < 100; size++)
                FromOrbitToPoitionAdvice[size] = null;
        }
        private void SetFromOrbitToPoitionAdvice()
        {
            for (int size = 1; size < 100; size++)
            {
                //For each size - Set advise to Dir,Returnn,Abort with max gain 
                FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = null;
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 100; Returnn++)
                    {
                        for (int abort = 0; abort <= 100; abort++)
                        {
                            if (IsValidCombination(dir, Returnn, abort, size))
                            {

                                double dGain = GetGain(p_iPosition: size, p_dir: dir,
                                p_Returnn: Returnn, p_abort: abort);
                                if (dGain!=0)
                                { 

                                    double dLoss = dir * (abort - size) - 5;
                                    if (_FromOrbitToPoitionAdvice == null)
                                    {
                                        _FromOrbitToPoitionAdvice = new FromOrbitToPoitionAdvice();
                                        _FromOrbitToPoitionAdvice.dir = dir;
                                        _FromOrbitToPoitionAdvice.returnn = Returnn;
                                        _FromOrbitToPoitionAdvice.abort = abort;
                                        _FromOrbitToPoitionAdvice.expectedGain = dGain;
                                        _FromOrbitToPoitionAdvice.maxLoss = dLoss;
                                    }
                                    else
                                    {
                                        if (dGain > _FromOrbitToPoitionAdvice.expectedGain ||
                                            (dGain == _FromOrbitToPoitionAdvice.expectedGain && dLoss < _FromOrbitToPoitionAdvice.maxLoss))
                                        {
                                            _FromOrbitToPoitionAdvice.dir = dir;
                                            _FromOrbitToPoitionAdvice.returnn = Returnn;
                                            _FromOrbitToPoitionAdvice.abort = abort;
                                            _FromOrbitToPoitionAdvice.expectedGain = dGain;
                                            _FromOrbitToPoitionAdvice.maxLoss = dLoss;

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                FromOrbitToPoitionAdvice[size] = _FromOrbitToPoitionAdvice;

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
                //For each size - Set advise to wait and calculate 
                //Expected gain of all sizes according to probobailty to 
                // move to them in next step
                double expectedLossWithWait = 0;
                double expectedGainWithWait = 0;

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
                            expectedLossWithWait += probabilityToMove * FromOrbitToPoitionAdvice[SizeTo].maxLoss;
                            expectedGainWithWait += probabilityToMove * FromOrbitToPoitionAdvice[SizeTo].expectedGain;
                        }
                    }

                }

                if (bCanSet)
                {
                    FromOrbitToWaitAdvice _FromOrbitToWaitAdvice = new FromOrbitToWaitAdvice();
                    _FromOrbitToWaitAdvice.maxLoss = expectedLossWithWait;
                    _FromOrbitToWaitAdvice.expectedGain = expectedGainWithWait;
                    this.FromOrbitToWaitAdvice[SizeFrom] = _FromOrbitToWaitAdvice;
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
                FromOrbitToAdvice _BestFromOrbitTo = null;
                FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = FromOrbitToPoitionAdvice[SizeFrom];
                FromOrbitToWaitAdvice _FromOrbitToWaitAdvice = FromOrbitToWaitAdvice[SizeFrom];

                double FromOrbitToPoitionAdviceGain = -9;
                double FromOrbitToWaitAdviceGain = -9;
                if (_FromOrbitToPoitionAdvice != null && _FromOrbitToPoitionAdvice.expectedGain > 0)
                    FromOrbitToPoitionAdviceGain = _FromOrbitToPoitionAdvice.expectedGain;

                if (_FromOrbitToWaitAdvice != null && _FromOrbitToWaitAdvice.expectedGain > 0)
                    FromOrbitToWaitAdviceGain = _FromOrbitToWaitAdvice.expectedGain;

                //Compare gain taking position
                //to expected gain of waiting to next size move
                if (FromOrbitToPoitionAdviceGain > FromOrbitToWaitAdviceGain)
                    _BestFromOrbitTo = _FromOrbitToPoitionAdvice;
                if (FromOrbitToPoitionAdviceGain < FromOrbitToWaitAdviceGain)
                    _BestFromOrbitTo = _FromOrbitToWaitAdvice;


                if (_FromOrbitToPoitionAdvice != null && _FromOrbitToWaitAdvice != null &&
                   _FromOrbitToWaitAdvice.expectedGain > 0 &&
                    _FromOrbitToPoitionAdvice.expectedGain <
                    _FromOrbitToWaitAdvice.expectedGain)
                {
                    _BestFromOrbitTo = _FromOrbitToWaitAdvice;
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
                              ((FromOrbitToPoitionAdvice)BestFromOrbitTo).abort, BestFromOrbitTo.expectedGain, BestFromOrbitTo.maxLoss));

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
                for (int Dir = -1; Dir < 2; Dir += 2)
                    BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(Dir)] = null;
        }

        private void SetBestFromPositionToAdvice()
        {
            //If FromOrbit-PoitionAdvice has Gain+5>0 && same dir ->hold position
            //Else abort

            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {

                BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(1)] = null;
                BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(-1)] = null;

                FromOrbitToPoitionAdvice _FromOrbitToPoitionAdvice = FromOrbitToPoitionAdvice[SizeFrom];

                if (_FromOrbitToPoitionAdvice != null)
                {
                    if (_FromOrbitToPoitionAdvice.expectedGain + 5 > 0) //Add 5 because  enetring position cost 5 but we are in position
                    {
                        FromPositionToHoldAdvice _FromPositionToHoldAdvice = new FromPositionToHoldAdvice();
                        _FromPositionToHoldAdvice.dir = _FromOrbitToPoitionAdvice.dir;
                        _FromPositionToHoldAdvice.returnn = _FromOrbitToPoitionAdvice.returnn;
                        _FromPositionToHoldAdvice.abort = _FromOrbitToPoitionAdvice.abort;
                        _FromPositionToHoldAdvice.expectedGain = _FromOrbitToPoitionAdvice.expectedGain + 5;
                        _FromPositionToHoldAdvice.maxLoss = _FromOrbitToPoitionAdvice.maxLoss + 5;
                        BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(_FromOrbitToPoitionAdvice.dir)] =
                            _FromPositionToHoldAdvice;
                        BestFromPositionToAdvice[SizeFrom, StaticFunctions.DirToArrayIndex(-1 * _FromOrbitToPoitionAdvice.dir)] =
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

                                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", size, Dir, _BestFromPositionTo.dir, ((FromPositionToHoldAdvice)_BestFromPositionTo).returnn,
                                 ((FromPositionToHoldAdvice)_BestFromPositionTo).abort, _BestFromPositionTo.expectedGain, _BestFromPositionTo.maxLoss));
                            else
                                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", size, Dir, _BestFromPositionTo.dir, -9,
                                    -9, _BestFromPositionTo.expectedGain, _BestFromPositionTo.maxLoss));

                        }

                    }
                }
            }
        }


        private void SetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            Gain[StaticFunctions.PositionToArrayIndex(p_iPosition), StaticFunctions.DirToArrayIndex(p_dir),
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)] = p_value;
        }

        private void AddToGain(int p_iSize, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {

            Gain[StaticFunctions.PositionToArrayIndex(p_iSize), StaticFunctions.DirToArrayIndex(p_dir),
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)] += p_value;
        }

        private double GetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort)
        {
            return Gain[StaticFunctions.PositionToArrayIndex(p_iPosition), StaticFunctions.DirToArrayIndex(p_dir),
                StaticFunctions.ReturnnToArrayIndex(p_Returnn), StaticFunctions.AbortToArrayIndex(p_abort)];
        }
        public bool IsValidCombination(int p_dir,int p_Returnn,int p_abort,int p_iSize)
        {
            return ((p_dir == 1 && p_Returnn > p_iSize && p_abort < p_iSize)
                    || (p_dir == -1 && p_Returnn < p_iSize && p_abort > p_iSize));


        }

    }


   
}


