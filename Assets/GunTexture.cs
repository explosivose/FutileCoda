using UnityEngine;
using System.Collections;

public class GunTexture : MonoBehaviour {

	public Texture shotgun;
	public Texture laser;

	void Start () 
	{
		guiTexture.enabled = false;
		int textureHeight = guiTexture.texture.height;
		int textureWidth = guiTexture.texture.width;
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;
		
		int textureHeightScaled = Mathf.RoundToInt (screenHeight/2);
		int	textureWidthScaled = Mathf.RoundToInt (screenWidth/2);
		Debug.Log (textureWidthScaled);
		Debug.Log (textureHeightScaled);
		guiTexture.pixelInset = new Rect(-textureWidthScaled/2, -screenHeight/2,textureWidthScaled, textureHeightScaled);

	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.Find ("GameManager").GetComponent<GameManager>().GameIsPaused)
		{
			guiTexture.enabled = true;
			//"GameObject.Find("Player(Clone)").GetComponent<Player>().weaponInventory[0].transform.name == "Shotgun" && 
			if(GameObject.Find ("Player(Clone)").GetComponent<Player>().selected == 0)
				guiTexture.texture = shotgun;
			else 
				guiTexture.texture = laser;
		}
		else 
			guiTexture.enabled = false;
	}
}
