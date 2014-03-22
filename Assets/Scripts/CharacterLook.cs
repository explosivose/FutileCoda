using UnityEngine;
using System.Collections;

public class CharacterLook : MonoBehaviour {

	public float sensitivityY = 15F;		// mouse sensitivity
	
	public float minimumY = -60F;			// locked Y axis view
	public float maximumY = 60F;
	
	float rotationY = 0F;
	
	void Update ()
	{
		// Get mouse input and move the camera rotation

		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
	}
}