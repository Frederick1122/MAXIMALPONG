using Base;
using GamePush;

public class GPManager : Singleton<GPManager>
{
    private Language _browserLanguage;
    private bool _isFetchLeaderboardActive = false;
    
    public Language GetBrowserLanguage() => _browserLanguage;

    protected override void Awake()
    {
        base.Awake();
        GP_Ads.ShowPreloader();
        _browserLanguage = GP_Language.Current();
    }
    
}