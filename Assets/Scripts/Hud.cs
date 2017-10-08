using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Hud : NetworkBehaviour {

	public Canvas _hud;
	private Text _healthText;
	private Text _statsText;
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

		_healthText.text = "+" + _healthControl.GetCurrentHealth();
		_statsText.text = "D: " + _healthControl.GetDeathCount() + "\n" +
			"K: " + _healthControl.killCount;
	}

	private void Setup()
	{
		if(_healthText == null) {
			_healthText = FindTextElementByTag ("HudHealthText");
		}

		if(_statsText == null) {
			_statsText = FindTextElementByTag("HudStatsText");
		}
	}

	private Text FindTextElementByTag(string tag)
	{
		Text element = null;
		var allTextElements = _hud.GetComponentsInChildren<Text>();
		foreach(var text in allTextElements)
		{
			if (text.tag == tag)
			{
				element = text;
				break;
			}
		}
		if (element == null)
		{
			Debug.LogError("Failed to find text element with tag " + tag);
		}
		return element;
	}
}
