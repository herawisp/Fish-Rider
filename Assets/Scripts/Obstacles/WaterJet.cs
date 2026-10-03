using UnityEngine;

public class WaterJet : MonoBehaviour {

    public float MinimumLength;
    public float MaximumLength;

    public Transform WaterJetAnchorPrefab;
    public Transform WaterJetLinePrefab;

    Vector3 _topWorldPosition;
    Vector3 _bottomWorldPosition;
    Vector3 _spawnPosition;
    Vector3 _endPosition;
    Vector3 _direction;
    Transform _anchor1;
    Transform _anchor2;
    Transform _waterJetLine;
    Rigidbody2D _rigidBody2D;

    void Awake() {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Generate() {
        _spawnPosition = PickRandomSpawnPosition();
        _direction = GetRandomDirection();
        _endPosition = _spawnPosition + _direction;
        
        GenerateAnchors();
        ConnectAnchor();

        _rigidBody2D.linearVelocityX = - ContentSpawner.Instance.Speed;
    }

    void GenerateAnchors() {
        if (_anchor1 == null) {
            _anchor1 = Instantiate(WaterJetAnchorPrefab, _spawnPosition, Quaternion.identity);
            _anchor1.SetParent(transform);
        } else _anchor1.position = _spawnPosition;

        if (_anchor2 == null) {
            _anchor2 = Instantiate(WaterJetAnchorPrefab, _endPosition, Quaternion.identity);
            _anchor2.SetParent(transform);
        } else _anchor2.position = _endPosition;
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
        return new(transform.position.x, randomYPosition, transform.position.z);
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

        if (_waterJetLine == null) {
            _waterJetLine = Instantiate(WaterJetLinePrefab, midPoint, quaternion);
            _waterJetLine.SetParent(transform);
        } else {
            _waterJetLine.position = midPoint;
            _waterJetLine.rotation = quaternion;
        }
        _waterJetLine.localScale = new(_direction.magnitude, 1, 1);
    }
}
