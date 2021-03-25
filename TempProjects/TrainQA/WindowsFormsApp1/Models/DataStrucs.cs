namespace DTLExpert.Models
{
    public class  RawMT4Data
    {
        public string yyyymmdd;
        public int Hour;
        public int min;
        public int sec ;
        public double Bid;
        public double Ask;
        public double Size;
        public RawMT4Data (string inyyyymmdd,int inHour,int inmin,int insec,
                double inBid, double inAsk,double inSize)
        {
            yyyymmdd = inyyyymmdd;
            Hour = inHour;
            min = inmin;
            sec = insec;
            Bid = inBid;
            Ask = inAsk;
            Size = inSize;

        }
        public void SetSize(double inSize)
        {
            this.Size = inSize;
        }
    }
}