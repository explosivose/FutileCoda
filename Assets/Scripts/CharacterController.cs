using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float moveSpeed = 10f;			// movespeed of the player

	public float sensitivityX = 15F;		// mouse sensitivity
	public float sensitivityY = 15F;
	
	public float minimumY = -60F;			// locked Y axis view
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
		// Get mouse input and move the camera rotation
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		// Take keyboard input WSAD
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");


		// Create a vector and populate it with the movement input
		Vector3 force = (transform.forward * v) + (transform.right * h);
		force.Normalize();
		
		rigidbody.AddForce(force * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}
}
