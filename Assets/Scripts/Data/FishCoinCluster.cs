using UnityEngine;

public class FishCoinCluster : MonoBehaviour {
    
    Rigidbody2D _rigidBody2D;

    void Awake() {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Generate() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }

        transform.position = PickRandomSpawnPosition();
        _rigidBody2D.linearVelocityX = - ContentSpawner.Instance.Speed;
    }


    (Vector3, Vector3) GetScreenHeightWorldPosition() {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 topScreenPosition = new(cameraPosition.x, Screen.height, cameraPosition.z);
        Vector3 bottomScreenPosition = new(cameraPosition.x, 0, cameraPosition.z);
        
        Vector3 topWorldPosition = Camera.main.ScreenToWorldPoint(topScreenPosition);
        Vector3 bottomWorldPosition = Camera.main.ScreenToWorldPoint(bottomScreenPosition);
        return (topWorldPosition, bottomWorldPosition);
    }

    Vector3 PickRandomSpawnPosition() {
        (Vector3 _topWorldPosition, Vector3 _bottomWorldPosition) = GetScreenHeightWorldPosition();
        float randomYPosition = Random.Range(_bottomWorldPosition.y, _topWorldPosition.y);
        return new(20, randomYPosition, transform.position.z);
    }
}
