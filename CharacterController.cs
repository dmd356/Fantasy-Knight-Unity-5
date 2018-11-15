using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterController : MonoBehaviour {

	[System.Serializable]
	public class MoveSettings
	{
		public float walkSpeed = 2f;
		public float runSpeed = 6f;
		public float jumpVel = 10f;
		public float rotateVel = 300;
		public float turnSmoothTime = .2f;
		public float turnSmoothVelocity;
		public float speedSmoothTime = 0.2f;
		public float speedSmoothVelocity;
		public float currentSpeed;
		public bool isRunning = false;
		public bool isWalking = false;
		public float distToGround = 0.1f;//isGrounded
		public LayerMask ground;


	}

	[System.Serializable]
	public class PhysSettings
	{
		public float inputDelay = 0.1f;
		public float downwardAcceleration = 0.75f;
	}

	[System.Serializable]
	public class InputSettings
	{
		public string FORWARD_AXIS = "Vertical";
		public string TURN_AXIS = "Horizontal";
	}
	public MoveSettings moveSettings = new MoveSettings ();
	public PhysSettings physSettings = new PhysSettings();
	public InputSettings inputSettings = new InputSettings();
	public GameObject autoJump;
	public GameObject AutoCatchFall;

	Vector3 velocity = Vector3.zero;
	Quaternion targetRotation;
	Rigidbody rBody;
	float forwardInput, turnInput, jumpInput;

	Animator animator;


	public Quaternion TargetRotation {
		get { return targetRotation; }

	}

	bool isGrounded(Transform transform){
		return Physics.Raycast (transform.position, Vector3.down, moveSettings.distToGround, moveSettings.ground);
	}

	void Start()
	{
		
		targetRotation = transform.rotation;
		if (GetComponent<Rigidbody> ()) {
			rBody = GetComponent<Rigidbody> ();
		} else {
			Debug.LogError ("please add a rigid body");
		}
		forwardInput = turnInput = 0;	
		animator = GetComponent<Animator> ();
	}

	void GetInput()
	{
		forwardInput = Input.GetAxis (inputSettings.FORWARD_AXIS); //getAxis returns vallue -1 to 1
		turnInput =  Input.GetAxis (inputSettings.TURN_AXIS); //returns value 
		//jumpInput =  Input.GetAxisRaw (inputSettings.JUMP_AXIS); //returns value 
	}

	void Update()
	{
		GetInput ();
		Turn ();
	}

	void FixedUpdate(){
		Run ();
		AutoJump ();
		rBody.velocity = transform.TransformDirection (velocity);
	}

	void Run(){
		moveSettings.runSpeed = 6f;
		moveSettings.walkSpeed = 2f;
		if(Mathf.Abs(forwardInput)>physSettings.inputDelay){
			//move
			moveSettings.isRunning = Input.GetKey (KeyCode.LeftShift) && moveSettings.isWalking;
			moveSettings.isWalking = Mathf.Abs(forwardInput)>0f;
			Debug.Log ("is walking "+moveSettings.isWalking);
			InputController ();
			Debug.Log ("is walking "+moveSettings.isWalking);
			Debug.Log (velocity.z+ " and forward input "+forwardInput);
			velocity.z = ((moveSettings.isRunning) ? moveSettings.runSpeed : moveSettings.walkSpeed) * forwardInput;
		} 
		else{
			//zero velocity
			velocity.z = 0f;
			moveSettings.isRunning = false;
			moveSettings.isWalking = false;
			InputController ();
		}
	}
		

	void Turn(){
		if (Mathf.Abs (turnInput)>physSettings.inputDelay) {
			targetRotation *= Quaternion.AngleAxis (moveSettings.rotateVel * turnInput * Time.deltaTime, Vector3.up);
			transform.rotation = targetRotation;
		}
	}

	void AutoJump(){
		Transform targetAutoJump = autoJump.transform;

		if (!isGrounded (targetAutoJump) && isGrounded (transform) && moveSettings.isRunning) {
			velocity.y = moveSettings.jumpVel;
		} //target is not grounded, but player is and is running, so he jumps
		else if (!isGrounded (targetAutoJump) && isGrounded (transform) && moveSettings.isRunning) {
			velocity.y = moveSettings.jumpVel *.5f;
		}//target is not grounded, but player is and is walking, so he jumps less distance
		else if (isGrounded (targetAutoJump) && isGrounded (transform) && (moveSettings.isRunning||moveSettings.isWalking)) {
			velocity.y = 0f;
		}//target is ground and also transform, and is walking or running
		else {
			velocity.y -= physSettings.downwardAcceleration;
		}
	}



	void InputController(){
		bool isAttacking = Input.GetKey (KeyCode.Mouse0);
		bool isTargeting = Input.GetKey (KeyCode.Z);
		bool isFlippingBack = forwardInput<0;
		bool isShielding = Input.GetKey (KeyCode.Q);

		float animationSpeedPercent;
		if (moveSettings.isWalking) {
			if (moveSettings.isRunning) {
				if (isAttacking) {
					moveSettings.runSpeed = 0f;
					moveSettings.isWalking = false;
					animationSpeedPercent = .66f;//lunge stab
				} else {
					animationSpeedPercent = .33f;//run
				}
			} else if (isAttacking) {
				moveSettings.walkSpeed = 0f;
				moveSettings.isWalking = false;
				animationSpeedPercent = 1f;

			} else if (isTargeting) {
				if (isFlippingBack) {
					moveSettings.walkSpeed = 1.5f;
					moveSettings.isWalking = true;
					animationSpeedPercent = .88f;//backflip
				} else {
					animationSpeedPercent = .44f;//target walk
				}
			}
			else {

				if (isShielding) {
					moveSettings.walkSpeed = .5f;
					moveSettings.isWalking = true;
					animationSpeedPercent = .555f;
				}else{
					animationSpeedPercent = .22f; //Walk regular
					moveSettings.walkSpeed = 2f;
					moveSettings.isWalking = true;				
				}
			}
		} else {
			if (isAttacking) {
				animationSpeedPercent = 1f;
			} else {
				animationSpeedPercent = .1f;
			}
		}
		animator.SetFloat ("speedPercent", animationSpeedPercent, moveSettings.speedSmoothTime, Time.deltaTime);

	}




}
	
