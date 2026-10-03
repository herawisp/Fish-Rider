using UnityEngine;

public class FishCoin : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        CurrencyService.Instance.AdjustFishCoin(1);
        gameObject.SetActive(false);
    }
}
