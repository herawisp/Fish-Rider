using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject prefab;
    private Queue<GameObject> pool = new();

    public GameObject GetObject() {
        if (pool.Count > 0) {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(prefab);
    }

    public void ReturnObject(GameObject obj) {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public IEnumerator ReturnLater(GameObject obj, float time) {
        yield return new WaitForSeconds(time);
        ReturnObject(obj);
    }
}
