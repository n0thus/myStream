using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myStream
{
    public partial class linearBoard : Form
    {
        public linearBoard()
        {
            InitializeComponent();
            InitializeChromium();
        }

        public ChromiumWebBrowser chromeBrowser;
        private TwitchAPI api;

        public void InitializeChromium()
        {
            chromeBrowser = new ChromiumWebBrowser("file:///D:/perso/myStreams/software/ui/html/linearBoard.html");
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        public void setUsername(string username)
        {
            api = new TwitchAPI(username);
            timer1.Start();
        }
        private void linearBoard_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            chat userChat = new chat();
            userChat.loadChannelChat(api.getName());
            userChat.Show();
        }


        bool lastLiveStatus = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            api.getUserInformations();

            if(api.currentlyInLive() && !lastLiveStatus)
            {
                string request = String.Format("setOnline();");
                chromeBrowser.EvaluateScriptAsync(request);
            }else if(!api.currentlyInLive() && lastLiveStatus)
            {
                string request = String.Format("setOffline();");
                chromeBrowser.EvaluateScriptAsync(request);
            }

            if(api.currentlyInLive())
            {
                string request = String.Format("updateElements('"+api.getCurrentViewers()+"','"+api.getTimeOfStream()+"','"+api.getNewFollowers()+"','"+api.getNewSubscribers()+"');");
                chromeBrowser.EvaluateScriptAsync(request);
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
    }
}
