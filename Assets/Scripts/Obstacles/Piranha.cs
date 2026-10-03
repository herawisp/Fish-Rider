using UnityEngine;

public class Piranha : MonoBehaviour {

    public float minimumWidth;
    public float maximumWidth;
    public float minimumHeight;
    public float maximumHeight;
    
    Vector3 _bottomWorldPosition;

    Rigidbody2D _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Generate() {
        Vector3 spawnPosition = PickRandomSpawnPosition();
        transform.position = spawnPosition;
        Launch();
    }

    Vector3 GetScreenBottomWorldPosition() {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 bottomScreenPosition = new(cameraPosition.x, 0, cameraPosition.z);
        Vector3 bottomWorldPosition = Camera.main.ScreenToWorldPoint(bottomScreenPosition);
        return bottomWorldPosition;
    }

    Vector3 PickRandomSpawnPosition() {
        float randomWidth = Random.Range(minimumWidth, maximumWidth);
        float randomXPosition = Movement.Instance.transform.position.x + randomWidth;
        _bottomWorldPosition = GetScreenBottomWorldPosition();
        return new(randomXPosition, _bottomWorldPosition.y, 0);
    }

    public void Launch() {
        float height = Random.Range(minimumHeight, maximumHeight);
        float width = Random.Range(minimumWidth, maximumWidth);

        float speedMultiplier = 2f;

        float g = - Physics2D.gravity.y;
        float vy = Mathf.Sqrt(2 * g * height);
        float vx = width * Mathf.Sqrt(g / (8 * height));
        _rigidbody.gravityScale = speedMultiplier * speedMultiplier;
        _rigidbody.linearVelocity = new Vector2(-vx, vy) * speedMultiplier;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        Debug.Log("Player touched the piranha");
        // TODO: End run
    }
}
