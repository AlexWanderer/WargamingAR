using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WAR;
using WAR.UI;
using WAR.Board;

namespace WAR.Game {
	
	public class WARControlSelection :  Manager<WARControlSelection> {
		
		private int selectMask = (1 << (int)Layers.TableTile) | (1 << (int)Layers.Selectable);
		
		// the objects we have selected
		public static ReactiveCollection<WARGridObject> Selection = new ReactiveCollection<WARGridObject>();
		
		// Use this for initialization
		void Start () {
			UIInput.TouchObservable.Subscribe(handleTouch);
		}
		
		void Clear() {
			while (Selection.Count > 0) {
				Selection.RemoveAt(0);
			}
		}
		
		void handleTouch(Vector3 touchObservable) {
			// cast a ray from the point we touched out to the board
			Ray ray = Camera.main.ScreenPointToRay(touchObservable);
			RaycastHit[] hits = Physics.RaycastAll(ray, 25f, selectMask);
			
			// if we didn't hit something the user means to 
			if (hits.Length == 0) {
			// clear the current selection
				Clear();
			} 
			// otherwise we hit something that could have selectable objects
			else {
				// save the first hit
				var target = hits[0];
				var cell = target.collider.GetComponent<WARActorCell>();
				
				// if the target is a cell with contents
				if (cell != null && cell.objects.Count > 0) {
					// use the contents as the selection using remove so that subscribers can listen
					Clear();
					// add each object in the cell to the selection
					foreach (var obj in cell.objects) {
						Selection.Add(obj);
					}
				}
			}
		}
	}
}
