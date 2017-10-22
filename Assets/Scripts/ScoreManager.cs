using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ScoreManager : NetworkBehaviour {

	public Dictionary <string, Dictionary<string, int>> playerScores;

	void Start()
	{
		SetScore("Test777", "kill", 2);
	}

	private void Init()
	{
		if(playerScores != null)
		{
			return;
		}
		playerScores = new Dictionary<string, Dictionary<string, int>>();
	}

	public void AddPlayer(string player)
	{
		Debug.Log("Player " + player + " joined the match.");
	}
		
	public int GetScore(string username, string scoreType)
	{
		Init();

		if(!playerScores.ContainsKey(username))
		{
			return 0;
		}

		if(!playerScores[username].ContainsKey(scoreType))
		{
			return 0;
		}
		return playerScores[username][scoreType];
	}

	public void SetScore(string username, string scoreType, int value)
	{
		Init();
		if(!playerScores.ContainsKey(username))
		{
			playerScores[username] = new Dictionary<string, int>();
		}
		playerScores[username][scoreType] = value;
		
	}

	public void ChangeScore(string username, string scoreType, int amount)
	{
		Init();
		var currentScore = GetScore(username, scoreType);
		SetScore(username, scoreType, currentScore + amount);
	}

	public string[] GetPlayerNames()
	{
		Init();
		return playerScores.Keys.ToArray();
	}
}
