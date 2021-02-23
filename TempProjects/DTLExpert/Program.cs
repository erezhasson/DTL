using System;
using System.Collections.Generic;
using System.IO;

namespace DTLExpert
{
     class Expert
     {
          private static List<double> m_Prices = new List<double>();
          private static List<double> m_Sizes = new List<double>();

          public static void readData(string filePath)
          {
               string[] fileLines = File.ReadAllLines(filePath);

               foreach (string line in fileLines)
               {
                    m_Prices.Add(double.Parse(line));
               }
          }

          private static void NormalizePrices()
          {
               bool bReach0, bReach10;
               double factor = 2.5 / (0.5 * 0.0018);
               double Chg = Math.Round(5 / factor, 5);
               double PrevLASTReach = m_Prices[0];
               double NextDown = m_Prices[0] - Chg;
               double NextUp = m_Prices[0] + Chg;
               double LASTReach;    
               double Size;

               m_Sizes.Add(5);
               for (int i = 0; i < m_Prices.Count; i++)
               {
                    bReach0 = m_Prices[i] < NextDown;
                    bReach10 = m_Prices[i] > NextUp;
                    LASTReach = bReach10 ? NextUp : (bReach0 ? NextDown : PrevLASTReach);
                    Size = factor * (m_Prices[i] - (bReach0 || bReach10 ? PrevLASTReach : LASTReach)) + 5;
                    Size = Math.Round(Size, 2);
                    NextDown = LASTReach - Chg;
                    NextUp = LASTReach + Chg;

                    PrevLASTReach = LASTReach;

                    if (i != 0)
                    {
                         m_Sizes.Add(Size);
                    }

                    //System.Console.Write(m_Prices[i].ToString() + ",");
                    //System.Console.Write(LASTReach.ToString() + ",");
                    //System.Console.Write(bReach0.ToString() + ",");
                    //System.Console.Write(bReach10.ToString() + ",");
                    //System.Console.Write(NextDown.ToString() + ",");
                    //System.Console.Write(NextUp.ToString() + ",");
                    //System.Console.WriteLine(Size.ToString() + ",");
               }                        
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
}
