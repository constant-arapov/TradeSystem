#include "MockMapperGlobals.h"
#include <list>



using namespace std;

void MockMapperGlobals :: TestMapperGlobals()
{
	
	CreateGlobals();


	list<long> inListIsinId = GenerateListIsinIds();

	

	SetInstruments(inListIsinId);

	list<long> inListIsinOut = GetInstrumentList();

}




void MockMapperGlobals :: SetInstruments(const list<long> & listInstrIsinIds)
{

	int i=0;
	
	for (list<long>::const_iterator iterator = listInstrIsinIds.begin(), end = listInstrIsinIds.end(); 
																iterator != end; ++iterator) 


		  *((_int64*)  pBufInstruments + i++) = * iterator;
		   
}

void MockMapperGlobals :: SetInstruments()
{

	SetInstruments(GenerateListIsinIds());

}


list<long> MockMapperGlobals::GenerateListIsinIds()
{
	list<long> inListIsinId;
	//test Plaza

	inListIsinId.push_back(1069509);
	inListIsinId.push_back(1079851);
	inListIsinId.push_back(917440);
	inListIsinId.push_back(1085642);
	inListIsinId.push_back(953300);



	/*inListIsinId.push_back(430354);
	inListIsinId.push_back(422393);
	inListIsinId.push_back(457935);
	inListIsinId.push_back(457974);*/
	
	//production Plaza
	/*inListIsinId.push_back(985489);
	inListIsinId.push_back(986815);
	inListIsinId.push_back(836971);
	inListIsinId.push_back(986827);
	inListIsinId.push_back(880556);
	*/

	return inListIsinId;
}