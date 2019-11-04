#pragma pack(push, 4)

#ifndef _AGGR_H_
#define _AGGR_H_

// Scheme "AGGR" description

#include "cgate.h"


    struct orders_aggr
    {
        signed long long replID; // i8
        signed long long replRev; // i8
        signed long long replAct; // i8
        signed int isin_id; // i4
        char price[11]; // d16.5
        signed long long volume; // i8
        struct cg_time_t moment; // t
        unsigned long long moment_ns; // u8
        signed char dir; // i1
        struct cg_time_t timestamp; // t
        signed int sess_id; // i4
        
    };

    const size_t sizeof_orders_aggr = 84;
    const size_t orders_aggr_index = 0;



#endif //_AGGR_H_

#pragma pack(pop)
