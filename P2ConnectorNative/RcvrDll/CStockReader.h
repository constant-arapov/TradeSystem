#ifndef _CSTOCK_READER_
#define _CSTOCK_READER_

#include <Windows.h>


class CStockReader
{

private: 

	HANDLE _hMapFile;
	LPCTSTR _pBuf;


public:
	int Init(long isinId);
	void ReadData(char **data);
	void Close();
};

#endif