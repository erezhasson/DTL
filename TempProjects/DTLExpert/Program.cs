using System;
using System.Collections.Generic;
using System.IO;

namespace DTLExpert
{
     class Expert
     {
          public static ExpertInput m_Input = new ExpertInput();

          public static void readData(string filePath)
          {
               string[] fileLines = File.ReadAllLines(filePath);

               foreach (string line in fileLines)
               {
                    m_Input.m_Prices.Add(double.Parse(line));
               }
          }

          private static List<NormalizedOutput> NormalizePrices()
          {
               List<double> prices = m_Input.m_Prices;
               List<NormalizedOutput> normalizedOutputList = new List<NormalizedOutput>();
               bool bReach0, bReach10;
               double factor = 2.5 / (0.5 * 0.0018);
               double Chg = Math.Round(5 / factor, 5);
               double PrevLASTReach = prices[0];
               double NextDown = prices[0] - Chg;
               double NextUp = prices[0] + Chg;
               double LASTReach;    
               double Size;

               
               for (int i = 0; i < prices.Count; i++)
               {
                    bReach0 = prices[i] < NextDown;
                    bReach10 = prices[i] > NextUp;
                    LASTReach = bReach10 ? NextUp : (bReach0 ? NextDown : PrevLASTReach);
                    Size = factor * (prices[i] - (bReach0 || bReach10 ? PrevLASTReach : LASTReach)) + 5;
                    Size = Math.Round(Size, 2);
                    NextDown = LASTReach - Chg;
                    NextUp = LASTReach + Chg;
                    PrevLASTReach = LASTReach;
                    normalizedOutputList.Add(new NormalizedOutput(Size, bReach10, bReach0));


                    //System.Console.Write(m_Prices[i].ToString() + ",");
                    //System.Console.Write(LASTReach.ToString() + ",");
                    //System.Console.Write(bReach0.ToString() + ",");
                    //System.Console.Write(bReach10.ToString() + ",");
                    //System.Console.Write(NextDown.ToString() + ",");
                    //System.Console.Write(NextUp.ToString() + ",");
                    //System.Console.WriteLine(Size.ToString() + ",");
               }

               return normalizedOutputList;
          }

          private static void TrainExpert()
          {

          }


          static void Main(string[] args)
          {
               readData("C:/Users/Oren/Desktop/לימודים/Git/DTL/Documents/Design/Price.csv");
               NormalizePrices();
               TrainExpert();

               // EvaluateExpert()
               // Prediction(Size) -> DIR(1, -1, 0 = do nothing), Return value, Abort value

          }
     }

     class ExpertInput
     {
          public List<double> m_Prices = new List<double>();
     }

     class NormalizedOutput
     {
          public double Size;
          public bool bEarnedList;
          public bool bAbortedList;

          public NormalizedOutput(double size, bool bEarnedList, bool bAbortedList)
          {
               Size = size;
               this.bEarnedList = bEarnedList;
               this.bAbortedList = bAbortedList;
          }
     }
}
