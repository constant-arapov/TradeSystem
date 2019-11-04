using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Common.Interfaces;
using Common.Utils;

namespace Common.IO
{
    public abstract class CFileIOBinary
    {
        IAlarmable _alarmer;
        protected string _fileName;

      


        public CFileIOBinary(string fileName, IAlarmable alarmer)
        {
            _fileName = fileName;
            _alarmer = alarmer;

        }


        public void WriteToFile<T>(T dataToSeirilze) 
        {

            string path = GetFileName();

            FileStream fileStream = null;
            try
            {
                //if file exists and valid use it
                if (File.Exists(path) && CUtil.FileSize(path) > 0)
                {
                    fileStream = new FileStream(path, FileMode.Create);
                }
                //if not create new file
                else
                {
                    fileStream = File.Create(path);
                    //serializationStream = new FileStream(path, FileMode.Open);

                }
            }
            catch (IOException e)
            {
                Error("WriteToFile IO ", e);

            }
            catch (Exception e)
            {
                Error("WriteToFile ", e);

            }


            if (fileStream != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fileStream, dataToSeirilze);
                }
                catch (Exception e)
                {
                    Error("WriteToFile Serialize", e);
                }
                finally
                {
                    fileStream.Close();
                }
            }
          
        }



        
        public T LoadData<T>() 
        {


            

           T data =  default (T);//new T();


            string path = GetFileName();


            FileStream serializationStream = null;
            try
            {


                if (File.Exists(path) && CUtil.FileSize(path) != 0)
                {
                    serializationStream = new FileStream(path, FileMode.Open);
                }
                else
                {
                    //File.Create(path);
                    return data;//default value

                }
            }
            catch (IOException e)
            {
                Error("IO Error in LoadData ", e);

            }
            catch (Exception e)
            {
                Error("IO Error in LoadData ", e);

            }
            if (serializationStream == null)
            {

                return data;//default value
            }
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = (T)formatter.Deserialize(serializationStream);
            }
            catch (SerializationException e)
            {
                Error("IO Error in LoadData ", e);
            }
            catch (Exception e)
            {
                Error("IO Error in LoadData ", e);
            }
            finally
            {
                serializationStream.Close();
            }

            return data; //read value
        }
        


        protected virtual string GetFileName()
        {
            return "";

        }


        public void Error(string msg, Exception e)
        {
           
        }








    }
}
