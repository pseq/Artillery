using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerScript : MonoBehaviour {

	public Dictionary<int,GameObject> pool;
	public GameObject[] bulletsCatalog;
	
	// Use this for initialization
	void Start () {
		// заменить словарт на массив из двухэлементных массивов
		pool = new Dictionary<int, GameObject>();
		PoolFillingStart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PoolFillingStart() {
		//ищем все танки и просматриваем их арсеналы
		GameObject[] guns = GameObject.FindGameObjectsWithTag("gun");
		foreach(GameObject gun in guns) {
		Debug.Log("What in arsenal? " + gun.name);
		int[] arsenalKeys = gun.GetComponent<GunScript>().GetArsenalKeys();
		Debug.Log("arsenalKeys " + arsenalKeys);
		// просматриваем арсенал, и если снаряда ещё нет в пуле, добавляем его в пул
		foreach(int arsKey in arsenalKeys) {
				if (!pool.ContainsKey(arsKey)) {
					GameObject newBullet = Instantiate(bulletsCatalog[arsKey]);
					newBullet.SetActive(false);
					pool.Add(arsKey, newBullet);
				} 
			}
		}

	}

	public void addToPool(int poolKey, GameObject poolValue) {

	}

	public GameObject GetFromPool(int key) {
		return pool[key];
	}
}
