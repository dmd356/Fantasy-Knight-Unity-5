using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	public float minDist = 1.0f;
	public float maxDist = 4.0f;
	public float smooth = 10.0f;
	Vector3 dollyDirection;
	public Vector3 dollyDirectionAdjusted;
	public float distance;


	void Awake(){
		dollyDirection = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}
	// Update is called once per frame
	void Update () {
		Vector3 desiredCamPos = transform.parent.TransformPoint(dollyDirection*maxDist);
		RaycastHit hit;
		if(Physics.Linecast(transform.parent.position, desiredCamPos, out hit)){
			distance = Mathf.Clamp((hit.distance*0.95f), minDist, maxDist);
		} else {
			distance = maxDist;
		}
		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime *smooth);
	}
}
