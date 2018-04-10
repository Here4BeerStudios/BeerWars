using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChooser : MonoBehaviour {
	public string emblemPath = "path/to/spritefolder";
	public Sprite selected;

	private Sprite[] availableSprites;
	private int selectedIndex;

	// Use this for initialization
	void Start () {
		object[] loadedFiles = Resources.LoadAll (emblemPath, typeof(Sprite));
		availableSprites = new Sprite[loadedFiles.Length+1];
		for (int i = 0; i < loadedFiles.Length; i++)
			availableSprites [i+1] = (Sprite)loadedFiles [i];
			
		for (int i = 0; i < availableSprites.Length; i++) {
			if (availableSprites[i] == selected)
				selectedIndex = i;
		}
		// Maybe sprite is not in folder. Not allowed so reset.
		selected = availableSprites [selectedIndex];
		GetComponentInChildren<Image>().sprite = selected;
	}
	
	public void OnNextClick() {
		if (selectedIndex == availableSprites.Length - 1)
			selectedIndex = 0;
		else
			selectedIndex++;
		selected = availableSprites [selectedIndex];
		GetComponentInChildren<Image>().sprite = selected;
	}

	public void OnBackClick() {
		if (selectedIndex == 0)
			selectedIndex = availableSprites.Length - 1;
		else
			selectedIndex--;
		selected = availableSprites [selectedIndex];
		GetComponentInChildren<Image>().sprite = selected;
	}
}
