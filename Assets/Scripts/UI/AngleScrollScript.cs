using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleScrollScript : MonoBehaviour {

    ScrollRect scRect;
    float lastPosition;
    AudioSource tick;
    public float tickStep;

    // Use this for initialization
    void Start () {
        scRect = gameObject.GetComponent<ScrollRect>();
        lastPosition = scRect.verticalNormalizedPosition;
        tick = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayTick()
    {
        Debug.Log("last:"+lastPosition + " cur:" + scRect.verticalNormalizedPosition + " delta:" + Mathf.Abs(lastPosition - scRect.verticalNormalizedPosition));
        if (Mathf.Abs(lastPosition - scRect.verticalNormalizedPosition) > tickStep)
        {
            tick.Play();
            lastPosition = scRect.verticalNormalizedPosition;
        }
    }
}
