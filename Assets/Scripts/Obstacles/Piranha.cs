using UnityEngine;

public class Piranha : MonoBehaviour {

    Vector3 _middleWorldPosition;
    Vector3 _rightWorldPosition;
    Vector3 _bottomWorldPosition;

    Rigidbody2D _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        Vector3 spawnPosition = PickRandomSpawnPosition();
        transform.position = spawnPosition;
        Launch(5, 5);
    }

    (Vector3, Vector3) GetScreenWidthWorldPosition() {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 middleScreenPosition = new(Screen.width / 2, cameraPosition.y, cameraPosition.z);
        Vector3 rightScreenPosition = new(Screen.width, cameraPosition.y, cameraPosition.z);
            
        Vector3 middleWorldPosition = Camera.main.ScreenToWorldPoint(middleScreenPosition);
        Vector3 rightWorldPosition = Camera.main.ScreenToWorldPoint(rightScreenPosition);
        return (middleWorldPosition, rightWorldPosition);
    }

    Vector3 GetScreenBottomWorldPosition() {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 bottomScreenPosition = new(cameraPosition.x, 0, cameraPosition.z);
        Vector3 bottomWorldPosition = Camera.main.ScreenToWorldPoint(bottomScreenPosition);
        return bottomWorldPosition;
    }

    Vector3 PickRandomSpawnPosition() {
        (_middleWorldPosition, _rightWorldPosition) = GetScreenWidthWorldPosition();
        float randomXPosition = Random.Range(_middleWorldPosition.x, _rightWorldPosition.x);
        _bottomWorldPosition = GetScreenBottomWorldPosition();
        return new(randomXPosition, _bottomWorldPosition.y, 0);
    }

    void Launch(float height, float width) {
        float g = - Physics.gravity.y;
        float vy = Mathf.Sqrt(2 * g * height);
        float vx = width * Mathf.Sqrt(g / (8 * height));
        _rigidbody.linearVelocity = new(-vx, vy);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        Debug.Log("Player touched the piranha");
        // TODO: End run
    }
}
