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

        int[] Sizes = { 5, 6, 7, 6, 5, 4, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 7, 8, 9, 10 };
        Double[,,,] Gain = new Double[9, 2, 11, 11]; //Position1-9, dir {-1,1}, Return 0-10, Abort 0-10
        int[,] SizeMovesCount = new int[10, 10]; //Position1-9, Position1-9
        Double[,] SizeMovesStatistics = new Double[10,10]; //Position1-9, Position1-9

        Action[] Actions= new Action[10]; //Position1-9

        private void btnGo_Click(object sender, EventArgs e)
        {
            //CalcGain(8, -1, 2, 10,20); for testing

            //Initilize Gain to 0
            InitializeGain();//Initilize Gain to 0

            CalcAllGains();

            InitializeSizeMovesCount();
            CalcSizeMovesStatistics();


            SetBestAction();
            PrintBestAction();

            //PrintGains();




            MessageBox.Show("done...");
            
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
                                double GainToSet = CalcGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_StartiSize: iSize + 1);
                                AddToGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort, p_value: GainToSet);

                            }
                        }
                    }
                }

            }

        }
        private void CalcSizeMovesCount()
        {
            for (int indexSizeFrom = 0; indexSizeFrom <= 21 - 1; indexSizeFrom++)
            {
                int SizeFrom = Sizes[indexSizeFrom];
                int SizeTo = Sizes[indexSizeFrom+1];
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
                    //SizeMovesStatistics[SizeFrom, SizeTo] = ;
                }

                //calc statistics
                for (int SizeTo = 1; SizeTo < 10; SizeTo++)
                {
                    if (SumMoves > 0)
                        SizeMovesStatistics[SizeFrom, SizeTo] = SizeMovesCount[SizeFrom, SizeTo] / SumMoves;
                    else
                        SizeMovesStatistics[SizeFrom, SizeTo] = 0;
                }
            }

        }

        private void PrintGains ()
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
                                    (position + ","+ dir+", "+Returnn+", "+abort+","+
                                         GetGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort));
                            }
                        }
                    }
                }

            }

        }

 
        private void SetBestAction()
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
                               double dGain= GetGain(p_Position: position, p_dir: dir, p_Returnn: Returnn, p_abort: abort);
                                double  dLoss = dir * (abort - position) - 0.5;
                                Action CurrentAction = Actions[position];
                                if (CurrentAction == null)
                                {
                                    Action newAction = new Action();
                                    Actions[position] = newAction;
                                    newAction.dir = dir;
                                    newAction.returnn = Returnn;
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
                                        CurrentAction.expectedGain = dGain;
                                        CurrentAction.maxLoss = dLoss;
                                    }

                                }
                            }
                        }
                    }
                }

            }

        }

        private void PrintBestAction()
        {
            {
                for (int size = 1; size < 10; size++)
                {
                    Action CurrentAction = Actions[size];

                    Debug.Print(size + "," + CurrentAction.dir + "," + CurrentAction.expectedGain + "," + CurrentAction.maxLoss);
                }

            }

        }

        private double CalcGain(int p_Position, int p_dir, int p_Returnn, int p_abort, int p_StartiSize)
        {
            double dGain = 0;
            for (int iSize = p_StartiSize; iSize <= 21 ; iSize++)
            {
                int size = Sizes[iSize];
                if (p_dir==1 && p_Returnn<= size)
                {
                    dGain = size - p_Position - 0.5;
                    break;
                }
                if (p_dir == 1 && size <= p_abort)
                {
                    dGain = size - p_Position - 0.5;
                    break;
                }
                if (p_dir == -1 && size <= p_Returnn)
                {
                    dGain = p_Position- size - 0.5;
                    break;
                }
                if (p_dir == -1 && size >= p_abort)
                {
                    dGain = p_Position-size  - 0.5;
                    break;
                }


            }

            return dGain;
        }


        private void SetGain(int p_Position, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            Gain[p_Position - 1, (int)(p_dir + 1) / 2, p_Returnn, p_abort] = p_value;
        }

        private void AddToGain(int p_Position, int p_dir, int p_Returnn, int p_abort, Double p_value)
        {
            Gain[p_Position - 1, (int)(p_dir + 1) / 2, p_Returnn, p_abort] += p_value;
        }

        private double GetGain(int p_Position, int p_dir, int p_Returnn, int p_abort)
        {
            return Gain[p_Position - 1, (int)(p_dir + 1) / 2, p_Returnn, p_abort] ;
        }


    }
    public class Action
    {
        public int dir;
        public int returnn;
        public int abort;
        public double expectedGain;
        public double maxLoss;

    }
}
