using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UniRx;
using UnityEngine;
using WAR.Board;
using WAR.UI;
using WAR.Tools;
using WAR.Units;

namespace WAR.Game {
	public class WARPhaseMovement : Manager<WARPhaseMovement> {
		
		public void Start () {

			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => 
				WARGame.Mode.Value.current == GAME_MODE.gameplay &&
				WARGame.Phase.Value.current == GAME_PHASE.movement
			)
			.Where(_ => WARControlSelection.Selection.Count > 0)
			.Subscribe(moveObject).AddTo(disposables);
		}
		
		// when a move order is issued
		public void moveObject(Vector3 pos) {
			// find the cell underneath the point we clicked
			RaycastHit hit;
			int layerMask = 1 << (int)Layers.TableTile;
			
			// if there is an object under the vector
			if (Physics.Raycast(ray: Camera.main.ScreenPointToRay(pos), hitInfo: out hit, maxDistance: 5, layerMask: layerMask)) {
				// the id of the cell we clicked on 
				var id = hit.collider.GetComponent<WARActorCell>().id;
				// add the current selected objects to a list if the current player owns them
				var list = WARControlSelection.Selection
					.Where(obj => (obj as WARUnit).owner == WARGame.CurrentPlayer)
					.ToList();
				
				// if we own any selected objects
				if (list.Count > 0) {
					// move the list of objects to the right cell
					WARControlBoard.MoveObjectsToCell(id, list);	 			
				}
			}
		}
		
	}
	
}
