using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class RewardedAdsManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public GameObject gameManager;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => {
            LoadRewardedAd();
        });
    }

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad...");
        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            rewardedAd = ad;

            
         

            RegisterEventHandlers(rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"Rewarded: {reward.Type}, Amount: {reward.Amount}");
                // You can optionally give rewards here too.
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready.");
            // If ad is not ready, directly call restart
            CallGameManagerRestart();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad closed. Calling GameManager.OnRestart()");
            CallGameManagerRestart();

            // Reload ad for next time
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open: " + error);
            CallGameManagerRestart();
        };
    }

    private void CallGameManagerRestart()
    {
        if (gameManager != null)
        {
            gameManager.GetComponent<GamaManager>().onClickRestart();
            // OR if you know the script: gameManager.GetComponent<GameManager>().OnRestart();
        }
        else
        {
            Debug.LogWarning("GameManager not assigned.");
        }
    }
}
