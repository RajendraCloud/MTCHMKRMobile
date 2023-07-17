using System;
namespace Bird.Client.Mtchmkr.Business.ServiceCenter.Response
{
	public class PlayedMatch
	{
        public string userID { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public int ratings { get; set; }
        public string imageData { get; set; }
        public string gameName { get; set; }
        public string matchId { get; set; }
        public DateTime matchDate { get; set; }
        public string location { get; set; }
        public int frameWinCount { get; set; }
        public int maxFrame { get; set; }
        public int totalMatches { get; set; }
        public int win { get; set; }
        public int myFrameWinCount { get; set; }
    }
}

