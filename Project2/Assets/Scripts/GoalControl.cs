using UnityEngine;
using System.Collections;

public class GoalControl : MonoBehaviour {

	private Animation anim;
	private float smooth;
	private float scaler;
	private Vector3 velocity;
	private float walkingSpeed;
	private Vector3 previousValidPos;
	
	void Start()
	{
		smooth = 5.0f;
		scaler = 25.0f;
		walkingSpeed = 5.0f;
		velocity = Vector3.zero;
		anim = GetComponent<Animation> ();
		anim.CrossFade ("soldierIdleRelaxed");
		previousValidPos = transform.position;
	}
	void Update()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		velocity = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		velocity = velocity * scaler;
		Vector3 targetPosition = transform.position + velocity * Time.deltaTime;
		if (velocity != new Vector3())
			RotateTo (targetPosition);
		previousValidPos = transform.position;
		transform.position += velocity * Time.deltaTime;
		doAnimation ();
		
	}

	void doAnimation(){
		float mag = velocity.magnitude;
		if (mag > 0.0f && mag <= walkingSpeed) {
			anim.CrossFade ("soldierWalk");
		} else if (mag > walkingSpeed) {
			anim.CrossFade ("soldierRun");
		} else {
			anim.CrossFade ("soldierIdleRelaxed");
		}
	}

	void RotateTo(Vector3 targetPosition){
		//maxDistance is the maximum ray distance
		Quaternion destinationRotation;
		Vector3 relativePosition;
		relativePosition = targetPosition - transform.position;
		//		Debug.DrawRay(transform.position,relativePosition*10,Color.red);
		//		Debug.DrawRay(transform.position,velocity.normalized*20,Color.green);
		//		Debug.DrawRay(transform.position,acceleration.normalized*10,Color.blue);
		destinationRotation = Quaternion.LookRotation (relativePosition);
		transform.rotation = Quaternion.Slerp (transform.rotation, destinationRotation, Time.deltaTime * smooth);
	}

	void OnCollisionEnter(Collision col){
		Debug.Log (col.gameObject.name);
		transform.position = previousValidPos;
	}
}
