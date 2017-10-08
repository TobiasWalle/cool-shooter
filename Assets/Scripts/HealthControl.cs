using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthControl : NetworkBehaviour {

	private NetworkStartPosition[] spawnPoints;

	[SyncVar]
	private int _deathCounter = 0;
	[SyncVar]
	public int killCount = 0;

	[SyncVar]
	private int _currentHealth;
	public int maxHealth;

	void Start()
	{
		_currentHealth = maxHealth;
		if(isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
	}

	public void TakeDamage(int amount, GameObject damager)
	{
		if(!isServer) 
		{
			return;
		}

		Debug.Log(damager.name);

		_currentHealth -= amount;
		if (_currentHealth <= 0) 
		{
			_currentHealth = maxHealth;
			_deathCounter++;
			var otherHealth = damager.GetComponent<HealthControl>();
			if (otherHealth == null)
			{
				Debug.Log("Damager Health Control Script not found");
			} 
			else
			{
				otherHealth.killCount++;
			}
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

	public int GetCurrentHealth()
	{
		return _currentHealth;
	}

	public int GetDeathCount()
	{
		return _deathCounter;
	}
}
