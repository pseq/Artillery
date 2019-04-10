using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIScript : MonoBehaviour
{
    public Transform tank;
    public GunScript gun;

    // Start is called before the first frame update
    void Start()
    {
        //Canvas canvas = GetComponent<Canvas>();
        //canvas.worldCamera = Camera.main;
    }

    public void On() {
        //GetComponent<Animator>().Play("On");
    }

    public void Off() {
        //GetComponent<Animator>().Play("Off");
    }

    public void Reset() {
//        transform.localRotation = tank.rotation;
        transform.rotation = Quaternion.identity;
        gun.AimSectorUpdate();
    }
}
