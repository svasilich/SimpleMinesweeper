using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using SimpleMinesweeper.DialogWindows;

namespace SimpleMinesweeper.Core.Updater
{
    static class Updater
    {
        #region Fields

        private static IUpdateDialogProvider dialogProvider;

        #endregion

        #region Setters

        public static void SetDialogProvider(IUpdateDialogProvider provider)
        {
            dialogProvider = provider;
        }

        #endregion

        #region Business logic       
        
        public static void CheckVersionAndUpdateProgram(bool noConnectionMsg = false)
        {
            if (!CheckConnection())
            {
                if (noConnectionMsg)
                    MessageBox.Show($"Во время проверки наличия обновлений произошла ошибка:\nОтсутствует интернет-соединение.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!CheckUpdate(out Version ver))
                return;
            
            if(dialogProvider == null || ShowAskDialogInSTAThread(ver))
            {
                Update();
            }
        }

        private static bool ShowAskDialogInSTAThread(Version ver)
        {
            bool answer = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                answer = dialogProvider.AskBeforeUpdate(ver);
            });

            return answer;
        }

        public static bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(@"http://google.com/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckUpdate(out Version lastVersion)
        {
            using (WebClient client = new WebClient())
            {
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                try
                {
                    string versionString = client.DownloadString(@"http://www.simpleminesweeper.ru/GameData/LastVersion");
                    lastVersion = JsonConvert.DeserializeObject<Version>(versionString);                    
                    return lastVersion > currentVersion;
                }
                catch(Exception e)
                {
                    lastVersion = currentVersion;
                    MessageBox.Show($"Во время проверки наличия обновлений произошла ошибка:\n{e.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }

            return false;
        }

        public static void Update()
        {
            try
            {
                string installFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "SimpleMinesweeper.msi";
                if (File.Exists(installFile))
                    File.Delete(installFile);

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(@"http://www.simpleminesweeper.ru/GameData/GetInstaller", installFile);
                }

                Process.Start(installFile);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Во время загрузки новой версии произошла ошибка:\n{e.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
