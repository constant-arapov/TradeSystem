
#define _CRT_SECURE_NO_WARNINGS 

#include <Windows.h>
//#include <WinNT.h>
#include <string>
#include "cgate.h"

using namespace std;

#define DECIMAL_BASE 10
#define MAX_DATA_SIZE 27

const ULONGLONG  MSEC_IN_100NANOSEC = 10000;
const ULONGLONG  SECONDS_PER_100NS =  10000000;// 100.*1.e+9;
const ULONGLONG  MINUTES_PER_100NS =  60 * SECONDS_PER_100NS;
const ULONGLONG   HOURS_PER_100NS  =  60 * MINUTES_PER_100NS;
const ULONGLONG    DAYS_PER_100NS   =   24 * HOURS_PER_100NS ;


//#define  minutesPer100ns  secondsPer100ns*60;



string GetStringTime()
{

	SYSTEMTIME stm;
   
 	GetLocalTime(&stm);

	
	char buff[MAX_DATA_SIZE];

	//string st ="";
	//_itoa(stm.wYear,buff,DECIMAL_BASE);
	
	sprintf(buff,"[%d.%02d.%02d  %02d:%02d:%02d.%03d]", 
		stm.wYear, stm.wMonth,stm.wDay, stm.wHour, stm.wMinute, stm.wSecond, stm.wMilliseconds);

	string st = buff;

	return st;
}

string CGTImeToString(cg_time_t cgtm)
{
	char buff[MAX_DATA_SIZE];

	//string st ="";
	//_itoa(stm.wYear,buff,DECIMAL_BASE);
	
	sprintf(buff,"%d.%02d.%02d  %02d:%02d:%02d.%03d", 
		cgtm.year, cgtm.month,cgtm.day, cgtm.hour, cgtm.minute, cgtm.second, cgtm.msec);

	string st = buff;

	return st;


}

SYSTEMTIME CGTimeToSystemTime(cg_time_t cgtm)
{
	SYSTEMTIME st;
	st.wYear = cgtm.year;
	st.wMonth = cgtm.month;
	st.wDay = cgtm.day;
	st.wHour = cgtm.hour;
	st.wMinute = cgtm.minute;
	st.wSecond = cgtm.second;
	st.wMilliseconds = cgtm.msec;
	return st;

}



double TimeDifferentServerMS(cg_time_t cgtm)
{

	 SYSTEMTIME stmServer =  CGTimeToSystemTime(cgtm);
//	 stmServer.wHour +=2;
	 SYSTEMTIME stmLocal;
	 GetLocalTime(&stmLocal);

	FILETIME ftLocal,ftServer;

	ULARGE_INTEGER ftAsULargeLocal;
	ULARGE_INTEGER ftAsULargeServer;

		
	SystemTimeToFileTime(&stmLocal, &ftLocal);
	SystemTimeToFileTime(&stmServer, &ftServer);

	memcpy(&ftAsULargeLocal, &ftLocal, sizeof(ftAsULargeLocal));
	memcpy(&ftAsULargeServer, &ftServer, sizeof(ftAsULargeServer));

	//ftAsULargeServer.QuadPart += (double) 2/ HOURS_PER_100NS;
	//ULONGLONG delta = 2* 10000000 * 60 * 60;

//	int t = sizeof (ULONGLONG);

	ftAsULargeServer.QuadPart = ftAsULargeServer.QuadPart+ (ULONGLONG) 2 * HOURS_PER_100NS;
	ULONGLONG res ;
	double dres;

	if (ftAsULargeLocal.QuadPart  >ftAsULargeServer.QuadPart)
	{
		res =  (ftAsULargeLocal.QuadPart - ftAsULargeServer.QuadPart);
		dres = res/MSEC_IN_100NANOSEC ;
	}

	else
	{
		res = (ftAsULargeServer.QuadPart - ftAsULargeLocal.QuadPart);
		dres =  res/MSEC_IN_100NANOSEC ;
		dres = -dres;

	}
	


	//double dres = res/MSEC_IN_100NANOSEC ;

	

	return dres;

	
}



double TimeDiffereceMS(SYSTEMTIME syst1, SYSTEMTIME syst2 )
{
	FILETIME ft1,ft2;

	ULARGE_INTEGER ftAsULargeInt1;
	ULARGE_INTEGER ftAsULargeInt2;

	


	SystemTimeToFileTime(&syst1, &ft1);
	SystemTimeToFileTime(&syst2, &ft2);

	memcpy(&ftAsULargeInt1, &ft1, sizeof(ftAsULargeInt1));
	memcpy(&ftAsULargeInt2, &ft2, sizeof(ftAsULargeInt2));


	ULONGLONG res = ftAsULargeInt2.QuadPart - ftAsULargeInt1.QuadPart;



	//const int MSEC_IN_100NANOSEC = 10000;


	double dres = res/MSEC_IN_100NANOSEC ;



	return dres;

}


