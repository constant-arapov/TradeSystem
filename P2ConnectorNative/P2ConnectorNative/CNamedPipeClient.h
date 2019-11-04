#ifndef _NAMED_PIPE_CLIENT_
#define _NAMED_PIPE_CLIENT_

#include <Windows.h>
#include <string>
#include <queue>


#include "CLocker.h"

using namespace std;


class CNamedPipeClient
{

private:
	DWORD WINAPI ThreadProc() ;
	HANDLE _hPipeWriter,_hPipeReader; 
	BOOL Finished;

	LPTSTR _lpszPipeWriter;
	LPTSTR _lpszPipeReader;
	
	Clocker _lckQueueToWrite;
	queue<string>  _queueToWrite;
	HANDLE _evForceQueueMessages;


public:

	CNamedPipeClient(LPTSTR lpszPipeWriter, LPTSTR lpszPipeReader);

	void Process();
	void ThreadReader();
	void ThreadWriter();
	
	void WriteMessage(string);

	




};


#endif