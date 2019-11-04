using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces
{

    //TO DO make this a base calss not an interface

    public interface IXMLSerializable
    {

        string FileName { get; set; }
        
        bool NeedSelfInit { get; set; }

        void SelfInit();

    }
}
