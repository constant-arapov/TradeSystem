#define _CRT_SECURE_NO_WARNINGS 

#include <stdio.h>
#include <time.h>
#include <string>
#include <list>
#include <ShlObj.h>
#include <tchar.h>

#include "CLogger.h"
#include "utils.h"


using namespace std;




#define MAX_PATH 255
#define MAX_MSG_SIZE 255





 CLogger :: CLogger(string name, bool flushMode)
{


	_flushMode = flushMode;
	_name = name;

	char buff[MAX_PATH];

	//string stDir = GetDirectory ()+"\\";
	::sprintf(buff,"%s\\",GetDirectory().c_str());

	//CreateDirectoryA(stDir.c_str(),NULL);
	//SHCreateDirectory(NULL,stDir.c_str());
	SHCreateDirectoryExA(NULL,buff,NULL);
	


	_path=string(buff)+"\\"+_name+".txt";

	_logFile =  fopen(_path.c_str(),"a");
}

 list<string> dbg;


 string CLogger::GetDirectory ()
 {
    char buff[MAX_PATH];
	char * path = getenv("LOG_PATH");
	


	SYSTEMTIME stm;   
 	GetLocalTime(&stm);

	sprintf(buff,"%sP2ConnectorNative\\%d_%02d_%02d",path, stm.wYear, stm.wMonth, stm.wDay) ;	
	return string(buff);
 }


 void CLogger::Log(string msg)
 {


	 char buff[MAX_MSG_SIZE];
	

	 sprintf(buff,"%s %s\n",::GetStringTime().c_str(), msg.c_str());
	 string fullMsg = buff;
	 fwrite(buff,sizeof(char), fullMsg.length(), _logFile);
	 if (_flushMode)
		fflush(_logFile);

	 //for debug
//	 dbg.push_back(buff);
	 
	 //string st = ::GetStringTime();

/*	 time_t seconds = time (NULL);
	tm* timeinfo = new  tm;
		localtime_s(timeinfo,&seconds);

		char buff[MAX_MSG_SIZE] ;

			asctime_s(buff, MAX_MSG_SIZE, timeinfo);

	*/



 }
void CLogger::LogStartBanner()
{
	Log("\n\n\n");
	Log("=============================================================================================================================");
	Log("=																															 =");
	Log("=																															 =");
	Log("=========================================================== STARTED =========================================================");
	Log("=																															 =");
	Log("=																															 =");
	Log("=============================================================================================================================");


}

void CLogger::LogBanner(string banner)
{
	Log("==========================================================="+ banner +" ==================================================================");

}

void CLogger::Flush()
{
	fflush(_logFile);
}


CLogger:: ~CLogger()
{
	fflush(_logFile);
	fclose(_logFile);

}