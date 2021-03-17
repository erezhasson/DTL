using DTLExpert.Models;
using System;

using System.IO;

using System.Windows.Forms;

namespace DTLExpert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        clsDTLAdvisor Advisor;
        private void btnTrain_Click(object sender, EventArgs e)
        {
            //Get sizes data
            clsJobTrain JobTrain = new clsJobTrain();
            JobTrain.inSizes = GetSizesData();
            JobTrain.JobTrainProgressTick += jobTrainProgressTicked;

            JobTrain.Go(chkRecalcGains.Checked);




            MessageBox.Show("done...");
        }

        private double[] GetSizesData()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeData.csv");
            double[] Sizes = new double[lines.Length - 1];
            int pos = -1;
            foreach (string line in lines)
            {
                if (pos == -1)
                {
                    pos++;
                    continue; //Skip header
                }

                double dSize = -9;
                if (Double.TryParse(line, out dSize))
                    Sizes[pos] = dSize;
                pos++;
            }

            return Sizes;
        }

        private void jobTrainProgressTicked(int iCountDone, int iCountTotal)
        {
            int inewpbSize = 100 * iCountDone / iCountTotal;
            progressBar1.Value = inewpbSize;
            lblPB.Text = iCountDone.ToString() + "/" + iCountTotal.ToString();
            Application.DoEvents();


        }

        private void btnAdvise_Click(object sender, EventArgs e)
        {
            if ( Advisor==null)
                Advisor = new clsDTLAdvisor();
            State CurrState = new State(int.Parse(txtDir.Text), int.Parse(txtPosition.Text));
            Action outAction = Advisor.Advise(CurrState);
            if (outAction != null)
            {
                txtAdvisedDir.Text = outAction.dir.ToString();
                if (outAction.dir != 0)
                {
                    ActionWithDir outActionWithDir = (ActionWithDir)outAction;
                    txtAdvisedReturn.Text = outActionWithDir.returnn.ToString();
                    txtAdvisedAbort.Text = outActionWithDir.abort.ToString();
                }
                else
                {
                    txtAdvisedReturn.Text = "";
                    txtAdvisedAbort.Text = "";
                }
            }
            else
            {
                txtAdvisedDir.Text = "None";
                txtAdvisedReturn.Text = "";
                txtAdvisedAbort.Text = "";
            }

   

        }
    }
 
}
