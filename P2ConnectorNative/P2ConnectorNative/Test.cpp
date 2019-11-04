#include <fstream>

#include <sstream>
#include <iostream>
#include <algorithm>
#include <iterator>

#include <string>

#include "Clogger.h"
#include "CNamedPipeClient.h"
#include "CStopWatch.h"

#include "CAlarmer.h"
#include "CP2ConnectorNative.h"

#include "MockMapperGlobals.h"
#include "MockAlarmer.h"
#include "MockSynchro.h"
#include "MockP2ConnectorNative.h"

#include "CMapperStock.h"



#include "aggr.h"

using namespace std;


extern CP2ConnectorNative * g_p2ConnectorNative;
extern CAlarmer * g_alarmer;
extern CMapperGlobals * g_pMapperGlobals;




MockMapperGlobals * g_pMockMapperGloblas;
MockSynchro * g_pMockSynchro;
MockP2ConnectorNative * g_pMockP2ConnectorNative;


void TestLogger()
{
	CLogger * testLogger;
	testLogger = new CLogger("testLogger", true);
	//testLogger->Write("123");

}

void TestNamedPipe()
{
	CNamedPipeClient * namedPipeClient = new CNamedPipeClient(TEXT("\\\\.\\pipe\\myNamedPipe1"),
															  TEXT("\\\\.\\pipe\\myNamedPipe2"));
	namedPipeClient->Process();
	namedPipeClient->WriteMessage("alive");
}

void TestAlarmer()
{
	 //g_alarmer->Error("123");
	 //g_alarmer->Error("456");
	//Error("123");
	//Error("456");

}

void TestMapperGlobals()
{
	MockMapperGlobals  mapperGlobals;
	mapperGlobals.TestMapperGlobals();


}


void TestMain()
{
		g_alarmer = new MockAlarmer(new CNamedPipeClient(TEXT(""),TEXT("")));
	
		g_pMockSynchro = new MockSynchro();

		g_pMockMapperGloblas = new MockMapperGlobals();




		//g_pMapperGlobals = new CMapperGlobals();
	    g_pMockMapperGloblas = new MockMapperGlobals();
		g_pMockMapperGloblas->CreateGlobals();
		g_pMockMapperGloblas->SetInstruments();
	 
	  //::Sleep(100);

		g_pMockSynchro->SetInstrumentLoaded();


	  g_p2ConnectorNative = new CP2ConnectorNative(g_pMockMapperGloblas, g_pMockSynchro);
	  

	  g_p2ConnectorNative->Process();

	  getchar();

}


void TestSleepInterval()
{
	
	

	CLogger _lg("TestSleepInterval", true);
	

	while(true)
	{
		_lg.Log("test");
		Sleep(1);

	}

}


void TestMapperStock()
{
	CMapperStock mapperStock(/*538712*/430381);
	mapperStock.Init();


	char _bufferWork[STOCK_BUFFER_SIZE];


	 //TODO for debugging, not use in production
	 memset(_bufferWork,0, STOCK_BUFFER_SIZE);
	// memcpy(localBuffer,bufferWork,STOCK_BUFFER_SIZE);
	//copy data to workBuffer
	
	  
	  int i =0 ;
	  int currSize = 0;

	

	  int iBid =0;
	  int iAsk =0;
	  


	  char cntr = 0;
																			
	  while (true)
	  {
		  i=0;
			do 
			{

		  		  		  		  		 		  			
						 //0123456701
				         //1234567890
			char buff[11];


			cntr == 254 ? 0 : cntr++;

			buff[0] = 0;
			buff[1] = 0;
			buff[2] = 0;
			buff[3] = 0;
			buff[4] = 0;
			buff[5] = 1; //10000
			buff[6] = 0; //100
			buff[7] = cntr; //1
			buff[8] = 0;
			buff[9] = 0;
			buff[10] = 0;



			  //copy to bid area
			((orders_aggr* )_bufferWork + i)->dir = 1;
			((orders_aggr* )_bufferWork + i)->replID =1;
			memcpy(((orders_aggr* )_bufferWork + i)->price,buff,11);
			((orders_aggr* )_bufferWork + i)->volume = 10;
			 
		 
		 


			currSize = ++i * sizeof_orders_aggr;

			


			}
	   while (currSize < STOCK_BUFFER_SIZE && i<100 );


	   mapperStock.Write(_bufferWork);
	  //Sleep(1);
	  }


}

void InitCgateVars()
{
				
				char buff2[255];									
					
					char * confPath = getenv("CONFIG_PATH");
	if (confPath == NULL)
	{
		Error("Env CONFIG_PATH not found");
		return;
	}

	
	sprintf(buff2,"ini=%s\\P2ConnectorNative\\repl.ini;key=11111111",
					confPath);
	
	
	CG_RESULT res  = cg_env_open(buff2);
	if (res != CG_ERR_OK)
	{
		Error("Unable to open cgate invironment");
	}

}
char buff[11]={0};	

double GetBCDPrice(string val)
{

						  
					             // 0123456
			//		string inpVal = "14438.0000";
				//	string inpVal = "226075.0000";
					string inpVal =val;
					
					memset(buff,0,11);

					buff[1] = 16;

					int nDelim =  inpVal.find('.');
					string intPart = inpVal.substr(0,nDelim);

					buff[0] = 5;// intPart.size();

					if ( intPart.size() % 2 == 1)
						intPart = "0" + intPart;

					int i=intPart.size();
					int j=7;

					while (i>0)
					{
						
						 string currNum = intPart.substr( i-2,2);
						 buff[j--] = stoi(currNum);
						 i = i-2;

		
					}
					
				

					/*
					buff[0] = 5;
					buff[1] = 16;
					buff[2] = 0;
					buff[3] = 0;   
					buff[4] = 0;
					buff[5] = 1; //100000 1*100^2
					buff[6] = 44; //10000   44*100^1
					buff[7] = 38; //1  100  38*100^0
					buff[8] = 0;
					buff[9] = 0;
					buff[10] = 0;
					*/
					//this is just fo check do remove in the future
				    int64_t value=0;
					int8_t scale=0;

					int resCG = cg_bcd_get(buff,&value, &scale);
			
					double dblPrice= (double) value/pow(10.0, scale);
					
					return dblPrice;

}



void  Simulate()
{
		InitCgateVars();

		
		g_alarmer = new MockAlarmer(new CNamedPipeClient(TEXT(""),TEXT("")));
	
		g_pMockSynchro = new MockSynchro();

		g_pMockMapperGloblas = new MockMapperGlobals();




		//g_pMapperGlobals = new CMapperGlobals();
	    g_pMockMapperGloblas = new MockMapperGlobals();
		g_pMockMapperGloblas->CreateGlobals();
		g_pMockMapperGloblas->SetInstruments();
	 
	  //::Sleep(100);

		g_pMockSynchro->SetInstrumentLoaded();


	 // g_p2ConnectorNative = new CP2ConnectorNative(g_pMockMapperGloblas, g_pMockSynchro);
		g_pMockP2ConnectorNative = new MockP2ConnectorNative(g_pMockMapperGloblas, g_pMockSynchro);
		g_pMockP2ConnectorNative->InitSimulate();
		g_pMockP2ConnectorNative->Log("================================================== SUMULATION STARTED =================================================================================");
	//  g_p2ConnectorNative->Process();

	 
	  


		::CStopwatch sw;



	//ifstream infile("d:\\temp91\\ConnectorStockAggr.txt");
	//ifstream infile("e:\\temp99\\ConnectorStockAggr.txt");
     ifstream infile("C:\\temp2\\ConnectorStockAggr.txt");
	//ifstream infile("c:\\temp2\\end_sess.txt");
		

	//ifstream infile("d:\\temp92\\ConnectorStockAggr_short.txt");
		

     // ifstream infile("C:\\temp2\\ConnectorStockAggr_short.txt");
	//e:\temp99\


	string line;

	while (getline(infile, line))
	{
		
	
	    istringstream iss(line);

		if (line.size() == 0)
			continue;
	
		if (line[0]==' ')
			continue;

		if (line[0]!='[') 
			continue;

		if (line.find("BEGIN")!= string::npos)
		{
			g_pMockP2ConnectorNative->ProcessBegin();
		}
		else if (line.find("COMMIT")!= string::npos)
		{
			g_pMockP2ConnectorNative->ProcessCommit();
		}
		else if (line.find("ONLINE")!= string::npos)
		{
			g_pMockP2ConnectorNative->ProcessOnline();
		}

		else if (line.find("replID")!= string::npos )
		{
			 istringstream iss(line);
			 vector<string> tokens;

		copy(istream_iterator<string>(iss),
			istream_iterator<string>(),
			back_inserter(tokens));


		string::size_type sz;
		long replId=0;
		long replRev=0;

		double price =0;
		long isin_id=0;
		int dir=1;
		long volume=0;
		
	
		for (vector<string>::iterator it= tokens.begin(), end=tokens.end();
			 it!=end; it++)
			{
				string el = *it;
				if (el.find("[")!= string::npos)
					continue;

				if (el.find("]")!= string::npos)
					continue;				

				int pos = el.find("=");
				string key= el.substr(0,pos);
				string val = el.substr(pos+1,el.size());
							
				if (key == "replID")				
					replId= stoi(val,&sz);
				else if (key == "replRev")				
					replRev= stoi(val,&sz);
				else if (key == "price")				
					price = GetBCDPrice(val);				
				else if (key == "isin_id")				
					isin_id = stol(val, &sz);				
				else if (key == "volume")				
					volume = stol(val,&sz);
				else if (key == "dir")
					dir = stoi(val,&sz);
				

			}
		
		orders_aggr ordersAggr;

		ordersAggr.replID = replId;
		memcpy(ordersAggr.price, buff,11);
		ordersAggr.isin_id = isin_id;
		ordersAggr.volume = volume;
		ordersAggr.dir = dir;
		ordersAggr.replRev = replRev;
		g_pMockP2ConnectorNative->ProcessOrdersAggr(&ordersAggr);
		
		
		}



	}
  getchar();
}


void Test()
{

	Simulate();


//	TestMapperStock();

//	TestSleepInterval();

	//TestMain();

//	TestMapperGlobals();

}