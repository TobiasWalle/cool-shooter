using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Hud : NetworkBehaviour {

	public Canvas _hud;
	private Text _healthText;
	private Text _statsText;
	private Image _deathScreen;
	private HealthControl _healthControl;

	private void Awake() 
	{
		_healthControl = GetComponent<HealthControl>();
		if(_healthControl == null)
		{
			Debug.LogError("Health Control script required but not found.");
		}

		if(_hud == null)
		{
			Debug.LogError("Hud canvas could not be loaded");
		}
	}

	private void OnGUI()
	{
		if (!isLocalPlayer) {
			if (_hud.enabled)
			{
				_hud.enabled = false;
			}
			return;
		}

		Setup();

		ShowDeathScreen(_healthControl.GetCurrentHealth() <= 0);

		_healthText.text = "+" + _healthControl.GetCurrentHealth();
		_statsText.text = "D: " + _healthControl.GetDeathCount() + "\n" +
			"K: " + _healthControl.killCount;
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
		var allElements = _hud.GetComponentsInChildren<T>();
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
}
