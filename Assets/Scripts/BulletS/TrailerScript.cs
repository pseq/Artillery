using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    TrailRenderer trail;
    public int framesTrailLengthCheck;
    float enableTime;
    
    // Start is called before the first frame update
    void Start()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
        gameObject.SetActive(false);
    }

    void OnEnable() {
        // время активации трейла
        enableTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // если время после активации больше время излучения трейла
        if (Time.time - enableTime > trail.time) {
        // то деактивируем трейл при нулевой длине
            if (trail.positionCount == 0) {
                trail.Clear();
                gameObject.SetActive(false);
            }
        }
    }
}
