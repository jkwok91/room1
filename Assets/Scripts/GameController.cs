using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public float speed;
	public float rspeed;

	//private Rigidbody rb;
	private Camera cam;
	private LayerMask grabbableMask;
	private float withinGrab;

	private GameObject heldObj;

	public float mouseSensitivity;
	public float clampAngle;

	private float rotY;
	private float rotX;

	void Start ()
	{
		Vector3 rot = transform.localRotation.eulerAngles; // hmmmmmm
		rotY = rot.y; // current local rotations
		rotX = rot.x;

		heldObj = null;
		cam = (transform.FindChild ("camera")).GetComponent<Camera> ();
		//rb = GetComponent<Rigidbody> ();
		grabbableMask = LayerMask.GetMask ("Grabbable");
		withinGrab = 1f;
	}

	void Update ()
	{
//		if (Input.GetMouseButtonDown (0)) {
//			TouchStuff ();
//		}
//		if (Input.GetMouseButton(0)) {
//			// idfk? move the thing around
//			Vector3 t = Input.mousePosition - heldObj.transform.position; // nope this doesnt work
//			DragStuff(t.normalized);
//		}
//		if (Input.GetMouseButtonUp (0)) {
//			ReleaseStuff ();
//		}
	}

	// i'm using LateUpdate because... this is a camera? is that a valid reason
	void LateUpdate ()
	{
		Turn ();
		Move ();
	}

	void Move () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (h, 0f, v);

		transform.Translate (movement*speed*Time.deltaTime);
		transform.position = Vector3.ProjectOnPlane (transform.position, Vector3.up); // i'm not happy about this.
	}

	void Turn ()
	{
		float mouseX = Input.GetAxis ("Mouse X"); // how much your mouse has moved
		float mouseY = Input.GetAxis ("Mouse Y"); // the opposite idfk man people have diff preferences for looking up and down i think??? we'll see

		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		if (!screenRect.Contains(Input.mousePosition))
			return; // thanks, the internet

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += -mouseY * mouseSensitivity * Time.deltaTime;

		//Debug.Log ("rotX is now " + rotX);
		rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle/2); // hmm
		//Debug.Log ("afterClamping rotX is now " + rotX);


		Quaternion localRotation = Quaternion.Euler (rotX, rotY, 0f);
		transform.rotation = localRotation;
	}

	void TouchStuff ()
	{
		Ray camRay = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (camRay, out hit, Mathf.Infinity, grabbableMask)) {
			//Vector3 playerToMouse = hit.point - transform.position;

			if (hit.rigidbody != null) {
				//Debug.Log ("hit " + hit.transform.name + " and pushing");
				heldObj = hit.collider.gameObject;
				//hit.rigidbody.AddForce (camRay.direction*100f); //hmm
			}
		}
	}

	void DragStuff (Vector3 t)
	{
		if (heldObj) {
			heldObj.transform.Translate (t);
		}
	}

	void ReleaseStuff ()
	{
		if (heldObj) {
			heldObj = null;
		}
	}
}
