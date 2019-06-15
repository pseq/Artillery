using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerScript : MonoBehaviour {

	public Dictionary<int,GameObject> pool;
	public GameObject[] bulletsCatalog;
    // для учёта окончания выстрела
    public int activeBulletsCounter  = 0;
    GameObject firingTank;

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
                    //newBullet.GetComponent<BulletScript>().SetPoolManager(this);
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

    public void IncreaseActiveBullets()
    {
        // ведём учет активных снарядов для отслеживания конца залпа
        activeBulletsCounter++;
    }

    public void DecreaseActiveBullets()
    {
        // ведём учет активных снарядов для отслеживания конца залпа
        activeBulletsCounter--;
        // когда активных снарядов не остаётся - залп завершён
        if (activeBulletsCounter < 1)
        {
            firingTank.GetComponent<TankAIScript>().ShootEnded();
            firingTank.GetComponent<TankScript>().SetLastHitPoint(transform.position.x);
        }
    }

    public void SetFiringTank(GameObject tank)
    {
        firingTank = tank;
    }
}
