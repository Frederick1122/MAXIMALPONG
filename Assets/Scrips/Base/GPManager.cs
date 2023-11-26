using System;
using System.Collections.Generic;
using Base;
using GamePush;
using Lean.Localization;

public class GPManager : Singleton<GPManager>
{
    private const string UI_PLAYER_LOCALIZATION = "UI_PLAYER";
    public event Action onUpdateLeaders;
    public event Action<bool> onRewardedClose;
    
    private List<LeaderboardFetchData> _leaders = new List<LeaderboardFetchData>();
    private List<LeaderboardFetchData> _leadersWithPlayer = new List<LeaderboardFetchData>();
    private LeaderboardFetchData _player;
    private Language _browserLanguage;
    private bool _isFetchLeaderboardActive = false;
    
    public List<LeaderboardFetchData> GetLeaders()
    {
        return _leadersWithPlayer;
    }

    public LeaderboardFetchData GetPlayer()
    {
        return _player;
    }

    public Language GetBrowserLanguage()
    {
        return _browserLanguage;
    }
    
    // public void UpdateScore(int score)
    // {
    //     _player.score = score;
    //     GP_Player.SetScore(_player.score);
    //     GP_Player.Sync();
    //     UpdateLeaders();
    // }
    //
    // public void UpdateNickname(string newNickname)
    // {
    //     _player.name = newNickname;
    //     GP_Player.SetName(_player.name);
    //     GP_Player.Sync();
    //     UpdateLeaders();
    // }

    public void ShowRewardedAds()
    {
        GP_Ads.ShowRewarded();
    }

    // public void FetchLeaderboard()
    // {
    //     if(_isFetchLeaderboardActive)
    //         return;
    //     
    //     _isFetchLeaderboardActive = true;
    //     GP_Leaderboard.Fetch("scoreboard", withMe: WithMe.last);
    // }
    
    protected override void Awake()
    {
        base.Awake();
        GP_Ads.ShowPreloader();
        GP_Ads.OnPreloaderClose += PreloaderClose;
        GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
        GP_Ads.OnRewardedClose += RewardedAdsClose;
        // _player = new LeaderboardFetchData()
        // {
        //     id = GP_Player.GetID(),
        //     name = GP_Player.GetName(),
        //     score = (int)GP_Player.GetScore()
        // };
        
        _browserLanguage = GP_Language.Current();
    }
    
    private void Start()
    {
        GP_Ads.ShowSticky();
    }
    
    private void OnDestroy()
    {
        GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
        GP_Ads.OnRewardedClose -= RewardedAdsClose;
        GP_Ads.OnPreloaderClose -= PreloaderClose;
    }

    private void RewardedAdsClose(bool success)
    {
        onRewardedClose?.Invoke(success);
    }

    private void PreloaderClose(bool success)
    {
        // FetchLeaderboard();
    }
    
    private void OnFetchSuccess(string fetchTag, GP_Data data)
    {
        _isFetchLeaderboardActive = false;
        _leaders = data.GetList<LeaderboardFetchData>();
        UpdateLeaders();
    }
    
    private void UpdateLeaders()
    {
        _leadersWithPlayer = _leaders;
        
        if(_leadersWithPlayer.Count == 0)
        {
            _leadersWithPlayer.Add(_player);
        }
        else
        {
            for (var index = _leadersWithPlayer.Count - 1; index >= 0; index--)
            {
                if (_leadersWithPlayer[index].score == 0 && _leadersWithPlayer[index].id != _player.id)
                {
                    _leadersWithPlayer.RemoveAt(index);
                }
                else if (_leadersWithPlayer[index].name == "")
                {
                    _leadersWithPlayer[index].name =
                        $"{LeanLocalization.GetTranslationText(UI_PLAYER_LOCALIZATION)}#{_leadersWithPlayer[index].id}";
                }
            }
        }
        
        onUpdateLeaders?.Invoke();
    }
}

[Serializable]
public class LeaderboardFetchData
{
    public int id;
    public int score;
    public string name;
}