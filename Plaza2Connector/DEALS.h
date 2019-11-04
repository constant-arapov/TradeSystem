#pragma pack(push, 4)

// Scheme "Deals" description




    struct deal
    {
        signed long long replID; // i8
        signed long long replRev; // i8
        signed long long replAct; // i8
        signed int sess_id; // i4
        signed int isin_id; // i4
        signed long long id_deal; // i8
        signed long long id_deal_multileg; // i8
        signed long long id_repo; // i8
        signed int pos; // i4
        signed int amount; // i4
        signed long long id_ord_buy; // i8
        signed long long id_ord_sell; // i8
        char price[11]; // d16.5
        struct cg_time_t moment; // t
        signed char nosystem; // i1
        
    };

    const size_t sizeof_deal = 104;
    const int deal_index = 0;




#pragma pack(pop)
