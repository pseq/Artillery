public float framesUpsideCheck = 10;
public float upsideReturnUp = 10;
public float upsideReturnDamage = 10;
Vector2 lastPosition;

update
if (Time.frameCount % framesUpsideCheck == 0) {
	UpsideCheck();
	}
	
void UpsideCheck() {
	lastPosition = transform.position;
	if (Mathf.Abs(Mathf.DeltaAngle(0, transform.eulerAngles.z - angleSide)) > 90) // upsidedown
Debug.Log("UPSIDE DOWN");
	speed = 0;
	angularSpeed = 0;
	TankBlink();
	position = lastPosition + Vector2.Up * upsideReturnUp;
	TankBlink();
	}
