
using System;
using UnityEngine;

public class CurrencyService : MonoBehaviour {
    
    //==============================================================================================//
    //==============================================================================================//

    public event Action OnFishCoinChanged;

    //==============================================================================================//
    //==============================================================================================//
    
    public static CurrencyService Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //==============================================================================================//
    //==============================================================================================//

    public void AdjustFishCoin(int amount) {
        DataService.Instance.Data.FishCoins += amount;
        OnFishCoinChanged?.Invoke();
        Debug.Log("COIN AMOUNT: " + DataService.Instance.Data.FishCoins);
    }

    public bool HaveEnoughFishCoins(int amount) {
        return DataService.Instance.Data.FishCoins >= amount;
    }

    //==============================================================================================//
    //==============================================================================================//
}