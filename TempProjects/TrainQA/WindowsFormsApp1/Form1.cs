using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
          int[] Sizes = { 5,6,7,6,5,4,3,4,5,6,7,8,7,6,5,4,5,6,4,2,1,0 };
          Double[,,,] Gain = new Double[9, 2, 11, 11]; //Position1-9, dir {-1,1}, Return 0-10, Abort 0-10
          int[,] SizeMovesCount = new int[10, 11]; //Position1-9, Position1-9
          int[,,] calcPositions = new int[11,11,11]; //Position0-10, Return 0-10, Abort 0-10
          Double[,] SizeMovesStatistics = new Double[10, 11]; //Position1-9, Position1-9
          ActionWithDir[] ActionsWithDir = new ActionWithDir[10]; //Position1-9
          ActionWithNoDir[] ActionsWithNoDir = new ActionWithNoDir[10]; //Position1-9
          Action[] BestActions = new Action[10]; //Position1-9

          private void btnGo_Click(object sender, EventArgs e)
          {
               //CalcGain(8, -1, 2, 10,20); for testing

               //Initilize Gain to 0
               InitializeGain();//Initilize Gain to 0
               InitializeCalcPositions();// Initilize Calc Position to -1
               CalcAllGains();
               //PrintGains();
               //PrintCalcPositions();

               InitializeSizeMovesCount();
               CalcSizeMovesCount();
               CalcSizeMovesStatistics();
            // PrintSizeMovesStatistics();

                SetActionWithDir();
                SetActionsWithNoDir();
               SetBestActions();
               PrintBestAction();

               MessageBox.Show("done...");
          }

        private void PrintGains()
        {
            {
                for (int position = 1; position < 10; position++)
                {
                    for (int dir = -1; dir < 2; dir += 2)
                    {
                        for (int Returnn = 0; Returnn <= 10; Returnn++)
                        {
                            for (int abort = 0; abort <= 10; abort++)

                            {
                                Debug.Print
                                    (position + "," + dir + ", " + Returnn + ", " + abort + "," +
                                         GetGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort));
                            }
                        }
                    }
                }

            }

        }

        private void PrintCalcPositions()
        {
            Debug.Print("Position\tReturn\tAbort\tIndex\n");

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int k = 0; k < 11; k++)
                    {
                        Debug.Print(i + "\t\t" + j + "\t\t" + k + "\t\t" + calcPositions[i, j, k]);
                    }
                    Debug.Print("\n");
                }
            }
        }
        private void InitializeGain()
        {
            for (int position = 1; position < 10; position++)
            {
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 10; Returnn++)
                    {
                        for (int abort = 0; abort <= 10; abort++)

                        {
                            SetGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: 0);
                        }
                    }
                }
            }

        }
        private void InitializeCalcPositions()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int k = 0; k < 11; k++)
                    {
                        calcPositions[i, j, k] = -1;
                    }
                }
            }
        }
        private void CalcAllGains()
        {
            for (int iSize = 0; iSize <= 21 - 1; iSize++)
            {
                int position = Sizes[iSize];
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 10; Returnn++)
                    {
                        for (int abort = 0; abort <= 10; abort++)
                        {
                            if ((dir == 1 && Returnn > position && abort < position)
                                || (dir == -1 && Returnn < position && abort > position))
                            {
                                if (iSize > calcPositions[position, Returnn, abort])
                                {
                                    double GainToSet = CalcGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_StartiSize: iSize + 1);
                                    AddToGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: GainToSet);
                                }
                            }
                        }
                    }
                }
            }
        }
        private double CalcGain(int p_Position, int p_dir, int p_Returnn, int p_abort, int p_StartiSize)
        {
            double dGain = 0;

            if (p_StartiSize > calcPositions[p_Position, p_Returnn, p_abort])
            {
                for (int iSize = p_StartiSize; iSize <= 21; iSize++)
                {
                    int size = Sizes[iSize];
                    if (p_dir == 1 && p_Returnn <= size)
                    {
                        dGain = size - p_Position - 0.5;
                        calcPositions[p_Position, p_Returnn, p_abort] = iSize + 1;
                        break;
                    }
                    if (p_dir == 1 && size <= p_abort)
                    {
                        dGain = size - p_Position - 0.5;
                        calcPositions[p_Position, p_Returnn, p_abort] = iSize + 1;
                        break;
                    }
                    if (p_dir == -1 && size <= p_Returnn)
                    {
                        dGain = p_Position - size - 0.5;
                        calcPositions[p_Position, p_Returnn, p_abort] = iSize + 1;
                        break;
                    }
                    if (p_dir == -1 && size >= p_abort)
                    {
                        dGain = p_Position - size - 0.5;
                        calcPositions[p_Position, p_Returnn, p_abort] = iSize + 1;
                        break;
                    }
                }
            }

            return dGain;
        }
        private void InitializeSizeMovesCount()
        {
            for (int SizeFrom = 1; SizeFrom < 10; SizeFrom++)
            {
                for (int SizeTo = 1; SizeTo < 10; SizeTo++)
                {
                    SizeMovesCount[SizeFrom, SizeTo] = 0;
                }
            }

        }
        private void CalcSizeMovesCount()
        {
            for (int indexSizeFrom = 0; indexSizeFrom <= 21 - 1; indexSizeFrom++)
            {
                int SizeFrom = Sizes[indexSizeFrom];
                int SizeTo = Sizes[indexSizeFrom + 1];
                SizeMovesCount[SizeFrom, SizeTo] += 1;
            }

        }

        private void CalcSizeMovesStatistics()
        {
            for (int SizeFrom = 1; SizeFrom < 10; SizeFrom++)
            {
                //calc total
                int SumMoves = 0;
                for (int SizeTo = 1; SizeTo < 10; SizeTo++)
                {
                    SumMoves += SizeMovesCount[SizeFrom, SizeTo];
                }

                //calc statistics
                for (int SizeTo = 1; SizeTo < 10; SizeTo++)
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
            for (int SizeFrom = 1; SizeFrom < 10; SizeFrom++)
            {
                for (int SizeTo = 1; SizeTo < 11; SizeTo++)
                {
                    Debug.Print(SizeFrom + "," + SizeTo + "," + SizeMovesStatistics[SizeFrom, SizeTo]);
                }
            }

        }
        private void SetActionWithDir()
        {
            for (int position = 1; position < 10; position++)
            {
                for (int dir = -1; dir < 2; dir += 2)
                {
                    for (int Returnn = 0; Returnn <= 10; Returnn++)
                    {
                        for (int abort = 0; abort <= 10; abort++)
                        {
                            double dGain = GetGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort);
                            double dLoss = dir * (abort - position) - 0.5;
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

  
        private void SetActionsWithNoDir()
        {
            for (int SizeFrom = 1; SizeFrom < 10; SizeFrom++)
            {
                double expectedLossWithNoDir = 0;
                double expectedGainWithNoDir = 0;

                for (int SizeTo = 1; SizeTo < 10; SizeTo++)
                {
                    double probabilityToMove = SizeMovesStatistics[SizeFrom, SizeTo];
                    if (probabilityToMove > 0)
                    {
                        expectedLossWithNoDir += probabilityToMove * ActionsWithDir[SizeTo].maxLoss;
                        expectedGainWithNoDir += probabilityToMove * ActionsWithDir[SizeTo].expectedGain;
                    }

                }

                ActionWithNoDir NoDirAction = new ActionWithNoDir();
                NoDirAction.maxLoss = expectedLossWithNoDir;
                NoDirAction.expectedGain = expectedGainWithNoDir;
                ActionsWithNoDir[SizeFrom] = NoDirAction;
            }
        }

        
        private void SetBestActions()
        {
            for (int SizeFrom = 1; SizeFrom < 10; SizeFrom++)
            {
                Action BestAction;
                ActionWithDir BestActionWithDir = ActionsWithDir[SizeFrom];
                ActionWithNoDir BestActionWithNoDir = ActionsWithNoDir[SizeFrom];

                if (BestActionWithDir != null)
                    BestAction = BestActionWithDir;
                else
                    BestAction = BestActionWithNoDir;

                if (BestActionWithDir != null && BestActionWithNoDir.expectedGain != 0 &&
                     BestActionWithDir.expectedGain + BestActionWithNoDir.maxLoss < BestActionWithNoDir.expectedGain + BestActionWithNoDir.maxLoss)
                {
                    BestAction = BestActionWithNoDir;
                }

                BestActions[SizeFrom] = BestAction;
            }
        }


        private void PrintBestAction()
          {
               for (int size = 1; size < 10; size++)
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

                else
                    {
                         Debug.Print(size + "-9,-9,-9-,-9,-9,-9");
                    }
               }
          }

 

          private void SetGain(int p_Position, int p_dir, int p_Returnn, int p_abort, Double p_value)
          {
               Gain[p_Position - 1, (p_dir + 1) / 2, p_Returnn, p_abort] = p_value;
          }

          private void AddToGain(int p_Position, int p_dir, int p_Returnn, int p_abort, Double p_value)
          {
               Gain[p_Position - 1, (p_dir + 1) / 2, p_Returnn, p_abort] += p_value;
          }

          private double GetGain(int p_Position, int p_dir, int p_Returnn, int p_abort)
          {
               return Gain[p_Position - 1, (p_dir + 1) / 2, p_Returnn, p_abort];
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
