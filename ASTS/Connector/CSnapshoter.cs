using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



using Common;
using Common.Interfaces;
using Common.IO;
using Common.Utils;

using ASTS.Interfaces.Clients;
using ASTS.Common;


namespace ASTS.Connector
{
    public class CSnapshoter : CBaseFunctional
    {

         

        private byte[] _snapshotNew;
        private byte[] _snapshotSave;

        private AutoResetEvent _evUpdate = new AutoResetEvent(false);

        private object _lkSnapshot = new object();
        private CFileIOBinaryDataStatic _fileIO;

        private IClientSnapshoter _client;

        private string _fileName;

        

        public bool IsSnapshotAvail
        {
            get
            {
                if (_snapshotSave != null)
                    return true;
                
                return false;
            }

        }


        public bool IsSnapshotFileIsOld()
        {

         //   if (!IsSnapshotAvail)
           //     return true;

            string fullPath = GetFullPath();

            if (!File.Exists(fullPath))
                return true;

            double _parDtHours = 4;
            DateTime dtFile = File.GetLastWriteTime(fullPath); //File.GetCreationTime(fullPath);


           // if ((DateTime.Now - dt).TotalHours > _parDtHours)
             //   return true;

			//double dtDays = (DateTime.Now - dt).TotalDays;

			//if (dtDays > 0)
				//return true;

			if ( (DateTime.Now.Date != dtFile.Date) ||  //another day
				((DateTime.Now.Date== dtFile.Date) &&  ((DateTime.Now - dtFile).TotalHours > _parDtHours  ))) //night time end and next session begin
				return true;



         

            return false;
        }


        public CSnapshoter(IClientSnapshoter clientSnapshoter,
                            IAlarmable alarmer) 
            : base(alarmer)
        {
            //GetSnapshot = getSnapshot;
            _fileName =  "TablesSnapshot.dat";

            _fileIO = new CFileIOBinaryDataStatic(GetFullPath(), _alarmer);
            _snapshotSave =   _fileIO.LoadData<byte[]>();
            //_snapshotSave_snapshotNew.CopyTo(_snapshotSave, 0);

            CUtil.ThreadStart(ThreadSaveSnapshot);
            _client = clientSnapshoter;

        }

        public void UpdateSnapshot()
        {
            try
            {


                _snapshotNew = _client.GetSnapshot();
                      

                lock (_lkSnapshot)
                {
                    _snapshotSave = new byte[_snapshotNew.Length];
                    _snapshotNew.CopyTo(_snapshotSave, 0);
                }

                _evUpdate.Set();

            }
            catch (Exception e)
            {
                Error("UpdateSnapshot",e);
            }


        }

        public byte[] LoadSnapshot()
        {
            return _snapshotSave;
        }

        private string GetFullPath()
        {

            string path =   CUtil.GetDataDir() +"\\" + _fileName;
            return path;
        }

        private void ThreadSaveSnapshot()
        {
            
           
            while (true)
            {
                _evUpdate.WaitOne();
                lock (_lkSnapshot)
                {   
                    _fileIO.WriteToFile(_snapshotSave);
                }

            }

        }



        





    }
}
