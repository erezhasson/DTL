using DTLExpert.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DTLExpert
{
    class clsJobLoadData
    {
        List<RawMT4Data> RawMT4DataList = new List<RawMT4Data>();

        public void Go()
        {
            LoadData("D:\\Projects\\DTL\\TempProjects\\TrainQA\\MT4DTLData");
            CalcSize();
            printSizeDataFull();
            printSizeDataShort();
        }
        private void LoadData(string DataPath)
        {
            var fileEntries = new DirectoryInfo(DataPath).GetFiles()
                              .OrderBy(f => f.FullName)
                              .ToList();

            foreach (FileInfo _FileInfo in fileEntries)
                ReadFile(_FileInfo.FullName, _FileInfo.Name.Replace(".dat", "")) ;

        }
        private void ReadFile(string FilewithPathName,string FileName)
        {
            string[] lines = File.ReadAllLines(FilewithPathName);
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
                string time = aValues[0];
                int Hour = int.Parse(time.Substring(0, 2));
                int min = int.Parse(time.Substring(3, 2));
                int sec = int.Parse(time.Substring(6, 2));
                double Bid = double.Parse(aValues[1]);
                double Ask = double.Parse(aValues[2]);
                RawMT4DataList.Add(new RawMT4Data(FileName,Hour, min, sec, Bid, Ask,-9));


            }

        }
        private void CalcSize()
        {
            double LastReach = -9, NextDown=-9, NextUp=-9;
            DateTime Prevdate= new DateTime();
            double LastBid = -9;
 
            for (int RawMT4DataListPos = 0; RawMT4DataListPos < RawMT4DataList.Count; RawMT4DataListPos++)
            { 
                RawMT4Data _RawMT4Data = RawMT4DataList[RawMT4DataListPos];
                double NewSize = -9;

                double Bid = _RawMT4Data.Bid;
                string strDate = _RawMT4Data.yyyymmdd;
                DateTime date = new DateTime(int.Parse(strDate.Substring(0, 4)), int.Parse(strDate.Substring(4, 2)), int.Parse(strDate.Substring(6, 2)),
                                    _RawMT4Data.Hour, _RawMT4Data.min, _RawMT4Data.sec);
 
                if (RawMT4DataListPos == 0) //FirstRow
                {
                    NewSize = 50;
                    RawMT4DataList[RawMT4DataListPos].SetSize(NewSize);
                    LastReach = Bid;
                }
                else
                {
                     System.TimeSpan diff1 = date.Subtract(Prevdate);
                    if (diff1.Seconds +60* diff1.Minutes > 300 && Math.Abs(Bid- LastBid)>0.00018) //Gap in time
                    {
                        NewSize = 50;
                        RawMT4DataList[RawMT4DataListPos - 1].Size = -9;
                        LastReach = Bid;
                    }
                    else
                    {
                        NewSize = 27777.77778 * (Bid - LastReach) + 50;
                        if (Bid < NextDown) LastReach = NextDown;

                        if (Bid > NextUp) LastReach = NextUp;
                    }

                }
                RawMT4DataList[RawMT4DataListPos].SetSize(NewSize);
                Prevdate = date;
                LastBid = Bid;

                NextDown = LastReach - 0.0018;
                NextUp = LastReach + 0.0018;


            }
   



        }

 
        private void printSizeDataFull()
        {


            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeDataFull.csv";
            double LastBid = -9,LastAsk=-9;
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", "Date", "Time", "Bid", "Ask",
                    "Size"));


                foreach (RawMT4Data _RawMT4Data in RawMT4DataList)
                {

                    if (_RawMT4Data.Bid != LastBid || _RawMT4Data.Ask != LastAsk)
                    {
                        LastBid = _RawMT4Data.Bid;
                        LastAsk = _RawMT4Data.Ask;
                        stream.WriteLine(string.Format("{0},{1}:{2}:{3},{4},{5},{6},",
                        _RawMT4Data.yyyymmdd,
                        _RawMT4Data.Hour, _RawMT4Data.min, _RawMT4Data.sec,
                        _RawMT4Data.Bid, _RawMT4Data.Ask, _RawMT4Data.Size
                    ));

                    }
                }
            }
        }
        private void printSizeDataShort()
        {


            var file = @"D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeDataShort.csv";
            double LastBid = -9, LastSize = -9;
            using (var stream = File.CreateText(file))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4}", "Date", "Time", "Bid", "Ask",
                    "Size"));

                foreach (RawMT4Data _RawMT4Data in RawMT4DataList)
                {

                    if (_RawMT4Data.Bid != LastBid || _RawMT4Data.Size != LastSize)
                    {
                        string strDate = _RawMT4Data.yyyymmdd;
     
                        LastBid = _RawMT4Data.Bid;
                        LastSize = _RawMT4Data.Size;
                        stream.WriteLine(string.Format("{0},{1}:{2}:{3},{4},{5},{6},",
                        strDate,
                        _RawMT4Data.Hour, _RawMT4Data.min, _RawMT4Data.sec,
                        _RawMT4Data.Bid, _RawMT4Data.Ask, _RawMT4Data.Size
                    ));

                    }
                }
            }
        }
    }
}
