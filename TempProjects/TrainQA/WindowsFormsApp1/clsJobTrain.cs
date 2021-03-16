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
        ActionWithDir[] ActionsWithDir = new ActionWithDir[100]; //Position1-99
        ActionWithNoDir[] ActionsWithNoDir = new ActionWithNoDir[100]; //Position1-99
        public Action[] BestPositionActions = new Action[100]; //Position1-99
        public Action[] BestAbortActions = new Action[100]; //Position1-99

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

            /* ActionWithDir  ActionWithDir */
            InitializeActionWithDir();
            SetActionWithDir();
            PrintActionWithDir();
            InitializeActionsWithNoDir();
            SetActionsWithNoDir();
            PrintActionWithNoDir();

            /* BestPositionActions   BestAbortActions*/

            InitializeBestPositionActions();
            SetBestPositionActions();
            PrintBestPositionAction();
            InitializeBestAbortActions();
            SetBestAbortActions();
            PrintBestAbortAction();
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
                for (int dir = -1; dir < 2; dir += 2)
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
        private void InitializeActionWithDir()
        {
            for (int position = 1; position < 100; position++)
                ActionsWithDir[position] = null;
        }
        private void SetActionWithDir()
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
                            ActionWithDir CurrentAction = ActionsWithDir[position];
                            if (CurrentAction == null)
                            {
                                ActionWithDir newAction = new ActionWithDir();
                                ActionsWithDir[position] = newAction;
                                newAction.dir = dir;
                                newAction.returnn = Returnn;
                                newAction.abort = abort;
                                newAction.expectedGain = dGain;
                                newAction.maxLoss = dLoss;
                            }
                            else
                            {
                                if (dGain > CurrentAction.expectedGain ||
                                    (dGain == CurrentAction.expectedGain && dLoss < CurrentAction.maxLoss))
                                {
                                    CurrentAction.dir = dir;
                                    CurrentAction.returnn = Returnn;
                                    CurrentAction.abort = abort;
                                    CurrentAction.expectedGain = dGain;
                                    CurrentAction.maxLoss = dLoss;

                                }

                            }
                        }
                    }
                }
            }
        }

        private void PrintActionWithDir()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\ActionWithDir.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Size", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));
                for (int position = 1; position < 100; position++)
                {
                    ActionWithDir ActionWithDir = ActionsWithDir[position];
                    if (ActionWithDir != null)
                        stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", position, ActionWithDir.dir,
                            ActionWithDir.returnn, ActionWithDir.abort, ActionWithDir.expectedGain,
                            ActionWithDir.maxLoss));
                }

            }

        }

        private void InitializeActionsWithNoDir()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                ActionsWithNoDir[SizeFrom] = null;
        }

        private void SetActionsWithNoDir()
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
                        bCanSet = ActionsWithDir[SizeTo] != null;
                        if (!bCanSet)
                            break;
                        else
                        {
                            expectedLossWithNoDir += probabilityToMove * ActionsWithDir[SizeTo].maxLoss;
                            expectedGainWithNoDir += probabilityToMove * ActionsWithDir[SizeTo].expectedGain;
                        }
                    }

                }

                if (bCanSet)
                {
                    ActionWithNoDir NoDirAction = new ActionWithNoDir();
                    NoDirAction.maxLoss = expectedLossWithNoDir;
                    NoDirAction.expectedGain = expectedGainWithNoDir;
                    ActionsWithNoDir[SizeFrom] = NoDirAction;
                }
            }
        }
        private void PrintActionWithNoDir()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\ActionWithNoDir.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3}", "Size", "Dir", "ExpectedGain", "MaxLoss"));
                for (int position = 1; position < 100; position++)
                {
                    ActionWithNoDir ActionWithNoDir = ActionsWithNoDir[position];
                    if (ActionWithNoDir != null)
                        stream.WriteLine(string.Format("{0},{1},{2},{3}", position, ActionWithNoDir.dir,
                            ActionWithNoDir.expectedGain,
                            ActionWithNoDir.maxLoss));
                }

            }

        }
        private void InitializeBestPositionActions()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                BestPositionActions[SizeFrom] = null;
        }

        private void SetBestPositionActions()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                Action BestAction;
                ActionWithDir BestActionWithDir = ActionsWithDir[SizeFrom];
                ActionWithNoDir BestActionWithNoDir = ActionsWithNoDir[SizeFrom];

                if (BestActionWithDir != null)
                    BestAction = BestActionWithDir;
                else
                    BestAction = BestActionWithNoDir;

                if (BestActionWithDir != null && BestActionWithNoDir != null &&
                    BestActionWithNoDir.expectedGain >0 &&
                     BestActionWithDir.expectedGain + BestActionWithNoDir.maxLoss <
                     BestActionWithNoDir.expectedGain + BestActionWithNoDir.maxLoss)
                {
                    BestAction = BestActionWithNoDir;
                }

                BestPositionActions[SizeFrom] = BestAction;
            }
        }

        private void PrintBestPositionAction()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestPosition.csv";

            using (var stream = File.CreateText(file))
            {

                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Size", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));

                for (int size = 1; size < 100; size++)
                {
                    Action BestAction = BestPositionActions[size];

                    if (BestAction != null)
                    {
                        if (BestAction.dir != 0)
                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestAction.dir, ((ActionWithDir)BestAction).returnn,
                              ((ActionWithDir)BestAction).abort, BestAction.expectedGain,BestAction.maxLoss));

                          else
                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestAction.dir, -9,
                              -9, BestAction.expectedGain, BestAction.maxLoss));

                    }

                   
                }
            }
        }

        private void InitializeBestAbortActions()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                BestAbortActions[SizeFrom] = null;
        }

        private void SetBestAbortActions()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                Action BestAction;
                ActionWithDir BestActionWithDir = ActionsWithDir[SizeFrom];
                ActionWithNoDir BestActionWithNoDir = ActionsWithNoDir[SizeFrom];

                if (BestActionWithDir != null)
                    BestAction = BestActionWithDir;
                else
                    BestAction = BestActionWithNoDir;

                if (BestActionWithDir != null && BestActionWithNoDir != null &&
                    BestActionWithNoDir.expectedGain < 0 &&
                     BestActionWithDir.expectedGain  <
                     BestActionWithNoDir.expectedGain )
                {
                    BestAction = BestActionWithNoDir;
                }

                
                BestAbortActions[SizeFrom] = BestAction;
            }
        }

        private void PrintBestAbortAction()
        {
            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestAbortAction.csv";
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Size", "Dir", "Returnn",
                 "Abort", "ExpectedGain", "MaxLoss"));
                for (int size = 1; size < 100; size++)
                {
                    Action BestAction = BestAbortActions[size];

                    if (BestAction != null)
                    {
                        if (BestAction.dir != 0)

                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestAction.dir, ((ActionWithDir)BestAction).returnn,
                             ((ActionWithDir)BestAction).abort, BestAction.expectedGain, BestAction.maxLoss));
                        else
                            stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", size, BestAction.dir, -9,
                                -9, BestAction.expectedGain, BestAction.maxLoss));

                    }

                    //      else
                    //          {
                    //               Debug.Print(size + "-9,-9,-9-,-9,-9,-9");
                    //          }
                }
            }
        }

        private void SetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            Gain[p_iPosition - 1, (p_dir + 1) / 2, p_Returnn, p_abort] = p_value;
        }

        private void AddToGain(double p_dPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            int iposition = dTOi(p_dPosition);

            Gain[iposition - 1, (p_dir + 1) / 2, p_Returnn, p_abort] += p_value;
        }

        private double GetGain(int p_iPosition, int p_dir, int p_Returnn, int p_abort)
        {
            return Gain[p_iPosition - 1, (p_dir + 1) / 2, p_Returnn, p_abort];
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


