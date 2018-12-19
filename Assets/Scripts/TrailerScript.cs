using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    TrailRenderer trail;
    
    // Start is called before the first frame update
    void Start()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // деактивируем трейл при нулевой длине
        if (trail.positionCount == 0) gameObject.SetActive(false);
    }
}
