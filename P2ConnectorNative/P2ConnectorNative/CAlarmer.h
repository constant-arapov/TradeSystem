#ifndef  _CALARMER_
#define  _CALARMER_


#include <string>

#include "CLogger.h"
#include "CNamedPipeClient.h"





using namespace std;

class CAlarmer
{

	private:
		CNamedPipeClient * _namedPipeClient;
		
		CLogger * _logger;	
	protected:
		CAlarmer(CNamedPipeClient * namedPipeClient);

	public:
		
		static CAlarmer * Create();
		
		virtual void Error(string message);
	
};

void Error(string message);



#endif