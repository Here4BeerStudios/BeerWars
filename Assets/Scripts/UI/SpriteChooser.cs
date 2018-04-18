using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChooser : MonoBehaviour {
	public string spritePath = "path/to/spritefolder";
	public Sprite selected;


	Sprite[] availableSprites;
	private int selectedIndex;

	void Awake () {
		SelectSprite (selected);

		GetComponentsInChildren<Button> () [0].onClick.AddListener (() => OnBackClick());
		GetComponentsInChildren<Button> () [1].onClick.AddListener (() => OnNextClick());
	}

	public void SelectSprite(Sprite s) {
		if (availableSprites == null)
			InternalLoadSprites ();

		InternalSelectSprite (s);
	}

	private void InternalLoadSprites() {
		object[] loadedFiles = Resources.LoadAll (spritePath, typeof(Sprite));
		availableSprites = new Sprite[loadedFiles.Length+1];
		for (int i = 0; i < loadedFiles.Length; i++)
			availableSprites [i+1] = (Sprite)loadedFiles [i];
	}

	private void InternalSelectSprite(Sprite s) {
		for (int i = 0; i < availableSprites.Length; i++) {
			if (availableSprites[i] == s)
				selectedIndex = i;
		}
		// Maybe sprite is not in folder. Not allowed so reset.
		selected = availableSprites [selectedIndex];
		GetComponentsInChildren<Image>()[1].sprite = selected;
	}
	
	public void OnNextClick() {
		if (selectedIndex >= availableSprites.Length - 1)
			selectedIndex = 0;
		else
			selectedIndex++;
		selected = availableSprites [selectedIndex];
		GetComponentsInChildren<Image>()[1].sprite = selected;
	}

	public void OnBackClick() {
		if (selectedIndex <= 0)
			selectedIndex = availableSprites.Length - 1;
		else
			selectedIndex--;
		selected = availableSprites [selectedIndex];
		GetComponentsInChildren<Image>()[1].sprite = selected;
	}
}
