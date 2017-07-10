using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WAR.UI;
using WAR.Game;

namespace WAR.Board {
	public abstract class WARGrid : MonoBehaviour {
		// display the grid on the view 
		abstract public void CreateGrid();
		
		// add objects to a specific grid
		abstract public void AddObjectsToCell(int cellId, List<WARGridObject> objects);
		
		// the list of cells in the grid - index is list is assumed to be the id
		protected List<WARActorCell> cells = new List<WARActorCell>();
		
		// our subscription object for the current selection
		private IDisposable subscription;
		
		public void Start() {
			// when an item is selected
			subscription = WARControlSelection.Selection.ObserveAdd().Subscribe(selectionAdded);
		}
		
		// when an object is selected
		public void selectionAdded(CollectionAddEvent<WARGridObject> gridObject) {
			Debug.Log(gridObject.ToString());
		}
		
		// when the object is destroyed
		public void OnDestroy() {
			// if we have a subscription and it hasn't been disposed yet
			if (subscription != null) {
				// stop listening for changes to the selection
				subscription.Dispose();
			}	
		}
		
	}
}