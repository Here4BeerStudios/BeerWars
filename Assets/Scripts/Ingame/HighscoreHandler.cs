using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreHandler : MonoBehaviour {
	public GameObject[] UIElements;

	private int[] topId;

	private Player[] players;
	private int[] points;

	public void InitPlayerScore(Player[] p) {
		players = p;
		points = new int[p.Length];
		for (int i = p.Length; i < UIElements.Length; i++)
			UIElements [i].SetActive (false);
		
		// Init Toplist with first X players.
		topId = new int[Mathf.Min(UIElements.Length, p.Length)];
		for (int i = 0; i < topId.Length; i++)
			topId [i] = i;

		UpdateUI ();
	}

	public void IncScore(Player player, int score) {
		int playerId = (int) player.NetId;
		points [playerId] += score;
		// Check player already in highscore.
		int i = 0;
		while (i < topId.Length && topId [i] != playerId)
			i++;
		i--;
		while (i >= 0 && points[topId[i]] < points[playerId]) {
			if(i+1 < topId.Length)
				topId [i+1] = topId [i];
			topId [i] = playerId;
			i--;
		}

		UpdateUI ();
	}

	public void UpdateUI() {
		for(int i = 0; i < topId.Length; i++) {
			// Set Emblem
			//UIElements[i].GetComponentInChildren<Image>().sprite

			Text[] texts = UIElements[i].GetComponentsInChildren<Text>();
			// Set Name
			texts[0].text = players[topId[i]].Name;
			// Set Score
			texts[1].text = points[topId[i]].ToString();
		}
	}
}
