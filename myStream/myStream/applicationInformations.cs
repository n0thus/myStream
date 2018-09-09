using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myStream
{
    class applicationInformations
    {
        public static string clientID = "";
        public static string clientSecret = "";

        public static String version = "1.0.0";
        public static void checkForNewVersionAvailable()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://raw.githubusercontent.com/n0thus/myStream/master/version.json");
            StreamReader reader = new StreamReader(stream);
            String gitFile = reader.ReadToEnd();
            JObject gitVersion = JObject.Parse(gitFile);

            if (!gitVersion["version"].ToString().Equals(version))
            {
                MessageBox.Show("A new version of myStream is available ! If you want to install it get it on the discord (https://discord.gg/Mv92QhZ)", "New version detected !", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
        }
    }
}
