using System;
using System.Collections.Generic;

namespace DTLML
{
     class DTLGameTree
     {
          private static Dictionary<string, GameNode> GameNodes = new Dictionary<string, GameNode>();
          private static GameNode start = new GameNode("Start"), abort = new GameNode("Abort");
          

          private static void buildGameTree()
          {
               for (int OS = 0; OS < 11; OS++) // Build PS Nodes
               {
                    GameNode OSNode = new GameNode("OS" + OS.ToString());
                    GameNodes[OSNode.ID] = OSNode;
                    start.Neighbor.Add(OSNode); // Build arches start -> PSNODE

                    for (int DIR = -1; DIR < 2; DIR += 2)
                    {
                         GameNode PSNode = new GameNode(OSNode.ID + "_PS" + DIR.ToString());
                         GameNodes[PSNode.ID] = PSNode;
                         OSNode.Neighbor.Add(PSNode); // Build arches PSNODE -> ALL DIRNODES

                         for (int ES = 0; ES < 11; ES++)
                         {     
                              GameNode ESNode = new GameNode(PSNode.ID + "_ES" + ES.ToString());
                              GameNodes[ESNode.ID] = ESNode;

                              if (OS != ES)
                              {
                                   PSNode.Neighbor.Add(ESNode); // Build arches DIRNODE -> ESNODE
                              }
                         }

                         for (int ES = 0; ES < 11; ES++)
                         {
                              GameNode ESNode = GameNodes[PSNode.ID + "_ES" + ES.ToString()];

                              for (int neighbor = 0; neighbor < 11; neighbor++)
                              {
                                   if (neighbor != ES && ES != 10 && ES != 0)
                                   {
                                        ESNode.Neighbor.Add(GameNodes[PSNode.ID + "_ES" + neighbor.ToString()]);
                                   }
                              }

                              ESNode.Neighbor.Add(abort);
                         }
                    }
               }

               foreach (KeyValuePair<string, GameNode> entry in GameNodes)
               {
                    Console.WriteLine(entry.Key + " -> ");
                    entry.Value.printNeighbor();
                    Console.WriteLine("");
                    Console.WriteLine("");
               }
          }

          public static void Main()
          {
               buildGameTree();
          }
     }

     class GameNode
     {
          public string ID { get; set; }
          public List<GameNode> Neighbor = new List<GameNode>();

          public GameNode(string i_ID)
          {
               ID = i_ID;
          }

          public void printNeighbor()
          {
               for (int i = 0; i < Neighbor.Count; i++)
               {
                    Console.Write(Neighbor[i].ID + ", ");
               }
          }
     }
}
