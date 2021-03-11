using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
     public partial class Form1 : Form
     {
          public Form1()
          {
               InitializeComponent();
          }

        //int[] Sizes = { 5, 6, 7, 6, 5, 4, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 7, 8, 9, 10 };
        //int[] Sizes = { 5, 6, 7, 6, 5, 4, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 4, 2, 1, 0 };
        //int[] Sizes = { 5,6,7,6,5,4,3,4,5,6,7,8,7,6,-9,5,4,5,6,4,2,1,0 , 5, 6, 7, 6, 5, 4, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 7, 8, 9, 10 };
        //int[] Sizes = { 50, 60, 70, 60, 50, 40, 30, 40, 50, 60, 70, 80, 70, 60, -9, 50, 40, 50, 60, 40, 20, 10, 0, 50, 60, 70, 60, 50, 40, 30, 40, 50, 60, 70, 80, 70, 60, 50, 40, 50, 60, 70, 80, 90, 100 };
          double[] Sizes;
          Double[,,,] Gain = new Double[99, 2, 101, 101]; //Position1-99, dir {-1,1}, Return 0-100, Abort 0-100
          int[,] SizeMovesCount = new int[100, 101]; //Position1-99, Position1-100
          int[,,] tmpLastCalcIndexInArray = new int[101,101,101]; //Position0-100, Return 0-100, Abort 0-100
          Double[,] SizeMovesStatistics = new Double[100, 101]; //Position1-99, Position1-100
          ActionWithDir[] ActionsWithDir = new ActionWithDir[100]; //Position1-99
          ActionWithNoDir[] ActionsWithNoDir = new ActionWithNoDir[100]; //Position1-99
          Action[] BestActions = new Action[100]; //Position1-99

        private void btnGo_Click(object sender, EventArgs e)
        {
                //Get sizes data
                GetSizesData();
                //Testing start
                //int i75Pos = -9;
                //for (i75Pos = 1; i75Pos < Sizes.Length - 1; i75Pos++)
                //        if (Sizes[i75Pos]>=75)
                //            break ;
                //CalcGain(Sizes[i75Pos], 1, 95, 0, i75Pos+1);
                //Terting end

                //Initilize Gain to 0
                InitializeGain();//Initilize Gain to 0
                InitializetmpLastcalcPositions();// Initilize Calc Position to -1
                CalcAllGains();
                //PrintGains();
                //PrintCalcPositions();

                InitializeSizeMovesCount();
                CalcSizeMovesCount();
                InitializeSizeMovesStatistics();
                CalcSizeMovesStatistics();
                // PrintSizeMovesStatistics();

                InitializeActionWithDir();
                SetActionWithDir();
                InitializeActionsWithNoDir();
                SetActionsWithNoDir();
                InitializeBestActions();
                SetBestActions();
                PrintBestAction();

                MessageBox.Show("done...");
          }

        private void GetSizesData ()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeData.csv");
            Sizes= new double[lines.Length-1];
            int pos = -1;
            foreach (string line in lines)
            {
                if (pos==-1)
                {
                    pos ++;
                    continue; //Skip header
                }

                double dSize=-9;
                if (Double.TryParse(line, out dSize))
                    Sizes[pos] = dSize;
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
            progressBar1.Value = ipbSize;
  
            for (int iSize = 0; iSize <= Sizes.Length - 1; iSize++)
            {
                int inewpbSize = 100 * iSize / (Sizes.Length - 1) ;
                if (inewpbSize> ipbSize+1)
                {
                    ipbSize = inewpbSize;
                    progressBar1.Value = ipbSize;
                    lblPB.Text = (iSize+1).ToString() + "/" + Sizes.Length.ToString();
                    Application.DoEvents();

                }

                double dposition = Sizes[iSize];
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
        private double CalcGain(double  p_dPosition, int p_dir, int p_Returnn, int p_abort, int p_StartiSize)
        {
            int iposition = dTOi(p_dPosition);
            double dGain = 0;

            if (p_StartiSize > tmpLastCalcIndexInArray[iposition, p_Returnn, p_abort])
            {
                for (int iSize = p_StartiSize; iSize < Sizes.Length; iSize++)
                {
                    double dsize = Sizes[iSize];
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
            {
                for (int position = 1; position < 100; position++)
                {
                    for (int dir = -1; dir < 2; dir += 2)
                    {
                        for (int Returnn = 0; Returnn <= 100; Returnn++)
                        {
                            for (int abort = 0; abort <= 100; abort++)

                            {
                                double dGain = GetGain(p_iPosition: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort);
                                if (dGain>0)
                                         Debug.Print   (position + "," + dir + ", " + Returnn + ", " + abort + "," + dGain.ToString());
                            }
                        }
                    }
                }

            }

        }

        private void PrintCalcPositions()
        {
            Debug.Print("Position\tReturn\tAbort\tIndex\n");

            for (int i = 0; i < 101; i++)
            {
                for (int j = 0; j < 101; j++)
                {
                    for (int k = 0; k < 101; k++)
                    {
                        Debug.Print(i + "\t\t" + j + "\t\t" + k + "\t\t" + tmpLastCalcIndexInArray[i, j, k]);
                    }
                    Debug.Print("\n");
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
            for (int indexSizeFrom = 0; indexSizeFrom < Sizes.Length - 1; indexSizeFrom++)
            {
                double dSizeFrom = Sizes[indexSizeFrom];
                double  dSizeTo = Sizes[indexSizeFrom + 1];
                if (dSizeFrom!=-9 && dSizeTo!=-9 && dSizeFrom<100 && dSizeFrom>0)
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
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
            {
                for (int SizeTo = 1; SizeTo < 101; SizeTo++)
                {
                    Debug.Print(SizeFrom + "," + SizeTo + "," + SizeMovesStatistics[SizeFrom, SizeTo]);
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
                            double dGain = GetGain(p_iPosition: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort);
                            if (dGain <= 0)  //No gain here
                                continue;

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
        private void InitializeBestActions()
        {
            for (int SizeFrom = 1; SizeFrom < 100; SizeFrom++)
                BestActions[SizeFrom] = null;
        }

        private void SetBestActions()
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
                    BestActionWithNoDir.expectedGain != 0 &&
                     BestActionWithDir.expectedGain + BestActionWithNoDir.maxLoss < 
                     BestActionWithNoDir.expectedGain + BestActionWithNoDir.maxLoss)
                {
                    BestAction = BestActionWithNoDir;
                }

                BestActions[SizeFrom] = BestAction;
            }
        }

        private void PrintBestAction()
          {
               for (int size = 1; size < 100; size++)
               {
                    Action BestAction = BestActions[size];

                    if (BestAction != null)
                    {
                        if (BestAction.dir!=0)
                         Debug.Print(size + "," + BestAction.dir + "," + ((ActionWithDir)BestAction).returnn + "," + ((ActionWithDir)BestAction).abort + "," +
                            BestAction.expectedGain + "," + BestAction.maxLoss);
                        else
                        Debug.Print(size + "," + BestAction.dir + "," + "-9" + "," + "-9" + "," +
                             BestAction.expectedGain + "," + BestAction.maxLoss);

                }

                //      else
                //          {
                //               Debug.Print(size + "-9,-9,-9-,-9,-9,-9");
                //          }
            }
        }


            private void SetGain(int  p_iPosition, int p_dir, int p_Returnn, int p_abort, Double p_value)
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


     public class ActionWithNoDir : Action
     {
          public ActionWithNoDir()
          {
               dir = 0;
          }
     }

     public class ActionWithDir : Action
     {
          public int returnn;
          public int abort;
     }

     public class Action
     {
          public int dir;
          public double expectedGain;
          public double maxLoss;
     }
}
