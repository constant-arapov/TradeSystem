

//#include <string>
#include <Windows.h>

#include "CAlarmer.h"

#include "CStockProcessor.h"

#include "CMapperStock.h"

using namespace std;

#define MEMFILE_BUFF_SIZE 255

CMapperStock::CMapperStock(long isinId)
{

	_isinId = isinId;


}




void CMapperStock::Init()
{

	
	TCHAR bufferName[MEMFILE_BUFF_SIZE];

	swprintf(bufferName,MEMFILE_BUFF_SIZE,L"Global\\ATFS_Stock_%d",_isinId);
	//swprintf(szName, "%d",1);


	 _hMapFile = CreateFileMappingW(
                 INVALID_HANDLE_VALUE,    // use paging file
                 NULL,                    // default security
                 PAGE_READWRITE,          // read/write access
                 0,                       // maximum object size (high-order DWORD)
                 STOCK_BUFFER_SIZE,       // maximum object size (low-order DWORD)
				 bufferName);             // name of mapping object

	 if (_hMapFile == NULL)
	 {
		Error("CMapperStock::Init. Unable create file mapping");
		return;
	 }




	  pBuffer = (LPTSTR) MapViewOfFile(_hMapFile,   // handle to map object
                        FILE_MAP_ALL_ACCESS, // read/write permission
                        0,
                        0,
                       STOCK_BUFFER_SIZE);		


	  if (pBuffer == NULL)
	  {
		 Error("CMapperStock::Init. Unable map view of file");
		 return;
	  }



}


void CMapperStock::Write(void * pntr)
{

	memcpy(pBuffer,pntr,STOCK_BUFFER_SIZE);


}






