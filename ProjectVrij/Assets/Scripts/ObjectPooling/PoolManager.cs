using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

	Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

	static PoolManager _instance;
	public static PoolManager instance {
		get{
			if (_instance == null) {
				_instance = FindObjectOfType<PoolManager>();
			} 
			return _instance;
		}
	}
	public void CreatePool(GameObject prefab, int size) {
		int key = prefab.GetInstanceID ();

		GameObject group = new GameObject(prefab.name + " pool");
		group.transform.parent = transform;

		if (!poolDictionary.ContainsKey(key)) {
			poolDictionary.Add(key, new Queue<ObjectInstance>());

			for (int i = 0; i < size; i ++) {
				ObjectInstance newObject = new ObjectInstance(Instantiate (prefab) as GameObject);
				poolDictionary[key].Enqueue(newObject);
				newObject.SetParent(group.transform);
			
			}
		}
	}
	public GameObject ReuseObject(GameObject prefab, Vector3 pos, Quaternion rot) {
		int key = prefab.GetInstanceID();
 
		if (poolDictionary.ContainsKey(key)) {
			ObjectInstance obj = poolDictionary[key].Dequeue();
			poolDictionary[key].Enqueue(obj);
			obj.Reuse(pos, rot);
			return obj.gameObject;
		}
		return null;
	}

	public class ObjectInstance {
		public GameObject gameObject;
		Transform transform;
		bool hasPoolComponent;
		PoolObject poolObjectScript;

		public ObjectInstance(GameObject objectInstance) {
			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive(false);
			if (gameObject.GetComponent<PoolObject>()) {
				poolObjectScript = gameObject.GetComponent<PoolObject>();
			}
		}
		public void Reuse(Vector3 pos, Quaternion rot) {
			if (gameObject.GetComponent<PoolObject>()) {
				poolObjectScript.OnObjectReuse();
			}
			gameObject.SetActive(true);
			transform.position = pos;
			transform.rotation = rot;
		}
		public void SetParent(Transform parent){
			transform.parent = parent;
		}
	}
}
