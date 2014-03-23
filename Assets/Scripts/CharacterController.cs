using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	public bool canJump = true;
	public float jumpStrength = 30;

	public float moveSpeed = 10f;			// movespeed of the player

	public float sensitivityX = 15F;		// mouse sensitivity

	void Start ()
	{
		Screen.lockCursor = true;
	}

	void Update ()
	{
		// Get mouse input and move the camera rotation
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		
		transform.localEulerAngles = new Vector3(0, rotationX, 0);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		// Take keyboard input WSAD
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		if (!canJump)
		{
			h = 0;
			v = 0;
		}

		// Create a vector and populate it with the movement input
		Vector3 force = (transform.forward * v) + (transform.right * h);

		force.Normalize();

		if (Input.GetButtonDown ("Jump") && canJump)
		{
			rigidbody.velocity += Vector3.up * jumpStrength;
		}
		
		rigidbody.AddForce(force * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

		if (Physics.Raycast (transform.position, (Vector3.up*-1), 1.2f))
		{
			canJump = true;
			rigidbody.drag = 4;
		}

		else
		{
			canJump = false;
			rigidbody.drag = 0;
			rigidbody.velocity -= Vector3.up * 0.3f;
		}
	}
}
