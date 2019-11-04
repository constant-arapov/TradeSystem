#pragma pack(push, 4)

#ifndef _Deals_H_
#define _Deals_H_

// Scheme "Deals" description




    struct deal
    {
        signed long long replID; // i8
        signed long long replRev; // i8
        signed long long replAct; // i8
        signed int sess_id; // i4
        signed int isin_id; // i4
        signed long long id_deal; // i8
        signed int pos; // i4
        signed long long xpos; // i8
        signed int amount; // i4
        signed long long xamount; // i8
        signed long long id_ord_buy; // i8
        signed long long id_ord_sell; // i8
        char price[11]; // d16.5
        struct cg_time_t moment; // t
        unsigned long long moment_ns; // u8
        signed char nosystem; // i1
        
    };

    const size_t sizeof_deal = 116;
    const size_t deal_index = 0;



#endif //_Deals_H_

#pragma pack(pop)
