// P2ConnectorNative.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
//#include <io.h>
//#include <fcntl.h>
#include <Windows.h>

#include <list>

#include "utils.h"

#include "Test.h"


#include "CP2ConnectorNative.h"
#include "CSynchro.h"

#include "CAlarmer.h"



using namespace std;




CP2ConnectorNative * g_p2ConnectorNative;
CAlarmer * g_alarmer;
CMapperGlobals * g_pMapperGlobals;
CSynchro * g_pSynchro;








int _tmain(int argc, _TCHAR* argv[])
{
	
	 SetConsoleOutputCP(1251);
	

	//getchar();
    //Test();
	

	//return 1;




	  g_alarmer = CAlarmer::Create();	
	  
	  g_pSynchro = new CSynchro();
	  
	  //TODO normal synch
	  ::Sleep(100);
	  g_pMapperGlobals = new CMapperGlobals();
	  g_pMapperGlobals->CreateGlobals();

	  g_pSynchro = new CSynchro();
	  g_p2ConnectorNative = new CP2ConnectorNative(g_pMapperGlobals, g_pSynchro);





	//g_p2ConnectorNative->DBGSetInstrumentLoaded();
	g_p2ConnectorNative->Process();




	getchar();
	//::Sleep(10000000);

	return 0;
}




