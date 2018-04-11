using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

	public enum MenuState { MainMenu, OptionsMenu, StoryMenu, OnlineMenu };
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject storyMenu;
	public GameObject onlineMenu;

	public MenuState current;
	// Update is called once per frame
	void Awake () {
		current = MenuState.MainMenu;
	}

	void Update() {
		switch (current) {
		case MenuState.MainMenu:
			mainMenu.SetActive (true);
			optionsMenu.SetActive (false);
			storyMenu.SetActive (false);
			onlineMenu.SetActive (false);
			break;
		case MenuState.OptionsMenu:
			mainMenu.SetActive (false);
			optionsMenu.SetActive (true);
			storyMenu.SetActive (false);
			onlineMenu.SetActive (false);
			break;
		case MenuState.StoryMenu:
			mainMenu.SetActive (false);
			optionsMenu.SetActive (false);
			storyMenu.SetActive (true);
			onlineMenu.SetActive (false);
			break;
		case MenuState.OnlineMenu:
			mainMenu.SetActive (false);
			optionsMenu.SetActive (false);
			storyMenu.SetActive (false);
			onlineMenu.SetActive (true);
			break;
		}
	}

	public void OnMainMenu() {
		current = MenuState.MainMenu;
	}

	public void OnOptionsMenu() {
		current = MenuState.OptionsMenu;
	}

	public void OnStoryMenu() {
		current = MenuState.StoryMenu;
	}

	public void OnOnlineMenu() {
		current = MenuState.OnlineMenu;
	}

	public void OnQuit() {
		Application.Quit ();
	}

	public void OnStoryLevel(int levelId) {

	}
}
