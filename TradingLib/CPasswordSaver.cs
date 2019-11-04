using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


using Common;
using Common.Interfaces;
using Common.Utils;

//using Terminal.TradingStructs;
using TradingLib.ProtoTradingStructs;
using TradingLib.Data;

namespace TradingLib
{
    /// <summary>
    /// Saves passwords and load it
    /// </summary>
    public class CPasswordSaver
    {

        private UserConReq _lastUserConReq = null;

		private IAlarmable _alarmer;
        private CEncryptor _encryptor = new CEncryptor();

        /// <summary>
        /// List of CLocal account struct for last successfull auth 
        /// for ecach conId
        /// </summary>
		private List<CLocalAccount> _listLocalAccounts = new List<CLocalAccount>();




		public CPasswordSaver(IAlarmable alarmer)
		{
			_alarmer = alarmer;
			LoadData();
		}

        /// <summary>
        /// Find AuthReq in list for specific ConId
        /// </summary>
        /// <param name="ConId"></param>
        /// <returns></returns>
        public CAuthRequest GetUserAuthReq(int ConId)
		{

			var findRes = _listLocalAccounts.FirstOrDefault(a => a.ConnectionNum == ConId);

			if (findRes != null)
			{

                return new TradingLib.ProtoTradingStructs.CAuthRequest
                {
                    User = _encryptor.GetDecrypted(findRes.LoginHash),
                    Password = _encryptor.GetDecrypted(findRes.PwdHash)

                };

				

			}
			else //not exists
			{
				return null;
			}




		}

        /// <summary>
        /// On each connection trial remember UserConReq struct
        /// </summary>
        /// <param name="userConReq"></param>
        public void OnConnectionTrial(UserConReq userConReq)
        {

            _lastUserConReq = userConReq;

        }

		public void OnConnectedSuccess(int conId)
		{

			CUtil.TaskStart(SaveDataOnAuthSuccess);


		}



        /// <summary>
        /// If auth was succesfull do save 
        /// LocalAccount
        /// </summary>
		private void SaveDataOnAuthSuccess()
		{
			try
			{

				CLocalAccount localAccount = new CLocalAccount
				{
					 ConnectionNum =  _lastUserConReq.ConnNum,
					  LoginHash = _encryptor.GetEncrypted(_lastUserConReq.AuthRequest.User),
					  PwdHash = _encryptor.GetEncrypted(_lastUserConReq.AuthRequest.Password)
				};
                //2017-11-30
			/*	//found
				if (_listLocalAccounts.FirstOrDefault(a => a.ConnectionNum == _lastUserConReq.ConnNum) != null)
				{
					//_listLocalAccounts.Add(localAccount);
                    a = localAccount;

				}
				//not found
				else
				{
					_listLocalAccounts.Add(localAccount);
				}
                */

                bool bFound = false;
                for (int i=0; i <_listLocalAccounts.Count; i++)
                {
                    if (_listLocalAccounts[i].ConnectionNum == localAccount.ConnectionNum)
                    {
                        _listLocalAccounts[i] = localAccount;
                        bFound = true;
                        //  return;
                        break;
                    }

                }

                if (!bFound)
                    _listLocalAccounts.Add(localAccount);



				WriteToFile();

			}
			catch (Exception e)
			{
				_alarmer.Error("SavePassword", e);
			}
		}

		private void LoadData()
		{

			string fn = GetFileName();

			FileStream serializationStream = null;



			try
			{
				//if not exist create on saving
				if (!File.Exists(fn))
				{
					return;

				}
				else
				{
					serializationStream = new FileStream(fn, FileMode.Open);

				}

			}
			catch (Exception e)
			{
				Error("Saver. LoadData", e);
				return;
			}


			if (serializationStream == null)
			{
				Error("Saver.LoadData. SeriazlizationStream");
				return;
			}

			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				_listLocalAccounts = (List<CLocalAccount>)formatter.Deserialize(serializationStream);
			}
			catch (Exception exc)
			{
				Error("Saver. LoadData. Deserialize", exc);

			}
			finally
			{
				serializationStream.Close();

			}




		}



		private void WriteToFile()
		{

			string fn = GetFileName();

			FileStream fileStream = null;
			try
			{
				if (File.Exists(fn))
				{
					fileStream = new FileStream(fn, FileMode.Create);

				}
				else
				{
					fileStream = File.Create(fn);

				}
			}
			catch (Exception e)
			{

				Error("Saver.GetFileName",e);
					

			}
			if (fileStream == null)
			{
				Error("Saver FileStream not open");
				return;
			}


			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				formatter.Serialize(fileStream, _listLocalAccounts);
			}
			catch (Exception exc)
			{
				Error("Savaer. serizlize", exc);
			}
			finally
			{
				fileStream.Close();
			}


		}

		private void Error(string msg, Exception exc=null)
		{
			if (_alarmer != null)
				_alarmer.Error(msg, exc);
			
		}



		private string GetFileName()
		{
			string dir = CUtil.GetDataDir() + @"\colors.dat";
			
			return dir;

		}




    }
}
