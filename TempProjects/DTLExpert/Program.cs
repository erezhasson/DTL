using System;
using System.Collections.Generic;
using System.IO;

namespace DTLExpert
{
     class Expert
     {
          private static ExpertInput m_Input = new ExpertInput();
          private static float[,,,] m_Gain;
          private enum eResults { Unkown = -1, AbortBeforeReturn = 0, ReturnBeforeAbort = 1 };
          private enum eDirections {NegDirection = -1, PosDirection = 1, NonDirection = 0 };

          private static void readData(string filePath)
          {
               string[] fileLines = File.ReadAllLines(filePath);

               foreach (string line in fileLines)
               {
                    m_Input.m_Prices.Add(float.Parse(line));
               }
          }

          private static void initGain(int pricesCount)
          {
               m_Gain = new float[2, pricesCount, 100, 100];

               for (int i = 0; i < 2; i++)
               {
                    for (int j = 0; j < pricesCount; j++)
                    {
                         for (int k = 0; k < 100; k++)
                         {
                              for (int m = 0; m < 100; m++)
                              {
                                   m_Gain[i, j, k, m] = 0;
                              }
                         }
                    }
               }
          }

          private static List<NormalizedOutput> NormalizePrices()
          {
               List<float> prices = m_Input.m_Prices;
               List<NormalizedOutput> normalizedOutputList = new List<NormalizedOutput>();
               bool bReach0, bReach10;
               float factor = 2.5f / (0.5f * 0.0018f);
               float Chg = (float)Math.Round(5 / factor, 5);
               float PrevLASTReach = prices[0];
               float NextDown = prices[0] - Chg;
               float NextUp = prices[0] + Chg;
               float LASTReach;    
               float Size;

               
               for (int i = 0; i < prices.Count; i++)
               {
                    bReach0 = prices[i] < NextDown;
                    bReach10 = prices[i] > NextUp;
                    LASTReach = bReach10 ? NextUp : (bReach0 ? NextDown : PrevLASTReach);
                    Size = factor * (prices[i] - (bReach0 || bReach10 ? PrevLASTReach : LASTReach)) + 5;
                    Size = (float)Math.Round(Size, 2);
                    NextDown = LASTReach - Chg;
                    NextUp = LASTReach + Chg;
                    PrevLASTReach = LASTReach;
                    normalizedOutputList.Add(new NormalizedOutput(Size, bReach10, bReach0, i));
               }
               initGain(prices.Count);

               return normalizedOutputList;
          }

          private static eResults Simulation(List<NormalizedOutput> prices, NormalizedOutput priceSample, float Return, float Abort)
          {
               eResults simulationResult = eResults.Unkown;

               for (int i = priceSample.ID + 1; i < prices.Count && simulationResult == eResults.Unkown; i++)
               {
                    if (prices[i].Size >= Return)
                    {
                         simulationResult = eResults.ReturnBeforeAbort;
                    }

                    else if (prices[i].Size <= Abort)
                    {
                         simulationResult = eResults.AbortBeforeReturn;
                    }
               }

               return simulationResult;
          }
          

          private static void TrainExpert(List<NormalizedOutput> prices)
          {
               eResults PrevAbortResult = eResults.Unkown; ;
               eResults PrevReturnResult = eResults.Unkown;;
               eResults Result;
               

               foreach (NormalizedOutput priceSample in prices)
               {
                    float position = priceSample.Size;
                    PrevReturnResult = eResults.Unkown;

                    for (float Return = 10; Return >= position + 0.1; Return -= 0.1f)
                    {
                         PrevAbortResult = eResults.Unkown;

                         for (float Abort = 0; Abort <= position - 0.1; Abort += 0.1f)
                         {
                              Result = eResults.Unkown;
                              if (PrevReturnResult == eResults.ReturnBeforeAbort)
                                   Result = PrevReturnResult;
                              else
                              {
                                   if (PrevAbortResult == eResults.AbortBeforeReturn)
                                        Result = PrevAbortResult;
                                   else
                                   {
                                        Result = Simulation(prices, priceSample, Return, Abort);
                                        PrevReturnResult = Result;
                                        PrevAbortResult = Result;
                                   }                                       
                              }
                              m_Gain[(int)eDirections.PosDirection, (int)position * 10, (int)Return * 10, (int)Abort * 10] += Result == eResults.ReturnBeforeAbort ?
                                   Return : Abort - position - 0.5f;
                              m_Gain[(int)eDirections.NegDirection + 1, (int)position * 10, (int)Abort * 10, (int)Return * 10] += Result == eResults.AbortBeforeReturn ?
                                   Abort : Return - position - 0.5f;
                         }
                    }
               }
          }


          static void Main(string[] args)
          {
               readData("C:/Users/Oren/Desktop/לימודים/Git/DTL/Documents/Design/Price.csv");
               TrainExpert(NormalizePrices());

               // EvaluateExpert()
               // Prediction(Size) -> DIR(1, -1, 0 = do nothing), Return value, Abort value

          }
     }

     class ExpertInput
     {
          public List<float> m_Prices = new List<float>();
     }

     class NormalizedOutput
     {
          public int ID;
          public float Size;
          public bool bEarnedList;
          public bool bAbortedList;

          public NormalizedOutput(float size, bool bEarnedList, bool bAbortedList, int iD)
          {
               Size = size;
               this.bEarnedList = bEarnedList;
               this.bAbortedList = bAbortedList;
               ID = iD;
          }
     }
}
