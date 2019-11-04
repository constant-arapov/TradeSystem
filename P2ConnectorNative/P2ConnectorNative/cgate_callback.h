#ifndef _CGATE_CALLBACK_
#define _CGATE_CALLBACK_

#include "stdafx.h"
#include <Windows.h>




#define CHECK_FAIL(f) \
	{ \
		uint32_t res = f; \
		if (res != CG_ERR_OK) \
		{ \
			log_info("Client gate error: %x", res); \
			exit(1); \
		} \
	}

// helper variable used as a notification flag to exit
static volatile int bExit = 0;


#define WARN_FAIL(f) \
	{ \
		uint32_t res = f; \
		if (res != CG_ERR_OK) \
			log_info("Error: %x", res); \
	} 


BOOL WINAPI InterruptHandler(DWORD reason);
CG_RESULT MessageCallback(cg_conn_t* conn, cg_listener_t* listener, struct cg_msg_t* msg, void* data);


#endif