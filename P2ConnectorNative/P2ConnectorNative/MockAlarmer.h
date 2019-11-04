#ifndef _MOCK_ALARMER_
#define _MOCK_ALARMER_

#include "CLogger.h"
#include "CAlarmer.h"
#include "CNamedPipeClient.h"


class MockAlarmer : public CAlarmer
{

private: 
	CLogger _logger;


public:
	MockAlarmer(CNamedPipeClient * namedPipeClient);
	void Error(string message);
};


#endif