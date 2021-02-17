double dlastPrice=-9 ;
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void OnTick(void)
{
 

   

    SendPriceData() ;
  

  
     return ;
 

  
 }
 
 void SendPriceData()
 {
 
    double dPrice=(Bid+Ask) / 2 ;
    if (dPrice==dlastPrice)
      return ;
     dlastPrice= dPrice;
    //Delete old price files
    string sFileToRemove ;
    long search_handle=FileFindFirst("DTL_FromMT4_Price_*.txt",sFileToRemove);
//--- check if the FileFindFirst() is executed successfully
   if(search_handle!=INVALID_HANDLE)
   {
      do
      {
          FileDelete (sFileToRemove) ;
      }
      while(FileFindNext(search_handle,sFileToRemove));
      //--- close the search handle
      FileFindClose(search_handle);
   }
 
    
  // Create new file  
    
    string filename="DTL_FromMT4_Price_"+Bid+"_"+Ask+".txt" ;
    int fileHandle     =    FileOpen(filename , FILE_READ|FILE_WRITE|FILE_TXT);
    FileClose(fileHandle); 
    
 }
 
  

 
