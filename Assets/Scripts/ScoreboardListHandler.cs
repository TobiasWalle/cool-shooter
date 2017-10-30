using System.Collections;
using System.Collections.Generic;
using Assets.Core;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardListHandler : MonoBehaviour {

	public GameObject scoreboardListEntry;
	private ScoreManager _scoreManager;
	private int _lastChangeCounter;

	private void Start () {
		_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

		if (_scoreManager != null) return;
		Debug.LogError("No score manager found on game objects.");
		return;
	}

	private void Update () {
		if(_lastChangeCounter == _scoreManager.GetChangeCounter())
		{
			return;
		}

		EmptyList();
		LoadList();

		_lastChangeCounter = _scoreManager.GetChangeCounter();
	}

	private void EmptyList()
	{
		while(this.transform.childCount > 0)
		{
			var child = this.transform.GetChild(0);
			child.SetParent(null);
			Destroy(child.gameObject);
		}
	}

	private void LoadList()
	{
		var playser = _scoreManager.GetPlayers();
		foreach(var player in playser)
		{
			GameObject entry = (GameObject)Instantiate(scoreboardListEntry);
			SetHeaderText(entry, "HeaderName", player.Name);
			SetHeaderText(entry, "HeaderKills", player.Score.Kills.ToString());
			SetHeaderText(entry, "HeaderDeaths", player.Score.Deaths.ToString());
			entry.transform.SetParent(this.transform);
		}
	}

	private void SetHeaderText(GameObject entry, string headerName, string text)
	{
		entry.transform.Find(headerName).GetComponent<Text>().text = text;
	}
}
