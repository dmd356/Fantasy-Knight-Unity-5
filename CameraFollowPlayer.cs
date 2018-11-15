using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {
	 

	public float cameraMovementSpeed = 120.0f;
	public GameObject cameraFollowsObject;
	Vector3 followPosition;
	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	public GameObject cameraOBJ;
	public GameObject playerOBJ;
	public float camDistanceXtoPlayer;
	public float camDistanceYtoPlayer;
	public float camDistanceZtoPlayer;
	public float mouseX;
	public float mouseY;
	public float finalInputX;
	public float finalInputZ;
	public float smoothX;
	public float smoothY;
	private float rotX = 0f;
	private float rotY= 0f;


	void Start () {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;//sets where the camera should be
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis ("RightStickHorizontal");
		float inputZ = Input.GetAxis ("RightStickVertical");
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = Input.GetAxis ("Mouse Y");
		finalInputX = inputX + mouseX;
		finalInputZ = inputZ + mouseY;

		rotY += finalInputX * inputSensitivity * Time.deltaTime;
		rotX += finalInputZ * inputSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(rotX, rotY,0.0f);
		transform.rotation = localRotation;


	}

	void LateUpdate(){
		CameraUpdater ();

	}

	void CameraUpdater(){
		//set target object to follow, in this case 
		Transform target = cameraFollowsObject.transform;
		//moves toward the target
		float step = cameraMovementSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}
}
