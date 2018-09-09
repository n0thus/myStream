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
    public partial class chat : Form
    {
        public chat()
        {
            InitializeComponent();
        }

        private void chat_Load(object sender, EventArgs e)
        {

        }

        public ChromiumWebBrowser chromeBrowser;
        public void loadChannelChat(string name)
        {
            this.TopMost = true;
            InitializeChromium("https://www.twitch.tv/popout/" + name.ToLower() + "/chat?popout=");
        }
        
        public void InitializeChromium(String url)
        {
            chromeBrowser = new ChromiumWebBrowser(url);
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
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
