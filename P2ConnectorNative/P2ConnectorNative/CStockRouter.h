#ifndef _CSTOCKROUTER_
#define _CSTOCKROUTER_

#include <queue>
#include <list>
#include <map>

#include "TStockStruct.h"
#include "CStockProcessor.h"


#define MAXINSTRUMENTS 512

using namespace std;


/// <summary>
/// Routes data to instrument dependend objects - StockProcessors. 
/// Add extra transaction end message for instrument on commit.
/// 
/// DATA PROCESSING  ALGO CONTINUE description (see begining in CP2Connector description)
///
/// 4. On recieve "data" type element accumulate it in _queueRouteEntry
/// 5. On recieve "tranaction-end" type do:
///    5.1 Route data from  _queueRouteEntry  of specific instrument to 
///		   corresponed StockProcessor
///	   5.2. If at least one data element was received for instrument,
///			create "dummy" "transaction-end" type element and send it to
///			corresponded StockProcessor (see StockProcessor description next).
///			
/// </summary>


class CStockRouter
{

	private :
		queue<TStockStruct> _queueRouteEntry;
		

		int _instrumentCount ;
	 
		
		map <long, CStockProcessor*>  _mapStockProcessors;
		map <long, bool> _mapInstrNeedFlush;

		void ProcessTransaction();
		

	public:
		void Init(const list<long>  &  listIsinIds);
		void Route(const TStockStruct & stockStruct);
		

};


#endif