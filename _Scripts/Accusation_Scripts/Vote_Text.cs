using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vote_Text : MonoBehaviour {

	public void SetActive() {
		this.gameObject.SetActive (true);
	}

	public void VoteConvinced() {
		GetComponent<TextMesh> ().text = "Convinced";
	}

	public void VoteUnconvinced() {
		GetComponent<TextMesh> ().text = "Unconvinced";
	}
}
