using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    TrailRenderer trail;
    public int framesTrailLengthCheck;
    
    // Start is called before the first frame update
    void Start()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // деактивируем трейл при нулевой длине
        //Debug.Log("trailer count " + trail.positionCount);
        if (Time.frameCount % framesTrailLengthCheck == 0)
            if (trail.positionCount == 0) {
                trail.Clear();
                gameObject.SetActive(false);
            }
    }
}
