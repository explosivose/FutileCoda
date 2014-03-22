using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	private float moveForce = 100f;			// added force to move the player
	private float moveSpeed = 10f;			// movespeed of the player

	private float sensitivityX = 15F;		// mouse sensitivity
	private float sensitivityY = 15F;
	
	private float minimumY = -60F;			// locked Y axis view
	private float maximumY = 60F;

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
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		float y = 0;

		// Calculate the force to add to the player
		moveForce = moveSpeed * rigidbody.mass;
		// Create a vector and populate it with the movement input
		Vector3 force = new Vector3(x, y, z).normalized * moveForce * rigidbody.drag;
		rigidbody.AddForce(force);
	}
}
