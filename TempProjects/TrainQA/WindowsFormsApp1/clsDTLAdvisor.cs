using DTLExpert.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTLExpert
{
    class clsDTLAdvisor
    {
        Action[] BestPositionActions = new Action[100]; //Position1-99
        Action[] BestAbortActions = new Action[100]; //Position1-99

        public clsDTLAdvisor()
        {
            //Get trained actions
            GetBestPositionAction();
            GetBestAbortAction();


        }
        private void GetBestPositionAction()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestPositionAction.csv");
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

                ActionWithDir BestPositionAction = new ActionWithDir();
                BestPositionAction.dir = dir;
                BestPositionAction.returnn = Returnn;
                BestPositionAction.abort = abort;
                BestPositionAction.expectedGain = dGain;
                BestPositionAction.maxLoss = dMaxloss;

                BestPositionActions[position] = BestPositionAction;

            }

        }
        private void GetBestAbortAction()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\BestAbortAction.csv");
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

                ActionWithDir BestAbortAction = new ActionWithDir();
                BestAbortAction.dir = dir;
                BestAbortAction.returnn = Returnn;
                BestAbortAction.abort = abort;
                BestAbortAction.expectedGain = dGain;
                BestAbortAction.maxLoss = dMaxloss;

                BestAbortActions[position] = BestAbortAction;

            }

        }

        public Action Advise (State inState)
        {
            Action outAction = null;
            int CurrPosition = inState.position;
            int CurrDir = inState.dir;
            Action BestPositionAction = BestPositionActions[CurrPosition];

            if (CurrDir == 0) //Test entry advise
            {
                if (BestPositionAction != null)
                {
                    if (BestPositionAction.expectedGain > 0)
                        outAction = BestPositionAction;

                }
            }
            if (CurrDir != 0) //Test update advise
            {
                if (BestPositionAction != null)
                {
                    if (BestPositionAction.dir == CurrDir && BestPositionAction.expectedGain > 0)
                        outAction = BestPositionAction;
                    if (BestPositionAction.dir != CurrDir)
                        outAction = new ActionWithNoDir();
                }
                if (outAction == null) //no update/abort advise. Check abort
                {
                    Action BestAbortAction = BestAbortActions[CurrPosition];
                    if (BestAbortAction != null && BestAbortAction.expectedGain < 0)
                        outAction = new ActionWithNoDir();
                }

            }
                 
            return outAction;

        }
    }

}
