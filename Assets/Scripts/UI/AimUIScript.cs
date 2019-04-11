using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIScript : MonoBehaviour
{
    public GunScript gun;

    public void Reset() {
//        transform.localRotation = tank.rotation;
        transform.rotation = Quaternion.identity;
        gun.AimSectorUpdate();
    }
}
