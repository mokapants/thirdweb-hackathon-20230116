using System;
using UnityEngine;
using Thirdweb;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;

[System.Serializable]
public enum Chain
{
    Ethereum = 1,
    Goerli = 5,
    Polygon = 137,
    Mumbai = 80001,
    Fantom = 250,
    FantomTestnet = 4002,
    Avalanche = 43114,
    AvalancheTestnet = 43113,
    Optimism = 10,
    OptimismGoerli = 420,
    Arbitrum = 42161,
    ArbitrumGoerli = 421613,
    Binance = 56,
    BinanceTestnet = 97
}

public class ThirdwebManager : MonoBehaviour
{
    [Header("SETTINGS")] public Chain chain = Chain.Goerli;
    public List<Chain> supportedNetworks;

    public Dictionary<Chain, string> chainIdentifiers = new Dictionary<Chain, string>
    {
        { Chain.Ethereum, "ethereum" },
        { Chain.Goerli, "goerli" },
        { Chain.Polygon, "polygon" },
        { Chain.Mumbai, "mumbai" },
        { Chain.Fantom, "fantom" },
        { Chain.FantomTestnet, "testnet" },
        { Chain.Avalanche, "avalanche" },
        { Chain.AvalancheTestnet, "avalanche-testnet" },
        { Chain.Optimism, "optimism" },
        { Chain.OptimismGoerli, "optimism-goerli" },
        { Chain.Arbitrum, "arbitrum" },
        { Chain.ArbitrumGoerli, "arbitrum-goerli" },
        { Chain.Binance, "binance" },
        { Chain.BinanceTestnet, "binance-testnet" },
    };

    public ThirdwebSDK SDK;

    private ReactiveProperty<bool> isConnectWallet;

    public static ThirdwebManager Instance;

    public IReadOnlyReactiveProperty<bool> IsConnectWalletObservable => isConnectWallet;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        isConnectWallet = new ReactiveProperty<bool>();

#if !UNITY_EDITOR
        SDK = new ThirdwebSDK(chainIdentifiers[chain]);
#endif
    }

    private void Start()
    {
        IntervalWalletConnectState();
    }

    /// <summary>
    /// ウォレットの接続状況を定期的に確認する
    /// </summary>
    private void IntervalWalletConnectState()
    {
        // 定期的にウォレットの接続状況を確認
        Observable.Interval(TimeSpan.FromSeconds(2f)).Subscribe(_ => CheckWalletConnectStateAsync().Forget()).AddTo(this);
    }

    /// <summary>
    /// ウォレットの接続状況を更新
    /// </summary>
    private async UniTask CheckWalletConnectStateAsync()
    {
        if (Instance.SDK == null) return;
        isConnectWallet.Value = await Instance.SDK.wallet.IsConnected();
    }
}