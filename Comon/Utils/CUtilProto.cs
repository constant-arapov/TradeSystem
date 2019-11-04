using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;


using ProtoBuf;


namespace Common.Utils
{
    public static class CUtilProto
    {
        public static byte[] SerializeProto(object obj)
        {
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            sw.Reset(); sw.Start();
            sw1.Reset(); sw1.Start();
            sw2.Reset(); sw2.Start();


            sw.Reset(); sw.Start();


            var ms = new MemoryStream();
            sw2.Stop();
            Serializer.Serialize(ms, obj);
            sw1.Stop();
            byte[] serialized = ms.ToArray();
            sw.Stop();

            if (sw.ElapsedMilliseconds > 5)
            {
                System.Threading.Thread.Sleep(0);
            }

            return serialized;

        }

        public static T DeserializeProto<T>(byte[] byteArr)
        {
            return Serializer.Deserialize<T>(new MemoryStream(byteArr));

        }


    }
}
