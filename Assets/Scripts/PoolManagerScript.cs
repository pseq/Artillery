using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerScript : MonoBehaviour {

	public Dictionary<int,GameObject> pool;
	public GameObject[] bulletsCatalog;
	public Sprite[] bulletIconsCatalog;

	void Start () {
		pool = new Dictionary<int, GameObject>();
		PoolFillingStart();
	}

	public Sprite GetBulletIcon(int key) {
		return bulletIconsCatalog[key];
	}

	void PoolFillingStart() {
		//ищем все танки, заполняем и просматриваем их арсеналы
		GameObject[] guns = GameObject.FindGameObjectsWithTag("gun");
		foreach(GameObject gun in guns) {
			GunScript gunScript = gun.GetComponent<GunScript>();
			int[] arsenalKeys = gunScript.MakeArsenal();
			// просматриваем арсенал, и если снаряда ещё нет в пуле, добавляем его в пул
			foreach(int arsKey in arsenalKeys) {
				if (!pool.ContainsKey(arsKey)) {
					GameObject newBullet = Instantiate(bulletsCatalog[arsKey]);
					pool.Add(arsKey, newBullet);
				} 
			}
			// выбираем стартовый тип снаряда
			gunScript.SelectBullet(0);
		}
	}

	public GameObject GetFromPool(int key) {
		return pool[key];
	}
}
