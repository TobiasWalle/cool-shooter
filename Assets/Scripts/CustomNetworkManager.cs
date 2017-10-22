using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager {

	public GameObject mainMenu;
	public GameObject diconnectMenu;
	private string username = "Player";

	private void Start()
	{
		InGame(false);
		AddListenerToButton(mainMenu, "ButtonStartHost", StartupHost);
		AddListenerToButton(mainMenu, "ButtonJoinGame", JoinGame);
		AddListenerToButton(diconnectMenu, "ButtonDisconnect", Diconnect);
	}

	public void StartupHost()
	{
		SetPort();
		username = GetTextFromInputField(mainMenu, "InputFieldUsername");
		NetworkManager.singleton.StartHost();
		InGame(true);
	}

	public void JoinGame()
	{
		SetIpAddress();
		SetPort();
		username = GetTextFromInputField(mainMenu, "InputFieldUsername");
		NetworkManager.singleton.StartClient();
		InGame(true);
	}

	public void Diconnect()
	{
		InGame(false);
		NetworkManager.singleton.StopHost();
	}

	public string GetUsername()
	{
		return username;
	}

	private void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	private void SetIpAddress()
	{
		string ipAddress = GetTextFromInputField(mainMenu, "InputFieldIpAddress");
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	private void InGame(bool isInGame)
	{
		mainMenu.gameObject.SetActive(!isInGame);
		diconnectMenu.gameObject.SetActive(isInGame);
	}

	private void AddListenerToButton(GameObject panel, string buttonName, UnityAction callback )
	{
		var allButtons = panel.GetComponentsInChildren<Button>();
		var button = allButtons.FirstOrDefault(x => x.name == buttonName);
		button.onClick.AddListener(callback);
	}

	private string GetTextFromInputField(GameObject panel, string name)
	{
		var allInputFields = panel.GetComponentsInChildren<InputField>();
		var inputField = allInputFields.FirstOrDefault(x => x.name == name);
		return inputField.text;
	}
}
