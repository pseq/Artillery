using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUIScript : MonoBehaviour
{
    public GunScript gun;
    CanvasGroup canvasGr;
    public CanvasGroup canvasGrChild;

    void Start() {
        canvasGr = GetComponent<CanvasGroup>();
    }
    
    public void Reset() {
        transform.rotation = Quaternion.identity;
        gun.AimSectorUpdate();
    }

    void Update() {
        // пока все части прицельного интерфейса видимы - обновляем положение
        if (canvasGr.alpha > 0.01f && canvasGrChild.alpha > 0.01f) Reset();
    }
}
