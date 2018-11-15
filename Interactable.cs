
using UnityEngine;

public class Interactable : MonoBehaviour {

	public float radius = 1f;//how close we need to be
	public GameObject sword;

//
//	void OnDrawGizmosSelected(){
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawWireSphere (transform.position, radius);
//
//	}
//

	private void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			
			Debug.Log ("player entered");
			Transform bone = other.GetComponentInParent<Animator>().GetBoneTransform(HumanBodyBones.RightHand); 
			sword.transform.parent = bone; 
			sword.transform.localPosition = new Vector3(0,0,0);
			sword.transform.localRotation = Quaternion.identity;

		} else {

		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
