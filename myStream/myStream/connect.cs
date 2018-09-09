using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;

namespace myStream
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();

            applicationInformations.checkForNewVersionAvailable();
        }

        private void connect_Load(object sender, EventArgs e)
        {

        }



        public ChromiumWebBrowser chromeBrowser;
        public String username = "";

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser("");
            chromeBrowser.LoadHtml(myStream.Properties.Resources.connect.ToString());
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            showWait();
            setWaitText("Getting username...");
            string script = String.Format("document.getElementById('username').value;");

            chromeBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
            {
                var resp = x.Result;

                if (resp.Success && resp.Result.ToString() != null)
                {
                    username = resp.Result.ToString();
                }
            });

            int waitDuration = 0;

            while(username == "" && waitDuration < 5000)
            {
                Wait(1000);
                waitDuration += 1000;
            }

            if(waitDuration >= 5000)
            {
                showUsernameRegister();
            } else
            {
                webBrowser1.Navigate("https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=" + applicationInformations.clientID+ "&redirect_uri=http://localhost&response_type=code&scope=channel_subscriptions+user_read&state=" + applicationInformations.clientSecret);
                setWaitText("Getting code access...");
            }

        }


        public void showWait()
        {
            string waitScript = String.Format("waiting();");
            chromeBrowser.EvaluateScriptAsync(waitScript);
            button1.Hide();
        }

        public void showUsernameRegister()
        {
            String waitScript = String.Format("renderUsername();");
            chromeBrowser.EvaluateScriptAsync(waitScript);
            button1.Show();
        }

        public void setWaitText(String text)
        {
            string waitScript = String.Format("setDescText('"+text+"');");
            chromeBrowser.EvaluateScriptAsync(waitScript);
        }


        private void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
                Application.DoEvents();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().Contains("https://id.twitch.tv/oauth2/authorize?"))
            {
                webBrowser1.Show();
            }else
            {
                if (webBrowser1.Visible)
                    webBrowser1.Hide();
            }

            if (e.Url.ToString().Contains("http://localhost/?code="))
            {

                string code = e.Url.ToString().Remove(53);
                code = code.Replace("http://localhost/?code=", "");
                //MessageBox.Show("code :" + code, "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                setWaitText("Waiting for Twitch access token...");

                string uri = "http://localhost";

                string[] header = { };
                JObject result = httpRequest.createRequest("https://id.twitch.tv/oauth2/token?client_id=" + applicationInformations.clientID + "&client_secret=" + applicationInformations.clientSecret + "&code=" + code + "&grant_type=authorization_code&redirect_uri=" + uri, "POST", httpRequest.headerList.normal);

                //MessageBox.Show("Result :" + result.ToString(), "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

                string value = result["expires_in"].ToString();
                int toInt;
                Int32.TryParse(value, out toInt);
                if (toInt > 0)
                {
                    httpRequest.currentAccessToken = result["access_token"].ToString();
                    chromeBrowser.Delete();
                    linearBoard linearBoard = new linearBoard();
                    linearBoard.setUsername(username);
                    linearBoard.Show();
                    this.Hide();
                }
                
            }
        }

        bool _dragging = false;
        Point _start_point = new Point(0, 0);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
