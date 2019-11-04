using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Logger;



namespace TradingLib.Bots
{
    public class CBotState<T>
    {
        T m_currState;
        ILogable m_logger;

        object _lck = new object();
        
        public CBotState (T initialState, ILogable logger)
        {
            m_currState = initialState;
            m_logger = logger;

        }

        public void SetState(T newState)
        {

            lock (_lck)
            {
                if (!EqualityComparer<T>.Default.Equals(newState, m_currState))
                {
                    m_logger.Log("========================================== change state from " + m_currState + " ========== to ===========" + newState + "=======================================================");
                    m_currState = newState;

                }
            }
        }

        public bool IS(T compState)
        {
            lock (_lck)
            {
                return EqualityComparer<T>.Default.Equals(compState, m_currState);
            }             

        }

    }   


    public enum EnmSupervisorBotStates
    {
        stt_001_Initial,
        stt_0021_WaitCloseLastSessionPos,
        stt_003_OnlineControlling,
        stt_0031_WaitAllPositionsWithIsinClose,
        stt_010_ErrorSuspend
        

    }




}
