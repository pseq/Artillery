using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirespotScript : MonoBehaviour
{

    Vector2 shootParams;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShootParams(Vector2 sPrms) {
        shootParams = sPrms;
    }

    public Vector2 GetShootParams() {
        return shootParams;
    }
}
