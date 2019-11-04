#include <Windows.h>
#include <stdio.h>
#include <list>

#include "CAlarmer.h"
#include "CMapperGlobals.h"



#define INSTRUMENTS_NAME_BUFF_SIZE 255
#define BUFFER_INSTRUMENTS_SIZE 4096

using namespace std;


void CMapperGlobals::CreateGlobals()
{
	TCHAR bufferName[INSTRUMENTS_NAME_BUFF_SIZE];

	swprintf(bufferName,BUFFER_INSTRUMENTS_SIZE,L"Global\\ATFS_Instruments");
	//swprintf(szName, "%d",1);


	 _hMapFileInstruments = CreateFileMappingW(
                 INVALID_HANDLE_VALUE,    // use paging file
                 NULL,                    // default security
                 PAGE_READWRITE,          // read/write access
                 0,                       // maximum object size (high-order DWORD)
                 BUFFER_INSTRUMENTS_SIZE, // maximum object size (low-order DWORD)
				 bufferName);             // name of mapping object


	 if (_hMapFileInstruments  == NULL)
	 {
		 Error("CMapperGlobals::CreateGlobals. Unable create file mapping");
	 }

	   pBufInstruments = MapViewOfFile(_hMapFileInstruments, // handle to map object
               FILE_MAP_ALL_ACCESS,  // read/write permission
               0,
               0,
               BUFFER_INSTRUMENTS_SIZE);

	   

	if (pBufInstruments == NULL)
	{
		 Error("CMapperGlobals::CreateGlobals. Unable create view of file");
	}

}


list<long> CMapperGlobals::GetInstrumentList()
{


	list<long> outListInstr;
	int i =0;
    long value = -1;
	while ((value =  *((_int64*)  pBufInstruments + i++)) != 0)
		outListInstr.push_back(value);
		 
	
	


	

	return outListInstr;
}



