using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WAR.UI;
using WAR.Game;
using WAR.Pathfinder;

namespace WAR.Board {
	public abstract class WARGrid : MonoBehaviour {
		
		public IWARPathfinder pathfinder;
	
		// display the grid on the view 
		abstract public void createGrid();
		
		// add objects to a specific cellId
		abstract public void addObjectsToCell(int cellId, List<WARGridObject> objects);
		// remove objects from a specific cellId
		abstract public void removeObjectsFromCell(int cellId, List<WARGridObject> objects);
		
		// the list of cells in the grid - index is list is assumed to be the id
		protected List<WARActorCell> cells = new List<WARActorCell>();
		
		// collect all of our disposables together so we can disable them as a group
		private CompositeDisposable disposables = new CompositeDisposable();
		
		protected void initialize(IWARPathfinder pathfinder) {
			this.pathfinder = pathfinder;
		}
		
		public void Start() {
			// when an item is selected
			WARControlSelection.Selection.ObserveAdd().Subscribe(selectionAdded).AddTo(disposables);
			// when an item is remove from the selection
			WARControlSelection.Selection.ObserveRemove().Subscribe(selectionRemoved).AddTo(disposables);
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
				cells[cellId].highlighted.Value = true;
			}
		}
		
		// when an object is removed from the selection
		public void selectionRemoved(CollectionRemoveEvent<WARGridObject> gridObject) {
			// for each cell under the object
			foreach (var cellId in findCellsUnderObject(gridObject.Value)) {
				// set the cell to be highlighted
				cells[cellId].highlighted.Value = false;
			}
		}
		
		public void moveObjectsToCell(int cellId, List<WARGridObject> objects ) {
			int id = findCellsUnderObject(objects[0])[0];
				
			// if we have one
			if (pathfinder != null) {
				Debug.Log("found pathfinder");
				// remove objects from the original cell
				removeObjectsFromCell(id, objects);
				// TODO, cleanup thix findCellsUnderObject call, maybe just pass source cellId in
				// use the first object we are trying to move to find the cell id of the highlighted cell
				cells[id].highlighted.Value = false;
				// add objects to the target cell and highlight it
				addObjectsToCell(cellId, objects);
				cells[cellId].highlighted.Value = true;
			}
		}
		
		public WARActorCell GetCell(int cellId) {
			// return the desired cell if it is a valid id
			if (0 <= cellId && cellId < cells.Count) {
				return cells[cellId];
			} 
			// else raise an error that we could not do so
			else {
				Debug.LogError("cannot return cell with id:" + cellId.ToString());
				return null;
			}
		}
		
		// when the object is destroyed
		public void OnDestroy() {
			// make sure to clean up any subscriptions
			disposables.Dispose();
		}
		
	}
}