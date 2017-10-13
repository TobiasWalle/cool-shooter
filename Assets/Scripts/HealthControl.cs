using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthControl : NetworkBehaviour {


	[SyncVar]
	public int killCount = 0;
	public int maxHealth;
    public int coolDownTime;

	[SyncVar]
	private int deathCounter = 0;
	[SyncVar]
	private int currentHealth;
	[SyncVar]
    private bool isDead = false;
	[SyncVar]
    private float coolDownTimeLeft = 0f;
	private NetworkStartPosition[] spawnPoints;

	void Start()
	{
		currentHealth = maxHealth;
		if(isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
	}

    private void Update()
    {
        if (isDead)
        {
            if (coolDownTimeLeft > 0)
            {
                coolDownTimeLeft -= Time.deltaTime;
            } else
            {
                RpcRespawn();
            }
        }
    }

    public void TakeDamage(int amount, GameObject damager)
	{
		if(!isServer) 
		{
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0) 
		{
			var otherHealth = damager.GetComponent<HealthControl>();
            Die(otherHealth);
		}
	}

    void Die(HealthControl killerHealthControl)
    {
        if (killerHealthControl == null)
        {
            Debug.LogError("Killer Health Control Script not found");
            return;
        }
        deathCounter++;
        killerHealthControl.killCount++;

        coolDownTimeLeft = coolDownTime;
        isDead = true;
    }
		
	[ClientRpc]
	void RpcRespawn()
	{
		if(isLocalPlayer)
		{
			currentHealth = maxHealth;
			Vector3 spawnPoint = Vector3.zero;
			if(spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
            isDead = false;
		}
	}

	public int GetCurrentHealth()
	{
		return currentHealth;
	}

	public int GetDeathCount()
	{
		return deathCounter;
	}
}
