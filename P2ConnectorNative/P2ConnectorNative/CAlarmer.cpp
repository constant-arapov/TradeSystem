#include "CAlarmer.h"
#include "CLogger.h"


#include "CNamedPipeClient.h"


extern CAlarmer * g_alarmer;




using namespace std;

CAlarmer::CAlarmer(CNamedPipeClient * namedPipeClient)
{
	_namedPipeClient = namedPipeClient;
	_logger = new CLogger("CAlarmer");

}


CAlarmer * CAlarmer::Create()
{

	return new CAlarmer (new CNamedPipeClient(TEXT("\\\\.\\pipe\\myNamedPipe1"),
									   TEXT("\\\\.\\pipe\\myNamedPipe2"))
									  );
									


}
/*
 CAlarmer * CAlarmer::Get()
{
	
	

	return _instance;

}
*/


void CAlarmer::Error(string message)
{
	string writeMsg  = "[P2ConnectorNative] " + message;
	_namedPipeClient->WriteMessage(writeMsg);
	_logger->Log(message);

}


void Error(string message)
{
	g_alarmer->Error(message);

}




