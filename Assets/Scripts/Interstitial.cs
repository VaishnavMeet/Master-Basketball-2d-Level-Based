using GoogleMobileAds.Api;
using GoogleMobileAds;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Interstitial : MonoBehaviour
{
    private InterstitialAd _interstitialAd;
    public GameObject gameManager;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string _adUnitId = "unused";
#endif

    
    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadInterstitialAd();
        });
    }

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error);
                return;
            }

            _interstitialAd = ad;
            RegisterEventHandlers(_interstitialAd);
        });
    }

    public void ShowAdAndThenLoadLevel()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Ad closed. Now loading next level...");

                gameManager.GetComponent<GamaManager>().OnNextClick();
                // Preload next ad for future use
                LoadInterstitialAd();
            };

            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("Ad not ready, loading next level anyway.");
            
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue value) => { };
        ad.OnAdClicked += () => { };
        ad.OnAdFullScreenContentOpened += () => { };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Failed to open full screen content: " + error);
        };
        ad.OnAdFullScreenContentClosed += () => { };
    }
}
