using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public List<GameObject> prefabs;
    private Queue<GameObject> pool = new();

    void Start() {
        if (prefabs.Count == 1) return;
        foreach (GameObject prefab in prefabs) {
            GameObject gameObject = Instantiate(prefab);
            ReturnObject(gameObject);
        }
    }

    public GameObject GetObject() {
        if (pool.Count > 0) {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(PickRandomPrefab());
    }

    public void ReturnObject(GameObject obj) {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public IEnumerator ReturnLater(GameObject obj, float time) {
        yield return new WaitForSeconds(time);
        ReturnObject(obj);
    }

    GameObject PickRandomPrefab() {
        if (prefabs.Count == 1) return prefabs[0];
        return prefabs[Random.Range(0, prefabs.Count - 1)];
    }

}
