using UnityEngine;

public class Hazard : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        Debug.Log($"Player touched the {gameObject.name}");
        // TODO: End run
    }
}
