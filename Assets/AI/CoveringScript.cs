using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoveringScript : MonoBehaviour
{
    // NOT FORGET CHANGE
    public float wheelBase;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float GetTerrainAngle() {
        return(0);
    }

    Vector2 GetReachableRegion() {
        // TODO make this
        return(GetFuelRegion());
    }

    Vector2 GetFuelRegion() {
        float tankX = transform.position.x;
        float targetX = GetComponent<TankScript>().target.transform.position.x;
        float moveReserve = GetComponent<TankMoveScript>().GetTrackReserve();
        int sign = (int)Mathf.Sign(tankX - targetX);
        float nearX = tankX - sign * moveReserve;
        float farX  = tankX + sign * moveReserve;;
        return(new Vector2(nearX, farX));
    }

    void CoversDef() {

    }

    public void GetNextCover() {

    }
}
