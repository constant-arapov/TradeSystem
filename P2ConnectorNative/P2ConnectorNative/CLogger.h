#ifndef _CLOGGER_
#define _CLOGGER_

//#include <iosfwd>
#include <stdio.h>
#include <string>
using namespace std;

class CLogger
{

	private:
		//std::ofstream _logFile();
		FILE  * _logFile;
		string _name;
		string _path;
		bool _flushMode;


	public: 
		CLogger(string name, bool flushMode= false);


		~CLogger();

		void Log(string msg);
		void LogBanner(string msg);
		void LogStartBanner();
		void Flush();

		string GetDirectory ();	
		
};
#endif