#ifndef _CSTOCKPROCESSOR_
#define _CSTOCKPROCESSOR_

#include <Windows.h>
#include <queue>
#include <list>
#include <string>

#include "aggr.h"
#include "EnumBuySell.h"
#include "TStockStruct.h"

#include  "CMapperStock.h"
#include "CLogger.h"

using namespace std;

#define STOCK_BUFFER_SIZE 3 * 4096
#define STOCK_SIZE 50



/// <summary>
///	Accummulates data in buffer. 
///
///    DATA PROCESSING ALGO CONTINUE DESCRIPTION:(see begining in CStockRouter)
///		1. Accumulate data in _queueEntry
///     2. When recieved "transaction end" element do process all accumulated data  
///		   elements. Put each element to "Raw buffer". In "Raw buffer" each element has  
///	       it's own replId. If replId already exists - do update element with new one. 
///        If not  do add it to the end. 
///     3. After all transaction elements saved in raw buffer do process them.
///	       Put data to "Work buffer". "Buy" elements do put to the first half of buffer. "Sell"
///		   elements do put to the second half of buffer.
///     4. In work "Work buffer" do sort elements by price. "Buy" sort desc, "Sell" sort asc.
///		5. After element sorted in "Work buffer" push buffer to StockMapper which copies
///         data to Memory Mapped File.
/// </summary>
class CStockProcessor
{

	private:

		queue<TStockStruct> _queueEntry;
		queue<TStockStruct> _queueProcessing;

		CRITICAL_SECTION _critSect;
		HANDLE _evForceQueueProc;
	

		char _bufferRaw[10*STOCK_BUFFER_SIZE];
		char _bufferWork[STOCK_BUFFER_SIZE];
		
		int currBufferRowElCount;
		int _sizeStockOneDir;
		long _isinId;

		CMapperStock * _mapper;


		void ProcessElement(const orders_aggr &  rcvdOrdersAggr);
		void ProcessWorkBuffer();
		void ProcessTransaction();
		double GetDoublePrice(void * pPrice);
		
		void SortDir(int first, int last, EnumBuySell dir);

		//methods for debug
		void DbgSort(long firstEl, long lastEl,  list<string> &lst );
		void DbgPrintWorkBuff();

		CLogger  * _logger;

	public:
		CStockProcessor(long isinId);
		void AddStocStruct(TStockStruct stockStruct);
		void ThreadProcess();
		void ForceProcessQueue();
		

};



#endif