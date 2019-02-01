using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUpsideDownScript : MonoBehaviour
{

public float framesUpsideCheck = 10;
public float upsideReturnUp = 5;
public float upsideReturnDamage = 10;
public int blinks = 8;
Rigidbody2D rigid;
SpriteRenderer tankSprite;

Vector2 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        tankSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % framesUpsideCheck == 0) {
	    UpsideCheck();
	}
}

void UpsideCheck() {

    // test stop
	if (Mathf.Abs(Mathf.DeltaAngle(0, transform.eulerAngles.z)) > 90) {// upsidedown
    Vector3 scl = transform.localScale;
	lastPosition = transform.position;

    rigid.velocity = Vector2.zero;
    rigid.angularVelocity = 0f;
	StartCoroutine(TankBlink(scl));
    // TODO DAMAGE
    
    }
}

IEnumerator TankBlink(Vector3 scl) {
    
    rigid.constraints = RigidbodyConstraints2D.FreezeAll;
    for (int i = 0; i <= blinks; i++) {
        if (transform.localScale == Vector3.zero) transform.localScale = scl;
        else transform.localScale = Vector3.zero;
        // на половине блинков перемещаем танк на исходную позицию и чуть выше
        if (i == blinks/2) TankMoveBack();
        yield return new WaitForSeconds(.08f);
    }
    rigid.constraints = RigidbodyConstraints2D.None;
    transform.localScale = scl;
}

void TankMoveBack() {
	transform.position = lastPosition + Vector2.up * upsideReturnUp;
    transform.eulerAngles = Vector2.zero;
}

}
