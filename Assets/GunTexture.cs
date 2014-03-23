using UnityEngine;
using System.Collections;

public class GunTexture : Singleton<GunTexture> {

	public Texture shotgun;
	public Texture laser;

	void Start () 
	{
		guiTexture.enabled = false;
		ScaleTexture();
	}
	
	void ScaleTexture()
	{
		
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
	
	public void ChangeTexture(Transform weapon)
	{
		guiTexture.texture = weapon.guiTexture.texture;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameIsPaused)
			guiTexture.enabled = true;
		else 
			guiTexture.enabled = false;
	}
}
