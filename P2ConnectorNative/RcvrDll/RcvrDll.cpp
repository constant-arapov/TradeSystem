// RecvrCpp.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"

#include <stdio.h>
#include <conio.h>
#include <tchar.h>
#include <list>
#include <Windows.h>

#include <map>
#include "CStockReader.h"
#include "Clocker.h"


#pragma comment(lib, "user32.lib")



#define PAGESIZE 4096
#define BUF_SIZE 3*4096

#define INSTRUMENTS_NAME_BUFF_SIZE 255
#define BUFFER_INSTRUMENTS_SIZE 4096


using namespace std;


//TCHAR szName[]= TEXT("Global\\MyFileMappingObject");

TCHAR szName[]= TEXT("Global\\ATFS_Stock_430354");

map<long, CStockReader *> _gListStockReaders;
CRITICAL_SECTION g_critListStock;
Clocker _lckStocks;


typedef struct
{
	SYSTEMTIME Snd;
	SYSTEMTIME Rcv;

} TM;



HANDLE hMapFile;
LPCTSTR pBuf;
void * pInstrBuff;

HANDLE g_InstrumentLoaded;



extern "C" __declspec(dllexport) int    Init();
extern "C" __declspec(dllexport) void   InitStock(long isinId);

extern "C" __declspec(dllexport) void   ReadData(char ** data);
extern "C" __declspec(dllexport) void  ReadStock(long isinId, char ** data);
extern "C" __declspec(dllexport) void  CloseStock(long isinId);
extern "C" __declspec(dllexport) void   Close();

extern "C" __declspec(dllexport) int AddInstruments(void * data, long num);



///



CStockReader* GetStockReader(long isinId)
{
	_lckStocks.Lock();
		CStockReader *stockReader = _gListStockReaders[isinId];
	_lckStocks.Unlock();
	
	return stockReader;
}



void InitStock(long isinId)
{
	CStockReader * stockReader = new CStockReader ();
	stockReader->Init(isinId);

	//EnterCriticalSection(&g_critListStock);
	_lckStocks.Lock();
		_gListStockReaders[isinId] = stockReader;
	_lckStocks.Unlock();

	//LeaveCriticalSection(&g_critListStock);

}

void CloseStock(long isinId)
{
	CStockReader * stockReader = GetStockReader(isinId);
	stockReader->Close();
}







void ReadStock(long isinId,char ** data)
{

	CStockReader * stockReader = GetStockReader(isinId);
	stockReader->ReadData(data);

}







int   Init()
{


	//InitStockReader(430354);

	


   hMapFile = OpenFileMapping(
                   FILE_MAP_ALL_ACCESS,   // read/write access
                   FALSE,                 // do not inherit the name
                   szName);               // name of mapping object

   if (hMapFile == NULL)
   {
      _tprintf(TEXT("Could not open file mapping object (%d).\n"),
             GetLastError());
      return 1;
   }

   pBuf = (LPTSTR) MapViewOfFile(hMapFile, // handle to map object
               FILE_MAP_ALL_ACCESS,  // read/write permission
               0,
               0,
               BUF_SIZE);

   if (pBuf == NULL)
   {
      _tprintf(TEXT("Could not map view of file (%d).\n"),
             GetLastError());

      CloseHandle(hMapFile);

      return 1;
   }

}










void Close()
{

  UnmapViewOfFile(pBuf);
  CloseHandle(hMapFile);


}





//char buffer[10] = "123";


void ReadData(char **data)
{

     SYSTEMTIME * stm;
   TM SndRcv;
     //data = (void**)  &pBuf;
   //*data = buffer;

   *data  = (char *) pBuf;

  // std::list<TM> _lst;

   /*  stm  = (SYSTEMTIME *)  pBuf;
	  SndRcv.Snd = *stm;


	  SYSTEMTIME stmLocal;
	  GetLocalTime(&stmLocal);
	  SndRcv.Rcv = stmLocal;
	  */
}

int AddInstruments(void * data, long num)
{

	//char bufferInstrument [BUFFER_INSTRUMENTS_SIZE];

	TCHAR bufferName[INSTRUMENTS_NAME_BUFF_SIZE];
	swprintf(bufferName,BUFFER_INSTRUMENTS_SIZE,L"Global\\ATFS_Instruments");




	  hMapFile = OpenFileMapping(
                   FILE_MAP_ALL_ACCESS,   // read/write access
                   FALSE,                 // do not inherit the name
                   bufferName);               // name of mapping object


	  if (hMapFile == NULL)
		  return 1;


	   pInstrBuff = MapViewOfFile(hMapFile, // handle to map object
               FILE_MAP_ALL_ACCESS,  // read/write permission
               0,
               0,
               PAGESIZE);

	    if (hMapFile == NULL)
		  return 1;


		memcpy((void*)pInstrBuff, data, num* sizeof(__int64));

		 g_InstrumentLoaded  =  OpenEventW(EVENT_ALL_ACCESS,true,TEXT("ATFS_Instument_loaded"));
		 SetEvent(g_InstrumentLoaded );



	//	g_InstrumentLoaded = CreateEvent(NULL , false,false,  TEXT("ATFS_Instument_loaded"));
		

	/*
	for (int i=0; i < num; i++)
	{
		//long * isinID = (long*) data ;
		//*((long*) bufferInstrument+i) = 
	

	}
	*/
		 return 0;
}




int _tmain(int argc, _TCHAR* argv[])
{



  
	Init();

   while (true)
   {

		//ReadData();

   }



   //   MessageBox(NULL, pBuf, TEXT("Process2"), MB_OK);






	return 0;
}


