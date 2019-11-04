using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;
using System.Collections.Concurrent;
using System.Media;
using System.Text.RegularExpressions;
using System.IO;



namespace Common
{
    public class CSounder
    {
        BlockingCollection<CSoundMessage> m_queueSoundMsg = new BlockingCollection<CSoundMessage>();
        SoundPlayer m_soundPlayer = new SoundPlayer();

        string m_botDir;
        string m_isinDir;

        public CSounder()
        {
            string rootDir = System.Windows.Forms.Application.StartupPath+"\\sounds";
            m_botDir = rootDir + "\\bots";
            m_isinDir = rootDir + "\\instruments";

            (new Thread(ThreadFunc)).Start();

        }
        public void ThreadFunc()
        {
            foreach (CSoundMessage value in m_queueSoundMsg.GetConsumingEnumerable())
            {
                if (value.code == SoundCode.BotPositionOpen)
                    ProcessBotPositionOpen(value);



            }
        }

        public void PlaySound(CSoundMessage msg)
        {
            m_queueSoundMsg.Add(msg);


        }



        public void ProcessBotPositionOpen(CSoundMessage soundMessage)
        {
            string botName = soundMessage.param1;


            Regex newReg = new Regex(@"[\w\W]*_([\w\W]*)");
           Match m = newReg.Match(botName);

           if (m.Groups.Count > 1)
           {
               botName = m.Groups[1].ToString();
               string fn = m_botDir + "\\" + botName + ".wav";
               if (File.Exists(fn))
               {
                   m_soundPlayer.SoundLocation = fn;
                   m_soundPlayer.PlaySync();
               }
           }
           else return;
           string isin = soundMessage.param2;

           newReg = new Regex(@"([\w\W]*)\-[\w\W]*");
           m = newReg.Match(isin);
           if (m.Groups.Count > 1)
           {
               isin = m.Groups[1].ToString();
               string fn = m_isinDir + "\\" + isin + ".wav";
               if (File.Exists(fn))
               {
                   m_soundPlayer.SoundLocation = fn ;
                   m_soundPlayer.PlaySync();
               }
           }
           else return;

           
         
        }


    }
}
