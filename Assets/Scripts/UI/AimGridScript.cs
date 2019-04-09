using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGridScript : MonoBehaviour
{
    public Transform tank;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PositionUpdate()
    {
        transform.position = tank.position;
        transform.position = tank.position;
    }
}
