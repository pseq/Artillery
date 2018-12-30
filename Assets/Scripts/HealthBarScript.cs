using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        if (transform.parent.localScale.x < 0) transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1f,1f,1f));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = startRotation;
    }
}
