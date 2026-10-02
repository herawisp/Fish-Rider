using UnityEngine;

public class WaterJets : MonoBehaviour {

    public float MinimumLength;
    public float MaximumLength;

    public Transform WaterJetAnchorPrefab;

    Vector3 _topWorldPosition;
    Vector3 _bottomWorldPosition;
    Vector3 _spawnPosition;
    Vector3 _endPosition;
    Vector3 _direction;
    Transform _anchor1;
    Transform _anchor2;

    public void Generate(Transform parent) {
        _spawnPosition = PickRandomSpawnPosition();
        _direction = GetRandomDirection();
        _endPosition = _spawnPosition + _direction;
        
        _anchor1 = Instantiate(WaterJetAnchorPrefab, _spawnPosition, Quaternion.identity);
        _anchor2 = Instantiate(WaterJetAnchorPrefab, _endPosition, Quaternion.identity);
        _anchor1.SetParent(parent);
        _anchor2.SetParent(parent);

        ConnectAnchor();
        transform.SetParent(parent);
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
        (_topWorldPosition, _bottomWorldPosition) = GetScreenHeightWorldPosition();
        float randomYPosition = Random.Range(_bottomWorldPosition.y, _topWorldPosition.y);
        return new(0, randomYPosition, 0);
    }

    bool IsOutOfBounds(Vector3 position) {
        return (position.y > _topWorldPosition.y) || (position.y < _bottomWorldPosition.y);
    }

    Vector3 GetRandomDirection() {
        Vector3 direction;
        do {
            direction = Random.insideUnitCircle.normalized;
            float length = Random.Range(MinimumLength, MaximumLength);
            direction *= length;
        } while (IsOutOfBounds(_spawnPosition + direction));
        return direction;
    }

    void ConnectAnchor() {
        Vector3 midPoint = (_spawnPosition + _endPosition) / 2;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        Quaternion quaternion = Quaternion.Euler(0, 0, angle);

        transform.position = midPoint;
        transform.rotation = quaternion;
        transform.localScale = new(_direction.magnitude, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") return;
        Debug.Log("Player touched the water jet");
        // TODO: End run
    }

    void OnDestroy() {
        Destroy(_anchor1);
        Destroy(_anchor2);    
    }
}
