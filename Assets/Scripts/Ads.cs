using UnityEngine;
using YandexMobileAds.Base;
using YandexMobileAds;
using System;

namespace NewNamespace
{
    public class Ads : MonoBehaviour
    {
        [SerializeField] private bool _isActivated;

        private void Awake()
        {
            if (!_isActivated) return;
            //Banner
            RequestInlineBanner();

            //AppOpen
            SetupAppOpenLoader();
            AppStateObserver.OnAppStateChanged += HandleAppStateChanged;
            RequestAppOpenAd();

            //Interstitial
            SetupInterLoader();
            RequestInterstitial();

            //Rewarded
            SetupRewardedLoader();
            RequestRewardedAd();
        }

        private void OnDestroy()
        {
            AppStateObserver.OnAppStateChanged -= HandleAppStateChanged;
        }

        #region Banner

        private Banner banner;

        private int GetScreenWidthDp()
        {
            int screenWidth = (int)Screen.safeArea.width;
            return ScreenUtils.ConvertPixelsToDp(screenWidth);
        }

        private void RequestInlineBanner()
        {
            string adUnitId = "demo-banner-yandex"; // "R-M-12002785-4"
            BannerAdSize bannerMaxSize = BannerAdSize.InlineSize(GetScreenWidthDp(), 100);
            banner = new Banner(adUnitId, bannerMaxSize, AdPosition.BottomCenter); 
            AdRequest request = new AdRequest.Builder().Build();
            banner.LoadAd(request);
            banner.OnAdLoaded += (arg1, arg2) => ShowInlineBanner();
        }

        private void ShowInlineBanner()
        {
            if (!_isActivated) return;
            banner.Show();
        }
        #endregion

        #region AppOpen

        private AppOpenAdLoader appOpenAdLoader;
        private AppOpenAd appOpenAd;
        private bool isColdStartAdShown = false;


        private void SetupAppOpenLoader()
        {
            appOpenAdLoader = new AppOpenAdLoader();
            appOpenAdLoader.OnAdLoaded += HandleAdLoaded;
            appOpenAdLoader.OnAdFailedToLoad += HandleAppOpenAdFailedToLoad;
        }

        private void HandleAppStateChanged(object sender, AppStateChangedEventArgs args)
        {
            if (!args.IsInBackground)
            {
                ShowAppOpenAd();
            }
        }

        private void ShowAppOpenAd()
        {
            if (!_isActivated) return;
            if (appOpenAd != null)
            {
                appOpenAd.Show();
            }
        }

        private void RequestAppOpenAd()
        {
            string adUnitId = "demo-appopenad-yandex"; // "R-M-12002785-3"
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            appOpenAdLoader.LoadAd(adRequestConfiguration);
        }

        public void HandleAdLoaded(object sender, AppOpenAdLoadedEventArgs args)
        {
            appOpenAd = args.AppOpenAd;

            appOpenAd.OnAdFailedToShow += HandleAppOpenAdFailedToShow;
            appOpenAd.OnAdDismissed += HandleAppOpenAdDismissed;

            if (!isColdStartAdShown)
            {
                ShowAppOpenAd();
                isColdStartAdShown = true;
            }
        }

        public void HandleAppOpenAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // Ad {args.AdUnitId} failed for to load with {args.Message}
            // Attempting to load a new ad from the OnAdFailedToLoad event is strongly discouraged.
        }

        public void HandleAppOpenAdDismissed(object sender, EventArgs args)
        {
            DestroyAppOpenAd();

            RequestAppOpenAd();
        }

        public void HandleAppOpenAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            // Called when an ad failed to show.

            // Clear resources.
            DestroyAppOpenAd();

            // Now you can preload the next ad.
            RequestAppOpenAd();
        }

        public void DestroyAppOpenAd()
        {
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }
        }

        #endregion

        #region Interstitial

        private InterstitialAdLoader interstitialAdLoader;
        private Interstitial interstitial;

        private void SetupInterLoader()
        {
            interstitialAdLoader = new InterstitialAdLoader();
            interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
        }

        private void RequestInterstitial()
        {
            string adUnitId = "demo-interstitial-yandex"; // "R-M-12002785-1"
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            interstitialAdLoader.LoadAd(adRequestConfiguration);
        }

        public void ShowInterstitial()
        {
            if (!_isActivated) return;
            if (interstitial != null)
            {
                interstitial.Show();
            }
        }

        public void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            interstitial = args.Interstitial;
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdDismissed += HandleInterstitialDismissed;
        }

        public void HandleInterstitialDismissed(object sender, EventArgs args)
        {
            DestroyInterstitial();
            RequestInterstitial();
        }

        public void HandleInterstitialFailedToShow(object sender, EventArgs args)
        {
            DestroyInterstitial();
            RequestInterstitial();
        }

        public void DestroyInterstitial()
        {
            if (interstitial != null)
            {
                interstitial.Destroy();
                interstitial = null;
            }
        }
        #endregion

        #region Rewarded

        private RewardedAdLoader rewardedAdLoader;
        private RewardedAd rewardedAd;

        private void SetupRewardedLoader()
        {
            rewardedAdLoader = new RewardedAdLoader();
            rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
            rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
        }

        private void RequestRewardedAd()
        {
            string adUnitId = "demo-rewarded-yandex"; // "R-M-12002785-2"
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            rewardedAdLoader.LoadAd(adRequestConfiguration);
        }

        public void ShowRewardedAd()
        {
            if (!_isActivated) return;
            if (rewardedAd != null)
            {
                rewardedAd.Show();
            }
        }

        public void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            rewardedAd = args.RewardedAd;
            rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
            rewardedAd.OnAdDismissed += HandleAdDismissed;
            rewardedAd.OnRewarded += HandleRewarded;
        }

        public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // Ad {args.AdUnitId} failed for to load with {args.Message}
            // Attempting to load a new ad from the OnAdFailedToLoad event is strongly discouraged.
        }

        public void HandleAdDismissed(object sender, EventArgs args)
        {
            DestroyRewardedAd();
            RequestRewardedAd();
        }

        public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            // Called when rewarded ad failed to show.

            // Clear resources after an ad dismissed.
            DestroyRewardedAd();

            // Now you can preload the next rewarded ad.
            RequestRewardedAd();
        }

        public void HandleRewarded(object sender, Reward args)
        {
            // Called when the user can be rewarded with {args.type} and {args.amount}.
        }

        public void DestroyRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
        }

        #endregion
    }
}
