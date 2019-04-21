using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirespotScript : MonoBehaviour
{

    Vector2 shootParams;

    public void SetShootParams(Vector2 sPrms) {
        shootParams = sPrms;
    }

    public Vector2 GetShootParams() {
        return shootParams;
    }
}
