using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core.Updater
{
    class Updater
    {


        public bool CheckUpdate()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string versionString = client.DownloadString(@"http://www.simpleminesweeper.ru/GameData/LastVersion");
                    Version lastVersion = JsonConvert.DeserializeObject<Version>(versionString);
                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    return lastVersion > currentVersion;
                }
                catch(Exception)
                {

                }

                return false;
            }
        }
    }
}
