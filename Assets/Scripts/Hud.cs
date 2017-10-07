using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Hud : NetworkBehaviour {

	private GUIStyle healthStyle;
	private GUIStyle backStyle;
	private GUIStyle textStyle;
	private HealthControl healthControl;
	private const int height = 20;

	void Awake() 
	{
		healthControl = GetComponent<HealthControl>();
		if(healthControl == null)
		{
			Debug.LogError ("Health Control script required but not found.");
		}
	}

	void OnGUI()
	{
		if (!isLocalPlayer) {
			return;
		}
		InitStyles();

		Vector3 pos = new Vector2 (50, 300);

		// draw health bar background
		GUI.color = Color.grey;
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(pos.x - 25, Screen.height - pos.y + 20, healthControl.maxHealth, height), ".", backStyle);

		// draw health bar amount
		GUI.color = Color.green;
		GUI.backgroundColor = Color.green;
		GUI.Box(new Rect(pos.x - 25, Screen.height - pos.y + 20, healthControl.currentHealth, height), ".", healthStyle);

		var rect = new Rect (pos.x + 15, Screen.height - pos.y + 22, 200, 200);
		GUI.Label(rect, healthControl.currentHealth.ToString() , textStyle);
	}

	void InitStyles()
	{
		if( healthStyle == null )
		{
			healthStyle = new GUIStyle( GUI.skin.box );
			healthStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 1f, 0f, 1.0f ) );
		}

		if( backStyle == null )
		{
			backStyle = new GUIStyle( GUI.skin.box );
			backStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 1.0f ) );
		}

		if ( textStyle == null) 
		{
			textStyle = new GUIStyle( );
			textStyle.normal.textColor = new Color( 0f, 0f, 1f, 1.0f );
		}
	}

	Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
}
