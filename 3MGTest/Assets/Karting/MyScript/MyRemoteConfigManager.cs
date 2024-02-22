using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class MyRemoteConfigManager : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }
        else
        {
            Debug.Log("No Internet Connection.");
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteConfig(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session and no local cache file exists; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
                SetEventSettings();
                SetStoreItemPrice();
                break;
        }
    }

    void SetEventSettings()
    {
        LocalStorage.isEventEnable = RemoteConfigService.Instance.appConfig.GetBool("GoldRushEvent", false);
        ObserverEventManager.RaiseOnEventUpdate();
    }
    void SetStoreItemPrice()
    {
        foreach (Item item in StoreManager.AllItems)
        {
            LocalStorage.MyPlayerPrefs.SetInt("prive_"+item.Id, RemoteConfigService.Instance.appConfig.GetInt(item.Id, item.DefaultPrice));
        }
    }
}