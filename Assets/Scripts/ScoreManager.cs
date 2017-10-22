using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ScoreManager : NetworkBehaviour {

	private Dictionary <string, Dictionary<int, int>> playerScores;
	private int _changeCounter;

	public enum ScoreTypes {
		Kills,
		Deaths
	}

	void Start()
	{
		_changeCounter = 0;
	}

	private void Init()
	{
		if(playerScores != null)
		{
			return;
		}
		playerScores = new Dictionary<string, Dictionary<int, int>>();
	}
		
	public int GetScore(string username, ScoreTypes scoreType)
	{
		Init();
		if(!playerScores.ContainsKey(username))
		{
			return 0;
		}

		if(!playerScores[username].ContainsKey((int)scoreType))
		{
			return 0;
		}
		return playerScores[username][(int)scoreType];
	}

	[ClientRpc]
	public void RpcSetScore(string username, ScoreTypes scoreType, int value)
	{
		Init();
		if(!playerScores.ContainsKey(username))
		{
			Debug.LogError("Player must register before assining score");
			return;
		}
		var currentScore = GetScore(username, scoreType);
		playerScores[username][(int)scoreType] = value + currentScore;
		_changeCounter++;
	}
		
	public string[] GetPlayerNames(ScoreTypes sortBy)
	{
		Init();
		var names = playerScores.Keys;
		return names.OrderByDescending(x=> GetScore(x, sortBy)).ToArray();
	}
		
	public string GetValidName(string name)
	{
		Init();
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
		Init();
		if(!playerScores.ContainsKey(name))
		{
			playerScores[name] = new Dictionary<int, int>();
		}
		_changeCounter++;
	}

	public int GetChangeCounter()
	{
		return _changeCounter;
	}
}
