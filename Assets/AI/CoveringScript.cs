using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoveringScript : MonoBehaviour
{
    // NOT FORGET CHANGE
    public float wheelBase;
    TerrainScript terr;
    Vector3[] terrMap;
    public float maxTrackAngle = 45f;
    public float tankTop = 2f;
    LayerMask mask;

    
    // Start is called before the first frame update
    void Start()
    {
        wheelBase *= transform.localScale.x;
        mask = LayerMask.GetMask("Terrain");
    }

    float GetTerrainAngle(Vector2 a, Vector2 b) {
        // угол между точками
        float angle = Mathf.Atan2((b.y - a.y), (b.x - a.x)) * Mathf.Rad2Deg;
        return(angle);
    }

    Vector2 DropDown(float x) {
        // ищем высоту поверхности в точке
        //LayerMask mask = LayerMask.GetMask("Terrain");
        RaycastHit2D rkHit = Physics2D.Raycast(new Vector2(x, terr.maxY + 100), Vector2.down, terr.maxY + 100, mask);
        return(rkHit.point);
    }

    public void Test(int d) {
        GetCover((Direction)d);
    }

    //float Reachable(int side, Direction dir) {
    float Reachable(Direction dir) {
        float controlPoint = transform.position.x;

        // считаем границы
        float moveReserve = GetComponent<TankMoveScript>().GetTrackReserve();
        float terrLeft = terrMap[0].x;
        float terrRight = terrMap[terrMap.Length - 1].x;
        float left = Mathf.Clamp(transform.position.x - moveReserve, terrLeft, terrRight);
        float rigt = Mathf.Clamp(transform.position.x + moveReserve, terrLeft, terrRight);

        while(controlPoint < rigt && controlPoint > left) {
            // если угол участка слишком большой - это непроходимый участок
            //if (GetTerrainAngle(DropDown(controlPoint), DropDown(controlPoint + wheelBase)) * side > maxTrackAngle) break;
            if (GetTerrainAngle(DropDown(controlPoint), DropDown(controlPoint + wheelBase)) * (int)dir > maxTrackAngle) break;
            //controlPoint += wheelBase * side;
            controlPoint += wheelBase * (int)dir;
	    }
        return(controlPoint);
    }

    public float GetCover(Direction dir) {
        terr = GetComponent<TankScript>().terrainScript;
        terrMap = terr.GetMap();
        Vector2 enemy = GetComponent<TankScript>().target.transform.position;
        int i = terr.ClosestXindex(transform.position.x);
        // Reachable(side)
        while (
            !TestCover(enemy, terrMap[i], tankTop)  
            && i > 0 
            && i < terrMap.Length - 1
            //&& terrMap[i].x * side < Reachable(side) * side
            && terrMap[i].x * (int)dir < Reachable(dir) * (int)dir
            ) i += (int)dir;

    //DrawTestLine(Vector2.zero, terrMap[i]);

        // если не нашли укрытие - вернуть 0
        if(i == terrMap.Length - 1 || i == 0) return 0;
        else return(terrMap[i].x);
    }

    public bool TestCover(Vector2 enemy, Vector2 position, float top) {
        return(Physics2D.Linecast(position + Vector2.up * top, enemy, mask));
    }

    public void DrawTestLine(Vector2 p1, Vector2 p2) {
        Debug.DrawLine(p1, p2, Color.blue);
        Debug.Break();
    }
}
