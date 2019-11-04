#pragma pack(push, 4)

// Scheme "message" description




    struct FutAddOrder
    {
        char broker_code[5]; // c4
        char isin[26]; // c25
        char client_code[4]; // c3
        signed int type; // i4
        signed int dir; // i4
        signed int amount; // i4
        char price[18]; // c17
        char comment[21]; // c20
        char broker_to[21]; // c20
        signed int ext_id; // i4
        signed int du; // i4
        char date_exp[9]; // c8
        signed int hedge; // i4
        signed int dont_check_money; // i4
        struct cg_time_t local_stamp; // t
        
    };

    const size_t sizeof_FutAddOrder = 148;
    const int FutAddOrder_index = 0;

    const int FutAddOrder_msgid = 36;


    struct FORTS_MSG101
    {
        signed int code; // i4
        char message[256]; // c255
        signed long long order_id; // i8
        
    };

    const size_t sizeof_FORTS_MSG101 = 268;
    const int FORTS_MSG101_index = 1;

    const int FORTS_MSG101_msgid = 101;


    struct FORTS_MSG99
    {
        signed int queue_size; // i4
        signed int penalty_remain; // i4
        char message[129]; // c128
        signed int code; // i4
        
    };

    const size_t sizeof_FORTS_MSG99 = 144;
    const int FORTS_MSG99_index = 2;

    const int FORTS_MSG99_msgid = 99;


    struct FORTS_MSG100
    {
        signed int code; // i4
        char message[256]; // c255
        
    };

    const size_t sizeof_FORTS_MSG100 = 260;
    const int FORTS_MSG100_index = 3;

    const int FORTS_MSG100_msgid = 100;




#pragma pack(pop)
