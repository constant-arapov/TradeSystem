#ifndef _UTILS_
#define _UTILS_

#include <Windows.h>

#include <string>

#include "cgate.h"


using namespace std;

SYSTEMTIME CGTimeToSystemTime(cg_time_t cgtm);
double TimeDiffereceMS(SYSTEMTIME syst1, SYSTEMTIME syst2 );
double TimeDifferentServerMS(cg_time_t cgtm);
string CGTImeToString(cg_time_t cgtm);


string  GetStringTime();





#endif