using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDScript : MonoBehaviour
{
    public GameObject poolObject;
    //public GameObject gun;
    public GameObject tank;
    GunScript gunScript;
    PoolManagerScript poolManager;
    Dropdown dd;

    // Start is called before the first frame update
    void Start()
    {
        dd = GetComponent<Dropdown>();
        poolManager = poolObject.gameObject.GetComponent<PoolManagerScript>();
        //gunScript = gun.gameObject.GetComponent<GunScript>();
        gunScript = tank.GetComponent<TankScript>().GetGun();
    }

    public void CreateDDList(int[] bulletsKeys, int[] bulletsNumbers) {
        dd.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for(int i = 0; i < bulletsKeys.Length; i ++) {
            Dropdown.OptionData newOption = new Dropdown.OptionData(bulletsNumbers[i].ToString(), poolManager.GetBulletIcon(bulletsKeys[i]));
            options.Add(newOption);
        }
        dd.AddOptions(options);
    }

    public void DropdownValueChanged() {
        gunScript.SelectBullet(dd.value);
    }

    public void SetCurrentBulletCount(int currentCount) {
        if (currentCount > 0) {
            dd.options[dd.value].text = currentCount.ToString();
            dd.RefreshShownValue();
        }
        else {

            dd.options.RemoveAt(dd.value);
            dd.value = 0;
            dd.RefreshShownValue();
            DropdownValueChanged();
        }
    }
}
