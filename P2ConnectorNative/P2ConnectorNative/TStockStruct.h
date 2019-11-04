#ifndef _TSTOCKSTRUCT
#define _TSTOCKSTRUCT

#include "aggr.h"

  
struct TStockStruct
{
	char code;
	struct orders_aggr orders_aggr_inst;
};










#endif