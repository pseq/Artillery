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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PoolFillingStart() {
		//ищем все танки и просматриваем их арсеналы
		GameObject[] guns = GameObject.FindGameObjectsWithTag("tank");
		foreach(GameObject gun in guns) {
		int[] arsenalKeys = gun.GetComponent<GunScript>().GetArsenalKeys();
		// просматриваем арсенал, и если снаряда ещё нет в пуле, добавляем его ключ в пул
		foreach(int arsKey in arsenalKeys) {
				if (!pool.ContainsKey(arsKey)) pool.Add(arsKey, null);
			}
		}
		// сейчас у нас пул с одними уникальными ключами
		int[] poolKeys = new int[pool.Count];
		pool.Keys.CopyTo(poolKeys,0);
		foreach(int poolKey in poolKeys) {

		}
	}

	public void addToPool(int poolKey, GameObject poolValue) {

	}
}
