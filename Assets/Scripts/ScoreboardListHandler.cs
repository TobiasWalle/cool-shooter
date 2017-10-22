using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardListHandler : MonoBehaviour {

	public GameObject scoreboardListEntry;
	private ScoreManager _scoreManager;
	private int _lastChangeCounter;

	private void Start () {
		_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

		if(_scoreManager == null)
		{
			Debug.LogError("No score manager found on game objects.");
			return;
		}
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
			Transform child = this.transform.GetChild(0);
			child.SetParent(null);
			Destroy(child.gameObject);
		}
	}

	private void LoadList()
	{
		var playser = _scoreManager.GetPlayerNames(ScoreManager.ScoreTypes.Kills);
		foreach(var player in playser)
		{
			GameObject entry = (GameObject)Instantiate(scoreboardListEntry);
			SetHeaderText(entry, "HeaderName", player);
			var kills = _scoreManager.GetScore(player, ScoreManager.ScoreTypes.Kills).ToString();
			SetHeaderText(entry, "HeaderKills", kills);
			var deaths = _scoreManager.GetScore(player, ScoreManager.ScoreTypes.Deaths).ToString();
			SetHeaderText(entry, "HeaderDeaths", deaths);
			entry.transform.SetParent(this.transform);
		}
	}

	private void SetHeaderText(GameObject entry, string headerName, string text)
	{
		entry.transform.Find(headerName).GetComponent<Text>().text = text;
	}
}
