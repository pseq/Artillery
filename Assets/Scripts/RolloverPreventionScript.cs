using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RolloverPreventionScript : MonoBehaviour
{
    //public Text testText0;
    //public Text testText1;
    public float framesRollingoverCheck;
    public float framesUpsideCheck;
    public float critAngle;
    public float restAngle;
    public float rollbackImpulse;
    float oldAngle;

    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // периодически проверяем, не перевернулся ли танк
        if (Time.frameCount % framesRollingoverCheck == 0) {
	    UpsideCheck();
	    }

        if (Time.frameCount % framesUpsideCheck == 0) {
        oldAngle = transform.eulerAngles.z;
	    }
    }

    void UpsideCheck() {
        //testText0.text = "DeltaAngle = " + Mathf.Round(Mathf.DeltaAngle(0, transform.eulerAngles.z)).ToString();
        //testText1.text = "anVelocity = " + (rigid.angularVelocity).ToString();
        float deltaAngle = Mathf.DeltaAngle(0, transform.eulerAngles.z);
        float sign = Mathf.Sign(deltaAngle);

        if (deltaAngle > 90 - critAngle || deltaAngle < -90 - critAngle) 
            {
                //testText0.color = Color.red;
                // если танк перевернут - дать ему обратный импульс
                rigid.AddTorque(-sign * rollbackImpulse, ForceMode2D.Impulse);
                // а если перевернут и лежит - запустить анимацию переворота
                if (Mathf.Abs(Mathf.DeltaAngle(oldAngle, transform.eulerAngles.z)) < restAngle) GetComponent<TankAIScript>().UpsideDown();
            }
            //else 
            //testText0.color = Color.black;
    }
}
