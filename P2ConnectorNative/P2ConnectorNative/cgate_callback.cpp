#include <stdio.h>
#include <string.h>


#include "cgate.h"


#include "aggr.h"


#include "cgate_callback.h"


#include "CP2ConnectorNative.h"


//using namespace std;


extern CP2ConnectorNative *g_p2ConnectorNative;
extern CLogger lg;




/// <summary>
/// Callback from cgate - delegate data to g_p2ConnectorNative
/// 
/// </summary>
CG_RESULT MessageCallback(cg_conn_t* conn, cg_listener_t* listener, struct cg_msg_t* msg, void* data)
{
	int res = 0;

	switch (msg->type)
	{
	case CG_MSG_STREAM_DATA: 
		{ 
		// data recevied. print data properties (message name, index etc)

		struct cg_msg_streamdata_t* replmsg = (struct cg_msg_streamdata_t* )msg;
		if (replmsg->msg_name == NULL)
			break;

		/*
		printf("DATA message SEQ=%lld [table:[idx=%llu, id=%X, name=%s], dataSize:%llu]\n", 
			(long long)replmsg->rev,
			(unsigned long long)replmsg->msg_index,
			replmsg->msg_id, 
			replmsg->msg_name, 
			(unsigned long long)replmsg->data_size);
			*/
		if  (strcmp(replmsg->msg_name,  "orders_aggr") == 0  ) 
		{				
				orders_aggr * d = (orders_aggr*)  replmsg->data;			 
			   	g_p2ConnectorNative->ProcessOrdersAggr(d);
				break;
		}


		
		break;
		}
	case CG_MSG_P2REPL_ONLINE:
		g_p2ConnectorNative->ProcessOnline();
		
		// datastream is in ONLINE state
		//log_info("ONLINE");
		//lg.Write("==================================================================================== ONLINE ==================================================================");
		//bOnline = true;
		break;
	case CG_MSG_TN_BEGIN:
		// next data block is about to be received		
			g_p2ConnectorNative->ProcessBegin();
		break;
	case CG_MSG_TN_COMMIT:
		// data block finished. data is consistent now
			g_p2ConnectorNative->ProcessCommit();
		break;
	case CG_MSG_OPEN:
		// stream is opened, its scheme is available
		//log_info("OPEN");
		g_p2ConnectorNative->ProcessOpen();
		{
			// this is how scheme is handled
			struct cg_scheme_desc_t* schemedesc = 0;
		//	WARN_FAIL(cg_lsn_getscheme(listener, &schemedesc));
			if (schemedesc != 0)
			{
				struct cg_message_desc_t* msgdesc = schemedesc->messages;
				while (msgdesc)
				{
					struct cg_field_desc_t* fielddesc = msgdesc->fields;
					//log_info("Message %s, block size = %d", msgdesc->name, msgdesc->size);

					while (fielddesc)
					{
						//log_info("\tField %s = %s [size=%d, offset=%d]", fielddesc->name, fielddesc->type, fielddesc->size, fielddesc->offset);
						fielddesc = fielddesc->next;
					}
					
					msgdesc = msgdesc->next;
				}
			}
		}
		break;
	case CG_MSG_CLOSE:
		// Stream is closed, no more data will be received
		//log_info("CLOSE");
		break;
	case CG_MSG_P2REPL_LIFENUM:
		// Stream data scheme's life number was changed.
		// complete data snapshot re-replication will occur.
		//log_info("Life number changed to: %d", *((uint32_t*)msg->data));
		break;
	case CG_MSG_P2REPL_REPLSTATE:
		// Datastream state is recevied to be used for resume later.
		// Message content may be stored anywhere as a string
		// and then used in cg_lsn_open(..) call as value for
		// parameter "replstate="
		//log_info("Replica state: %s", msg->data);
		break;
	default:;
		// Other messages get logged but not handled
		//log_info("Message 0x%X", msg->type);
	}

	// code returns 0, since there were no errors.
	// any other error code may be return and it will be logged
	return CG_ERR_OK;
}


BOOL WINAPI InterruptHandler(DWORD reason)
{
	//log_info("----BREAK----");
	bExit = 1;
	return 1;
}