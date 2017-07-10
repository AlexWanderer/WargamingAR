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
		private IDisposable addSubscription;
		private IDisposable removeSubscription;
		
		public void Start() {
			// when an item is selected
			addSubscription = WARControlSelection.Selection.ObserveAdd().Subscribe(selectionAdded);
			removeSubscription = WARControlSelection.Selection.ObserveRemove().Subscribe(selectionRemoved);
		}
		
		// locate the cell under an object
		public List<int> findCellsUnderObject(WARGridObject obj) {
			// a place to store the result
			RaycastHit hit;
			int layerMask = 1 << (int)Layers.TableTile;
			
			// if there is an object under the unit
			if (Physics.Raycast(origin: obj.transform.position + Vector3.up, direction: -Vector3.up, hitInfo: out hit, maxDistance: 5, layerMask: layerMask)) {
				return new List<int>{hit.collider.GetComponent<WARActorCell>().id};
			}
			// we didn't hit a cell so there is nothing to return
			return new List<int>();
		}
		
		// when an object is added to the selection
		public void selectionAdded(CollectionAddEvent<WARGridObject> gridObject) {
			// for each cell under the object
			foreach (var cellId in findCellsUnderObject(gridObject.Value)) {
				// set the cell to be highlighted
				cells[cellId].highlighted.SetValueAndForceNotify(true);
			}
		}
		
		// when an object is removed from the selection
		public void selectionRemoved(CollectionRemoveEvent<WARGridObject> gridObject) {
			// for each cell under the object
			foreach (var cellId in findCellsUnderObject(gridObject.Value)) {
				// set the cell to be highlighted
				cells[cellId].highlighted.SetValueAndForceNotify(false);
			}
		}
		
		// when the object is destroyed
		public void OnDestroy() {
			// make sure to clean up any subscriptions
			foreach (var subscription in new List<IDisposable>{addSubscription, removeSubscription}) {
				// if we have an add subscription and it hasn't been disposed yet
				if (subscription != null) {
				// stop listening for changes to the selection
					subscription.Dispose();
				}	
			}
		}
		
	}
}