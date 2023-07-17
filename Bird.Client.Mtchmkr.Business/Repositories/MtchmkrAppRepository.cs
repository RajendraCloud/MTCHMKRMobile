using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bird.Client.Mtchmkr.Business.ServiceCenter.Request;
using Bird.Client.Mtchmkr.Business.ServiceCenter.Response;

namespace Bird.Client.Mtchmkr.Business.Repositories
{
    public class MtchmkrAppRepository
    {
        readonly HttpRequestHelper client;
        public MtchmkrAppRepository()
        {
            this.client = new HttpRequestHelper();
        }
        
        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            return await this.client.CreateGetResponse<LoginResponse>($"User/Login?userName={userName}&Password={password}");
        }

        public async Task<bool> ForgotPasswordAsync(string userName)
        {
            return await this.client.CreateGetResponse<bool>($"User/ForgotPassword?userName={userName}");
        }

        public async Task<RegisterResponse> RegistrationAsync(UserModel user)
        {
            return await this.client.CreatePostRequest<RegisterResponse>($"User", user);
        }

        public async Task<bool> UploadImageAsync(ImageDTO imageObj)
        {
            return await this.client.CreatePostRequest<bool>($"User/UploadImage", imageObj);
        }

        public async Task<UserProfileResponse> GetProfileByIdAsync(Guid userId)
        {
            return await this.client.CreateGetResponse<UserProfileResponse>($"Profile/GetProfileByUserId?userId={userId}");
        }

        public async Task<bool> UpdateUserProfileAsync(ProfileResuest request)
        {
            return await this.client.CreatePutRequest<bool>($"Profile/UpdateProfile", request);
        }

        public async Task<bool> CheckUserExistAsync(string obj)
        {
            return await this.client.CreateGetResponse<bool>($"User/isUserPresent?email={obj}");
        }

        public async Task<List<ShowCaseResponse>> GetShowCaseDataAsync(Guid userId, float latitude, float longitude)
        {
            return await this.client.CreateGetResponse<List<ShowCaseResponse>>($"Match/Showcase?userID={userId}&latitude={latitude}&longitude={longitude}");
        }

        public async Task<List<GamesModel>> GetGamesAsync()
        {
            return await this.client.CreateGetResponse<List<GamesModel>>($"Game");
        }

        public async Task<List<LocationModel>> GetLocationsAsync()
        {
            return await this.client.CreateGetResponse<List<LocationModel>>($"Match/GetLocations");
        }

        public async Task<List<PlayerDTO>> GetSearchPlayersAsync(Guid gameid, int minRating, int maxrating, int radious, string gameFrame, Guid userId, string bookDate)
        {
            return await this.client.CreateGetResponse<List<PlayerDTO>>($"Match/PlaylistForSearchByDateWithUserId?gameid={gameid}&minRating={minRating}&maxrating={maxrating}&radious={radious}&gameFrame={gameFrame}&UserId={userId}&date1={bookDate}");
        }

        public async Task<bool> PlayerBookingAsync(PlayerBooking obj)
        {
            return await this.client.CreatePostRequest<bool>($"Match", obj);
        }

        public async Task<List<PendingMatchResponse>> GetPendingMatchAsync(Guid userId)
        {
            return await this.client.CreateGetResponse<List<PendingMatchResponse>>($"Match/PendingMatches?Userid={userId}");
        }

        public async Task<bool> ShowcaseGetIsAgreedResult(Guid matchId, bool playStatus)
        {
            return await this.client.CreateGetResponse<bool>($"Match/GetIsAgreedResult?matchId={matchId}&isAgreed={playStatus}");
        }

        public async Task<bool> AcceptOrRejectMTCH(Guid matchId, bool playStatus, string userId)
        {
            return await this.client.CreateGetResponse<bool>($"Match/GetIsAgreedResult?matchId={matchId}&isAgreed={playStatus}&RequestedUserId={userId}");
        }

        public async Task<List<PlayerDTO>> ScoreCardGetUserDetailsByMatchId(Guid matchId)
        {
            return await this.client.CreateGetResponse<List<PlayerDTO>>($"Match/GetUserDetailsByMatchId?MatchId={matchId}");
        }

        public async Task<List<PaymentInfo>> GetPaymentInfoDataAsync(Guid userId)
        {
            return await this.client.CreateGetResponse<List<PaymentInfo>>($"UserPaymentInfo/GetUserPaymentInfoDetails?UserId={userId}");
        }

        public async Task<bool> InsertPaymentInfoAsync(PaymentRequest info)
        {
            return await this.client.CreatePostRequest<bool>($"UserPaymentInfo", info);
        }

        public async Task<bool> InsertFCMInfoAsync(FcmDeviceInfo info)
        {
            return await this.client.CreatePostRequest<bool>($"User/SaveUserDeviceInfo", info);
        }

        public async Task<bool> FinishGame(string userId,string matchId)
        {
            return await this.client.CreatePutRequest<bool>($"Match/FinishMatch?userId={userId}&matchId={matchId}",new object());
        }

        public async Task<List<GetMatchDetailResponse>> GetMatchInfo(string matchId)
        {
            return await this.client.CreatePostRequest<List<GetMatchDetailResponse>>($"MatchScoreCard?matchId={matchId}",new object());
        }

        public async Task<bool> PutFrameCount(string matchId, string frameWinnerId, string frameNumber, bool confirmedbyPlayer = true)
        {
            return await this.client.CreatePutRequest<bool>($"MatchScoreCard?matchId={matchId}&FrameWinner={frameWinnerId}&FrameNumber={frameNumber}&ConfirmedbyPlayer={confirmedbyPlayer}", new object());
        }

        public async Task<int> GetPlayerFrame(string frameWinnerId)
        {
            var res = await this.client.CreateGetResponse<string>($"MatchScoreCard/GetFrameWinnerCountByUserId?userId={frameWinnerId}");
            return int.Parse(res);
        }

        public async Task<List<PlayedMatch>> GetPlayedMatches(string userId)
        {
            var res = await this.client.CreateGetResponse<List<PlayedMatch>>($"match/playedmatches?userId={userId}");
            return res;
        }
    }
}