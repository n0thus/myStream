using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myStream
{
    class TwitchAPI
    {

        private string name;
        private string chanID;
        //private string quality = "";

        private int followers = 0;
        private int newFollowers = 0;
        private int followersBeforeStreaming = -1;


        private bool haveUnlockedSubscribers = false;
        private int subscribersBeforeStreaming = 0;
        private int newSubscribers = 0;

        private bool isInLive = false;
        private int currentViewers = 0;

        private List<int> viewers = new List<int>();

        private TimeSpan difference;


        public TwitchAPI(string name)
        {
            this.name = name;
            askChanId();
        }


        public void askChanId()
        {
            JObject chanInfos;
            chanInfos = httpRequest.createRequest("https://api.twitch.tv/helix/users?login="+this.name, "GET", httpRequest.headerList.normal);
            chanID = chanInfos["data"][0]["id"].ToString();

        

            chanInfos = httpRequest.createRequest("https://api.twitch.tv/kraken/channels/"+ this.chanID, "GET", httpRequest.headerList.normal);
            this.haveUnlockedSubscribers = (bool)chanInfos["partner"];
        }

        public string getName()
        {
            return this.name;
        }
        public int getFollowers()
        {
            return this.followers;
        }

        public int getNewFollowers()
        {
            return this.newFollowers;
        }
        

        public int getNewSubscribers()
        {
            return this.newSubscribers;
        }

        public String getTimeOfStream()
        {
            String hours = difference.Hours+"";
            String minutes = difference.Minutes + "";
            String seconds = difference.Seconds + "";

            if (difference.Hours < 10) hours = "0" + hours;
            if (difference.Minutes < 10) minutes = "0" + minutes;
            if (difference.Seconds < 10) seconds = "0" + seconds;

            return hours + ":" + minutes + ":" + seconds;
        }


        public int getCurrentViewers()
        {
            return this.currentViewers;
        }
        
        


        public bool currentlyInLive()
        {
            return isInLive;
        }

        public bool haveUnlockSubs()
        {
            return this.haveUnlockedSubscribers;
        }












        public void getUserInformations()
        {
            JObject usersInfos;



            /*if (!chat.Url.ToString().Contains("https://www.twitch.tv/" + currChanName + "/chat"))
            {
                chat.Navigate("https://www.twitch.tv/" + currChanName + "/chat");
            }*/


            usersInfos = httpRequest.createRequest("https://api.twitch.tv/helix/streams?user_id=" + this.chanID, "GET", httpRequest.headerList.normal);
            

            if (usersInfos["data"].ToString().Equals("[]") && this.isInLive == true)
            {
                this.isInLive = false;
            }
            else if (!usersInfos["data"].ToString().Equals("[]") && this.isInLive == false)
            {
                this.isInLive = true;
            }

            if (this.isInLive == true)
            {
                this.currentViewers = Int32.Parse(usersInfos["data"][0]["viewer_count"].ToString());
                //this.quality = usersInfos["stream"]["video_height"].ToString();
                //this.quality = "/";

                String date = usersInfos["data"][0]["started_at"].ToString();
                DateTime myDate = Convert.ToDateTime(date);

                DateTime currentDate = DateTime.Now;

                difference = TimeZoneInfo.ConvertTimeToUtc(currentDate).Subtract(myDate);
                

                usersInfos = httpRequest.createRequest("https://api.twitch.tv/helix/users/follows?to_id=" + this.chanID, "GET", httpRequest.headerList.normal);
                if (this.followersBeforeStreaming == -1)
                {
                    this.followersBeforeStreaming = Int32.Parse(usersInfos["total"].ToString());
                }


                /*this.viewers.Add(Int32.Parse(this.currentViewers));
                int[] viewersIntArray = viewers.ToArray();

                if (viewersIntArray.Length > 10)
                {
                    this.viewers.RemoveAt(0);
                }

                this.averageOfViewers = (int)this.viewers.Average();*/




                try
                {
                    this.followers = Int32.Parse(usersInfos["total"].ToString());

                    if (followers > (this.followersBeforeStreaming + this.newFollowers))
                    {
                        int newFollowersInf = this.followers - this.followersBeforeStreaming;
                        this.newFollowers = newFollowersInf;
                    }
                }catch(Exception) { }

                

                if(this.haveUnlockedSubscribers)
                {
                    usersInfos = httpRequest.createRequest("https://api.twitch.tv/kraken/channels/" + this.chanID + "/subscriptions", "GET", httpRequest.headerList.oAuth);
                    int subscribers = Int32.Parse(usersInfos["_total"].ToString());
                    if (subscribersBeforeStreaming == -1)
                    {
                        subscribersBeforeStreaming = subscribers;
                    }

                    if(subscribers > (this.subscribersBeforeStreaming+ this.newSubscribers)) {
                        this.newSubscribers = subscribers - this.subscribersBeforeStreaming;
                    }
                }

            }

        }

    }
}
