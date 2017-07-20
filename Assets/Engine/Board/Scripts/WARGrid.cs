using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WAR.UI;
using WAR.Game;
using WAR.Pathfinder;
using WAR.Tools;

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

		public void moveObjectsToCell(int target, List<WARGridObject> objects ) {
			// TODO, cleanup this findCellsUnderObject call, maybe just pass source cellId in
			// use the first object we are trying to move to find the cell id of the highlighted cell
			var source = findCellsUnderObject(objects[0])[0];
			// if we have one
			if (pathfinder != null) {
				// compute the path joining the two cells on this grid
				var path = pathfinder.findPath(source, target, this);

				// tell each object to follow the path we specified
				foreach (var obj in objects) {
					// if we have a movable object
					var movable = obj as WARMovableObject;
					
					// if 
					movable.followPath(path, this);
				}
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
