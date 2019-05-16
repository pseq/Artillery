using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIScript : MonoBehaviour
{
    public GameObject gun;
    GunScript gunScript;
    CanvasGroup canvasGr;
    public CanvasGroup canvasGrChild;
    public int framesAimUICheck;
    Quaternion lastGunRotation;

    void Start() {
        canvasGr = GetComponent<CanvasGroup>();
        gunScript = gun.GetComponent<GunScript>();
        lastGunRotation = gun.transform.rotation;
    }
    
    public void Reset() {
        transform.rotation = Quaternion.identity;
        gunScript.AimSectorUpdate();
    }

    void Update() {
        // пока все части прицельного интерфейса видимы и угол танка меняется - обновляем положение раз в framesAimUICheck кадров
        if (Time.frameCount % framesAimUICheck == 0 &&
            canvasGr.alpha > 0.01f                  &&
            canvasGrChild.alpha > 0.01f             &&
            lastGunRotation != gun.transform.rotation
            )
        {
            Reset();
            lastGunRotation = gun.transform.rotation;
        }
    }
}
