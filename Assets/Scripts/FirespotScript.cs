using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirespotScript : MonoBehaviour
{

    float shootPower;

    public void SetShootPower(float power) {
        shootPower = power;
    }

    public float GetShootPower() {
        return shootPower;
    }
}
