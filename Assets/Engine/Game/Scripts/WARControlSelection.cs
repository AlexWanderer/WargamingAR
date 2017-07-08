using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WAR;
using WAR.UI;

public class WARControlSelection :  Manager<WARControlSelection> {
	
	private int selectMask = (1 << (int)Layers.TableTile) | (1 << (int)Layers.Selectable);
	
	// Use this for initialization
	void Start () {
		UIInput.TouchObservable.Subscribe(handleTouch);
	}
	
	void handleTouch(Vector3 touchObservable) {
		Ray ray = Camera.main.ScreenPointToRay(touchObservable);
		RaycastHit[] hits = Physics.RaycastAll(ray, 25f, selectMask);
		if (hits.Length > 0) {
			hits[0].collider.GetComponent<MeshRenderer>().materials[0].color = Color.red;
		}
	}

}
