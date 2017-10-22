using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class Hud : NetworkBehaviour {

	public Canvas hud;
	private ScoreManager _scoreBoard;
	private Text _healthText;
	private Image _deathScreen;
	private HealthControl _healthControl;
	private Image _scoreboardScreen;

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
		Setup();
		HideScreen(_scoreboardScreen, false);
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

		ShowDeathScreen(_healthControl.GetCurrentHealth() <= 0);

		_healthText.text = "" + _healthControl.GetCurrentHealth();
	}

	private void Setup()
	{
		if(_healthText == null) 
		{
			_healthText = FindElementByTag<Text>("HudHealthText");
		}

		if(_deathScreen == null) 
		{
			_deathScreen = FindElementByTag<Image>("HudDeathScreen");
		}

		if(_scoreboardScreen == null) 
		{
			_scoreboardScreen = FindElementByTag<Image>("HudScoreboardScreen");
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

	private void HideScreen(Image screen, bool hidden)
	{
		var group = screen.GetComponent<CanvasGroup>();
		if(hidden) {
			group.alpha = 1;	
		} else {
			group.alpha = 0;
		}
	}

	public void ShowScoreboard(bool showScreen)
	{
		HideScreen(_scoreboardScreen, showScreen);
	}

	public void ShowDeathScreen(bool showScreen)
	{
		HideScreen(_deathScreen, showScreen);
	}
}
