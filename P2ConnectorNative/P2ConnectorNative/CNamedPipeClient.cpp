#include "CNamedPipeClient.h"

#include <stdio.h>

#include <Windows.h>
#include "ThreadingCreators.h"


 
CNamedPipeClient ::CNamedPipeClient(LPTSTR lpszPipeWriter, LPTSTR lpszPipeReader)
{
	_lpszPipeWriter = lpszPipeWriter;
	_lpszPipeReader = lpszPipeReader;

	_evForceQueueMessages = CreateEvent(NULL,false, false, NULL);


	_hPipeWriter = CreateFile(_lpszPipeWriter,	GENERIC_WRITE ,0,NULL,OPEN_EXISTING,FILE_FLAG_OVERLAPPED,NULL);
	_hPipeReader = CreateFile(_lpszPipeReader,	GENERIC_READ ,0,NULL,OPEN_EXISTING,FILE_FLAG_OVERLAPPED,NULL);


	 if ((_hPipeWriter == NULL || _hPipeWriter == INVALID_HANDLE_VALUE)||(_hPipeReader == NULL || _hPipeReader== INVALID_HANDLE_VALUE))
    { 
        printf("Could not open the pipe  - (error %d)\n",GetLastError());
        
    }
	 else
	 {
	
		// hThread = CreateThread( NULL, 0, &NET_RvThr, NULL, 0, NULL);
		 StartThread(this, StartCNamedPipeClientThreadReader);
		 StartThread(this, StartCNamedPipeClientThreadWriter);
			
	 }


}


void CNamedPipeClient :: Process()
{
	/*
    
	//_lpszPipeWriter = TEXT("\\\\.\\pipe\\myNamedPipe1"); 
	//_lpszPipeReader = TEXT("\\\\.\\pipe\\myNamedPipe2"); 

	

	//Thread Init Data

	

	

	Finished=FALSE;




	_hPipeWriter = CreateFile(_lpszPipeWriter,	GENERIC_WRITE ,0,NULL,OPEN_EXISTING,FILE_FLAG_OVERLAPPED,NULL);
	_hPipeReader = CreateFile(_lpszPipeReader,	GENERIC_READ ,0,NULL,OPEN_EXISTING,FILE_FLAG_OVERLAPPED,NULL);


	 if ((_hPipeWriter == NULL || _hPipeWriter == INVALID_HANDLE_VALUE)||(_hPipeReader == NULL || _hPipeReader== INVALID_HANDLE_VALUE))
    { 
        printf("Could not open the pipe  - (error %d)\n",GetLastError());
        
    }
	 else
	 {
	
		// hThread = CreateThread( NULL, 0, &NET_RvThr, NULL, 0, NULL);
		 StartThread(this, StartCNamedPipeClientThreadReader);
		 StartThread(this, StartCNamedPipeClientThreadWriter);

		
		
		Finished=TRUE;
	 }

	// getchar();

	*/
}


void CNamedPipeClient :: ThreadWriter()
{

	//Pipe Init Data
	char buf[MAX_PATH];

	DWORD cbWritten;
	DWORD dwBytesToWrite = (DWORD)strlen(buf);
	size_t sz=0;
	DWORD msWait =10;

	while (true)
     {

		 if (sz<=1)
			 WaitForSingleObject(_evForceQueueMessages,msWait);
		 
		 _lckQueueToWrite.Lock();
		 sz = this->_queueToWrite.size();

			
		 


			// printf ("Enter your message: ");
			 //scanf ("%s",buf);  
		    if (sz!=0)
			{
				string message =_queueToWrite.front();
				_queueToWrite.pop();
				memcpy(buf,message.c_str(),message.length());	
				//buf[message.length()] = '\0';
				WriteFile(_hPipeWriter, buf, dwBytesToWrite, &cbWritten, NULL);
				memset(buf,0xCC,MAX_PATH);
			}	
			 
			_lckQueueToWrite.Unlock();

		Sleep(100);	
	  }

		CloseHandle(_hPipeWriter);

}


void CNamedPipeClient :: WriteMessage(string message)
{
	_lckQueueToWrite.Lock();
		_queueToWrite.push(message);
	_lckQueueToWrite.Unlock();

	SetEvent(_evForceQueueMessages);


}



void CNamedPipeClient :: ThreadReader() 
{
	BOOL fSuccess; 
	char chBuf[100];
	DWORD dwBytesToWrite = (DWORD)strlen(chBuf);
	DWORD cbRead;
	int i;

	//temporary disabled as we don't need it

	/*
	while(1)
	{
		fSuccess =ReadFile( _hPipeReader,chBuf,dwBytesToWrite,&cbRead, NULL); 
		if(fSuccess)
		{
			printf("C++ App: Received %d Bytes : ",cbRead);
			for(i=0;i<cbRead;i++)
				printf("%c",chBuf[i]);
			printf("\n");
		}
		if (! fSuccess && GetLastError() != ERROR_MORE_DATA) 
		{
			printf("Can't Read\n");
			if(Finished)
				break;
		}
	}
	
	CloseHandle(_hPipeReader);
	*/
}




