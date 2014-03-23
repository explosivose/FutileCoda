using UnityEngine;
using System.Collections;

public class GunTextureScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//guiTexture.enabled = false;
		int textureHeight = guiTexture.texture.height;
		int textureWidth = guiTexture.texture.width;
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;

		int textureHeightScaled = Mathf.RoundToInt (screenHeight/2);
		int	textureWidthScaled = Mathf.RoundToInt (screenWidth/2);
		Debug.Log (textureWidthScaled);
		Debug.Log (textureHeightScaled);
		guiTexture.pixelInset = new Rect(-textureWidthScaled/2, -screenHeight/2,textureWidthScaled, textureHeightScaled);
		 ;

	}

	// Update is called once per frame
	void Update () {
	if(!GameObject.Find ("GameManager").GetComponent<GameManager>().GameIsPaused)
			guiTexture.enabled = true;
	}
}
