using System;
using System.Net;

namespace BDOWeb
{
    public class Errors
    {
        public static void LogError(Exception ex)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            string error = " Error: " + ex.ToString();
            logger.Error(error);
        }
        public static void LogQuery(int result)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Number of rows: " + result);
        }
        public static void LogNoPerms(string info)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(info);
        }



    }
}