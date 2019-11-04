#ifndef _MOCK_MAPPER_GLOBALS_
#define _MOCK_MAPPER_GLOBALS_

#include "stdafx.h"

#include <list>
using namespace std;



#include "CMapperGlobals.h"


class MockMapperGlobals : public CMapperGlobals
{


private:
		void SetInstruments(const list<long> & listInstrIsinIds);
		
		list<long> GenerateListIsinIds();

public:
		void TestMapperGlobals();
		void SetInstruments();


};


#endif