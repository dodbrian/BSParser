using System;
using BSForms.Properties;

namespace BSForms
{
    /// <summary>
    /// Static class responsible for 
    /// configuration settings
    /// </summary>
    public static class Config
    {
        public static String GetRulesFileName()
        {
            return Settings.Default.RulesFileName;
        }

        public static void SetRulesFileName(String fileName)
        {
            Settings.Default.RulesFileName = fileName;
            Settings.Default.Save();
        }
    }
}
