
#include "CAlarmer.h"
#include "MockAlarmer.h"


#include <string>


using namespace std;


MockAlarmer::MockAlarmer(CNamedPipeClient * namedPipeClient) 
	: CAlarmer(namedPipeClient), _logger("MockAlarmer", true)
{


}


void MockAlarmer::Error(string message)
{
	_logger.Log(message);

}
