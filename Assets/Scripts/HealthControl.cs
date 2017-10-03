using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthControl : NetworkBehaviour {

	private Rect TextArea;
	private NetworkStartPosition[] spawnPoints;

	[SyncVar]
	public int currentHealth = maxHealth;
	public const int maxHealth = 100;

	void Start()
	{
		TextArea = new Rect (10, 10, Screen.width, Screen.height);
		if(isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
	}

	void OnGUI()
	{
		if(isLocalPlayer)
		{
			GUI.Label (TextArea, "Health: " + currentHealth);
		}
	}

	public void TakeDamage(int amount)
	{
		if(!isServer) 
		{
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0) 
		{
			currentHealth = maxHealth;
			RpcRespawn();
		}
	}
		
	[ClientRpc]
	void RpcRespawn()
	{
		if(isLocalPlayer)
		{
			Vector3 spawnPoint = Vector3.zero;
			if(spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
		}
	}
}
