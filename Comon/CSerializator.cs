using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

using Common.Interfaces;
using Common.Utils;


namespace Common
{

    public static class CSerializator
    {


        public static void Write<T>(ref T ReadObj)
        {
           


            XmlSerializer ser = new XmlSerializer(typeof(T));
            System.IO.FileStream fileToWrite = null;
            string fname = "";
            string tmpFname = "";
         
                     
            fname = ((IXMLSerializable)ReadObj).FileName;
            tmpFname = CUtil.GetTempFileName(fname);

               
              

              const int MAX_TRIALS = 3;
              int i ;
              for (i= 0; i < MAX_TRIALS; i++)
               {
                    try
                    {
                        fileToWrite = System.IO.File.Open(tmpFname, System.IO.FileMode.Create);
                        ser.Serialize(fileToWrite, ReadObj);
                        fileToWrite.Flush();
                        
                       
                        break;
                    }
                    catch (Exception e)
                    {
                        if (e!=null)
                            Thread.Sleep(0); //for debugging
                    }

                    finally
                    {

                        if (fileToWrite != null)
                            fileToWrite.Close();

                    }
                 
                }


              if (i == MAX_TRIALS)
                  return;

              try
              {
                  File.Copy(tmpFname, fname, true);
                  File.Delete(tmpFname);
              }
              catch (Exception e)
              {
                  if (e != null)
                      Thread.Sleep(0); //for debugging

              }


           }
                             
    



        public static void Read<T>(ref T ReadObj)
        {

            FileStream fileToRead = null;

            try
            {

                if (((IXMLSerializable)ReadObj).NeedSelfInit)
                {
                    ((IXMLSerializable)ReadObj).SelfInit();
                    Write(ref ReadObj);

                }
                System.Xml.Serialization.XmlSerializer reader =
                       new System.Xml.Serialization.XmlSerializer(typeof(T));


                fileToRead = System.IO.File.OpenRead(((IXMLSerializable)ReadObj).FileName);
                string fileNameToRestore = ((IXMLSerializable)ReadObj).FileName;
                ReadObj = (T)reader.Deserialize(fileToRead);
                ((IXMLSerializable)ReadObj).FileName = fileNameToRestore;
              

            }
            catch (Exception e)
            {
				//2017-08-02
				throw e;

            }
            finally
            {
                if (fileToRead != null)
                    fileToRead.Close();
                   



            }

        }

        private static void ReadTrialMain<T>(ref T ReadObj)
        {

            bool bReadProblem = false;
            Exception eOut = null;
                if (((IXMLSerializable)ReadObj).NeedSelfInit)
                {
                    ((IXMLSerializable)ReadObj).SelfInit();
                    Write(ref ReadObj);

                }
                System.Xml.Serialization.XmlSerializer reader =
                       new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.IO.FileStream fileToRead =null;
                try
                {
                   fileToRead = System.IO.File.OpenRead(((IXMLSerializable)ReadObj).FileName);

                    string fileNameToRestore = ((IXMLSerializable)ReadObj).FileName;
                    ReadObj = (T)reader.Deserialize(fileToRead);
                    ((IXMLSerializable)ReadObj).FileName = fileNameToRestore;
                }
                catch (Exception e)
                {
                  //  System.Diagnostics.Debug.Assert(false, "Error. CSerializator.ReadTrialMain");
                    bReadProblem = true;
                    eOut = e;
                }

                finally
                {

                     if (fileToRead!=null)
                    fileToRead.Close();
                     if (bReadProblem)
                         throw eOut;
                }

        }

        private static void ReadTrialBackup<T>(ref T ReadObj)
        {

            if (((IXMLSerializable)ReadObj).NeedSelfInit)
            {
                ((IXMLSerializable)ReadObj).SelfInit();
                Write(ref ReadObj);

            }
            System.Xml.Serialization.XmlSerializer reader =
                   new System.Xml.Serialization.XmlSerializer(typeof(T));


            string fname = ((IXMLSerializable)ReadObj).FileName;

                string path = CUtil.GenBackupFileName(fname);

            System.IO.FileStream fileToRead =null;
                try
                {
                    fileToRead = System.IO.File.OpenRead(path);

                    string fileNameToRestore = path;// ((IXMLSerializable)ReadObj).FileName;
                    ReadObj = (T)reader.Deserialize(fileToRead);
                    ((IXMLSerializable)ReadObj).FileName = fileNameToRestore;
                }
                catch (Exception )
                {
                    throw;

                }
                finally
                {
                    if (fileToRead!=null)
                    fileToRead.Close();
                }
         }
        




        public static void ReadSafe<T>(ref T ReadObj)
        {
            try
            {
                try
                {

                    ReadTrialMain<T>(ref ReadObj);
                }

                catch (System.InvalidOperationException)
                {

                    ReadTrialBackup<T>(ref ReadObj);

                }

                

            }
            catch (Exception e)
            {

                String er = e.Message;

            }

        }





    }
   
}
