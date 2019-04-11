using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetTrigger("ShowOnStart");
    }
}
