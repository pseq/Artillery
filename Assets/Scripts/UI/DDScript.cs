using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDScript : MonoBehaviour
{
    public GameObject poolObject;
    public GameObject gun;
    GunScript gunScript;
    PoolManagerScript poolManager;
    Dropdown dd;

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        dd = GetComponent<Dropdown>();
        poolManager = poolObject.gameObject.GetComponent<PoolManagerScript>();
        gunScript = gun.gameObject.GetComponent<GunScript>();
        //gunScript.SelectBullet(0);


//        Debug.Log("DD DdScript Started " + gameObject.name);
        //Sprite[] bulletIconsCatalog = poolManager.GetComponent<bulletIconsCatalog>();
        /////////////////////////////
        //List<string> m_DropOptions = new List<string> { "Option 1", "Option 2"};
        //m_Dropdown.ClearOptions();

        //m_Dropdown.options.RemoveAt(0);
    }

    public void CreateDDList(int[] bulletsKeys, int[] bulletsNumbers) {
        dd.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for(int i = 0; i < bulletsKeys.Length; i ++) {
//            Debug.Log("DD NewItem " + i + " Count " + bulletsNumbers[i] + " Icon " + poolManager.GetBulletIcon(i));
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
            //Debug.Log("DD ddval minus " + dd.value);
        }
        else {

            dd.options.RemoveAt(dd.value);
            dd.value = 0;
            dd.RefreshShownValue();
            //Debug.Log("DD dd.options " + dd.options);
            DropdownValueChanged();
        }
    }
}
