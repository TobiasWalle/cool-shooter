using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Hud : NetworkBehaviour {

	public Canvas hud;
	private ScoreManager _scoreBoard;
	private Text _healthText;
	private Text _statsText;
	private Image _deathScreen;
	private HealthControl _healthControl;
	private bool _showScoreBoard;

	private void Awake() 
	{
		_healthControl = GetComponent<HealthControl>();
		if(_healthControl == null)
		{
			Debug.LogError("Health Control script required but not found.");
		}

		if(hud == null)
		{
			Debug.LogError("Hud canvas could not be loaded");
		}

		_scoreBoard = GameObject.FindObjectOfType<ScoreManager>();
		if(_scoreBoard == null) 
		{
			Debug.LogError("Unable to load score manager script.");
		}
		_showScoreBoard = false;
	}

	private void OnGUI()
	{
		if (!isLocalPlayer) {
			if (hud.enabled)
			{
				hud.enabled = false;
			}
			return;
		}

		Setup();

		ShowDeathScreen(_healthControl.GetCurrentHealth() <= 0);

		var name = GetComponent<PlayerController>().GetPlayerName();
		_healthText.text = "" + _healthControl.GetCurrentHealth() + " - " + name;
		_statsText.text = "Deaths: " + _healthControl.GetDeathCount() + "\n" +
			"Kills: " + _healthControl.killCount;

		if(_showScoreBoard && _scoreBoard != null) {
			var list = _scoreBoard.GetPlayerNames();
			var pos = 32;
			foreach(var player in list)
			{
				GUI.Label(new Rect(256, pos, 128, 32), player);
				pos += 32;
			}
		}
	}

	private void Setup()
	{
		if(_healthText == null) {
			_healthText = FindElementByTag<Text>("HudHealthText");
		}

		if(_statsText == null) {
			_statsText = FindElementByTag<Text>("HudStatsText");
		}

		if(_deathScreen == null) {
			_deathScreen = FindElementByTag<Image>("HudDeathScreen");
		}
	}

	private T FindElementByTag<T> (string tag) where T : MaskableGraphic
	{
		var allElements = hud.GetComponentsInChildren<T>();
		foreach(var element in allElements)
		{
			if (element.tag == tag)
			{
				return element;
			}
		}

		Debug.LogError("Failed to find GUI element with tag " + tag);
		return null;
	}

	private void ShowDeathScreen(bool showScreen)
	{
		var group = _deathScreen.GetComponent<CanvasGroup>();
		if(showScreen) {
			group.alpha = 1;	
		} else {
			group.alpha = 0;
		}
		
	}

	public void ShowScoreBoard(bool isShown)
	{
		_showScoreBoard = isShown;
	}
}
