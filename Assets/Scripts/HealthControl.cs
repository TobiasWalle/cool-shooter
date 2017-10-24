using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthControl : NetworkBehaviour {
	public int maxHealth;
    public int coolDownTime;

	[SyncVar]
	private int currentHealth;
	[SyncVar]
    private bool isDead = false;
	[SyncVar]
    private float coolDownTimeLeft = 0f;
	private NetworkStartPosition[] _spawnPoints;
	private ScoreManager _scoreManager;
	private PlayerController _playerController;

	void Start()
	{
		currentHealth = maxHealth;
		if(isLocalPlayer)
		{
			_spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
		_scoreManager = FindObjectOfType<ScoreManager>();
		_playerController = GetComponent<PlayerController>();
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
			currentHealth = 0;       
			var otherController = damager.GetComponent<PlayerController>();
			_scoreManager.RpcSetScore(_playerController.GetPlayerName(), ScoreManager.ScoreTypes.Deaths, 1);
			_scoreManager.RpcSetScore(otherController.GetPlayerName(), ScoreManager.ScoreTypes.Kills, 1);
			_playerController.RpcEnabled(false);
			Die();
		}
	}

    private void Die()
    {			
        coolDownTimeLeft = coolDownTime;
        isDead = true;
    }
		
	[ClientRpc]
	private void RpcRespawn()
	{
		currentHealth = maxHealth;
		Vector3 spawnPoint = Vector3.zero;
		if(_spawnPoints != null && _spawnPoints.Length > 0)
		{
			spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
		}
		transform.position = spawnPoint;
		isDead = false;
		_playerController.RpcEnabled(true);
	}

	public int GetCurrentHealth()
	{
		return currentHealth;
	}
}
