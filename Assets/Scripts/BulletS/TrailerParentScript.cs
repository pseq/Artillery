using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerParentScript : MonoBehaviour
{
    public GameObject trailer_pref;
    GameObject trailer;
    
    void OnEnable() {
        TrailerOn();
    }
    void Start()
    {
        trailer = Instantiate(trailer_pref);
        
    }
    void TrailerOn() {
        if (trailer) {
            trailer.transform.SetParent(transform);
            trailer.transform.localPosition = Vector3.zero;
            trailer.gameObject.SetActive(true);
        }
    }

    public void TrailerOff() {
        if (trailer) {
            trailer.transform.SetParent(null);
        }    
    }

}
