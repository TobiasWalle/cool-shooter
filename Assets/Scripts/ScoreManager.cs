using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ScoreManager : NetworkBehaviour {

	private Dictionary <string, Dictionary<ScoreTypes, int>> playerScores = new Dictionary<string, Dictionary<ScoreTypes, int>>();
	private int _changeCounter;

	public enum ScoreTypes {
		Kills,
		Deaths
	}

	void Start()
	{
		_changeCounter = 0;
	}
		
	public int GetScore(string username, ScoreTypes scoreType)
	{
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

	[ClientRpc]
	public void RpcSetScore(string username, ScoreTypes scoreType, int value)
	{
		if(!playerScores.ContainsKey(username))
		{
			Debug.LogError("Player must register before assining score");
			return;
		}
		var currentScore = GetScore(username, scoreType);
		playerScores[username][scoreType] = value + currentScore;
		_changeCounter++;
	}
		
	public string[] GetPlayerNames(ScoreTypes sortBy)
    { 
		var names = playerScores.Keys;
		return names.OrderByDescending(x=> GetScore(x, sortBy)).ToArray();
	}
		
	public string GetValidName(string name)
	{
		var newName = name;
		int count = 2;
		while(playerScores.ContainsKey(newName))
		{
			newName = name + "_" + count++;
		}
		return newName;
	}

	[ClientRpc]
	public void RpcRegisterPlayer(string name)
	{
		if(!playerScores.ContainsKey(name))
		{
			playerScores[name] = new Dictionary<ScoreTypes, int>();
		}
		_changeCounter++;
	}

	public int GetChangeCounter()
	{
		return _changeCounter;
	}
}
