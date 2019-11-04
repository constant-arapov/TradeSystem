using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


using Common;
using Common.Utils;


namespace Terminal.Common
{
    public class CIOBinary<T>  where T: new ()
    {
        private string _instrument;
        private string _subdir;


        public CIOBinary(string instrument, string subdir)
        {
            _instrument = instrument;
            _subdir = subdir;
        }

        private string GetDataDirectory()
        {
            string dir = CUtil.GetDataDir() + @"\"+_subdir+@"\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;

        }

        private string GetFileName()
        {
            string dir = GetDataDirectory();

            string path = dir + _instrument.ToString() + "_" + CUtilTime.GeDateString(DateTime.Now).ToString() + ".dat";
            return path;

        }


        public void WriteToFile(T dataToSeirilze)
        {

            string path = GetFileName();

            FileStream fileStream = null;
            try
            {
                //if file exists and valid use it
                if (File.Exists(path) && CUtil.FileSize(path)>0) 
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
                CKernelTerminal.ErrorStatic("WriteToFile IO " + _subdir, e);

            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("WriteToFile " + _subdir, e);

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
                    CKernelTerminal.ErrorStatic("WriteToFile Serialize", e);
                }
                finally
                {
                    fileStream.Close();
                }
            }


        }

        private void CleanOldData()
        {

            string dir = GetDataDirectory();
            string[] files = Directory.GetFiles(dir);

            int parDayOlder = 3;

            files.ToList().ForEach(file =>
            {
                if (file.Contains(_instrument))
                    if ((DateTime.Now - File.GetLastWriteTime(file)).TotalDays > parDayOlder)
                        File.Delete(file);
            }
            );




        }



     
        public T LoadData()
        {


            CleanOldData();

            T list = new T();


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
                    return list;

                }
            }
            catch (IOException e)
            {
                CKernelTerminal.ErrorStatic("IO Error in LoadData " + _subdir, e);

            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("Error in LoadData " +_subdir, e);

            }
            if (serializationStream == null)
            {

                return new T();
            }
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                list = (T)formatter.Deserialize(serializationStream);
            }
            catch (SerializationException e)
            {
                CKernelTerminal.ErrorStatic("Serialization error in LoadData " + _subdir, e);
            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("Common error during serialization in LoadData " + _subdir, e);
            }
            finally
            {
                serializationStream.Close();
            }
           
            return list;
        }





    }
}
