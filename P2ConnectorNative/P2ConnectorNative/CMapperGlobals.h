#ifndef _CMAPPER_GLOBALS_
#define _CMAPPER_GLOBALS_

#include <Windows.h>
#include <list>

using namespace std;


class CMapperGlobals
{
private:
	HANDLE _hMapFileInstruments ;

protected:
	void * pBufInstruments;
	

public:
	void CreateGlobals();
	list<long> GetInstrumentList();

};

#endif