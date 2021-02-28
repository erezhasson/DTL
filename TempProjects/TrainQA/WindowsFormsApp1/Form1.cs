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

        Double[,,,] Gain = new Double[9, 2, 11, 11]; //Position1-9, dir {-1,1}, Return 0-10, Abort 0-10
        int[] Sizes = { 5, 6, 7, 6, 5, 4, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 7, 8, 9, 10 };

        private void btnGo_Click(object sender, EventArgs e)
        {
            //CalcGain(8, -1, 2, 10,20); for testing

            //Initilize Gain to 0
            InitializeGain();//Initilize Gain to 0

            CalcAllGains();
            PrintResults();



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

        private  void PrintResults ()
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
}
