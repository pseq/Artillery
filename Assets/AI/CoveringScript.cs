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
        controlPoint = transform.position; 
        int i = 0;
        while(controlPoint.x < right && i < terr.length) {
	        do {
		        i++;
		        } while (i < terr.length && Vector2.Distance(controlPoint, terr[i]) < wheelBase);
    	        if (GetTerrainAngle(controlPoint, terr[i]) > maxTrackAngle) break;
	            }
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
