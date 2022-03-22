using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace zFrame
{
    public class LogManager :MonoSingleton<LogManager>
    {
        public bool DeBugWriteOut = false;

        public override void OnInit()
        {
            DelLog();
            if (DeBugWriteOut)
            {
                Debug.Log("Log路径->"+ Application.persistentDataPath + "/zFrame.log");
                Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
            }
            
        }
    
        private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            WriteLog("![" + type.ToString() + "]->"+ condition+"\r\n ->"+ stackTrace + "\r\n");
        }
        private void WriteLog(string s)
        {
            FileStream fs = new FileStream(Application.persistentDataPath + "/zFrame.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.WriteLine(s);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        private void DelLog()
        {
            File.Delete(Application.persistentDataPath + "/zFrame.log");
        }

    } 
}
