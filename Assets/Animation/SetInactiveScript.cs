using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactiveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
