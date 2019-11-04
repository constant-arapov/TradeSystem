#ifndef _CMAPPER_
#define _CMAPPER_
	

class CMapperStock
{
private:


	private:
		long _isinId;
	    HANDLE _hMapFile;
		void * pBuffer;


	public:
		void Init();
		CMapperStock(long isinId);
		void Write(void * pntr);

};



#endif


