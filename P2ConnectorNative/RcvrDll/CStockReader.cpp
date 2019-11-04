#include "stdafx.h"
#include <Windows.h>


#include "CStockReader.h"

//TODO from global header
#define BUF_SIZE 3*4096
#define INSTRUMENTS_NAME_BUFF_SIZE 255

int CStockReader::Init(long isinId)
{

TCHAR bufferName[INSTRUMENTS_NAME_BUFF_SIZE];
//TODO get from global library
swprintf(bufferName,INSTRUMENTS_NAME_BUFF_SIZE ,L"Global\\ATFS_Stock_%d",isinId);

//TCHAR szName[]= TEXT("Global\\ATFS_Stock_430354");

_hMapFile = OpenFileMapping(
                   FILE_MAP_ALL_ACCESS,   // read/write access
                   FALSE,                 // do not inherit the name
                   bufferName);               // name of mapping object

   if (_hMapFile == NULL)
   {

	   //TODO out error
      //_tprintf(TEXT("Could not open file mapping object (%d).\n"),
        //     GetLastError());
      return 1;
   }

   _pBuf = (LPTSTR) MapViewOfFile(_hMapFile, // handle to map object
               FILE_MAP_ALL_ACCESS,  // read/write permission
               0,
               0,
               BUF_SIZE);

   if (_pBuf == NULL)
   {
	   //TODO out error
      //_tprintf(TEXT("Could not map view of file (%d).\n"),
        //     GetLastError());

      CloseHandle(_hMapFile);

      return 1;
   }

   return 0;
}


void CStockReader::ReadData(char **data)
{
	*data  = (char *) _pBuf;

}


void CStockReader::Close()
{
  UnmapViewOfFile(_pBuf);
  CloseHandle(_hMapFile);


}
