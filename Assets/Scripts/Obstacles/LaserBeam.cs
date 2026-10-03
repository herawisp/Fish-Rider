using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
    
    public Transform LaserHintPrefab;
    public Transform LaserHitPrefab;
    public float HintDuration;
    public float LaserDuration;

    Vector3 _topWorldPosition;
    Vector3 _bottomWorldPosition;
    Transform _laserHint;
    Transform _laserHit;

    void Start()
    {
        Generate();
    }
    public void Generate() {
        StartCoroutine(OnGenerate());
    }

    IEnumerator OnGenerate() {
        Vector3 spawnPosition = PickRandomSpawnPosition();

        if (_laserHint == null) {
            _laserHint = Instantiate(LaserHintPrefab, spawnPosition, Quaternion.identity);
            _laserHint.SetParent(transform);
        } else {
            _laserHint.gameObject.SetActive(true);
            _laserHint.position = spawnPosition;
        }

        yield return new WaitForSeconds(HintDuration);
        _laserHint.gameObject.SetActive(false);

        if (_laserHit == null) {
            _laserHit = Instantiate(LaserHitPrefab, spawnPosition, Quaternion.identity);
            _laserHit.SetParent(transform);
        } else {
            _laserHit.gameObject.SetActive(true);
            _laserHit.position = spawnPosition;
        }

        yield return new WaitForSeconds(LaserDuration);
        _laserHit.gameObject.SetActive(false);
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
}
