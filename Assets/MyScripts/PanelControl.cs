﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour {

	public GameObject Panel;

	public void hidePanel() {
		Panel.gameObject.SetActive(false);
	}

	public void popPanel() {
		Panel.gameObject.SetActive(true);
	}
}
