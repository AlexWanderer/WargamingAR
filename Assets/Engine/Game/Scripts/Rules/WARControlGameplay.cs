using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using WAR.Board;
using WAR.UI;
using WAR.Tools;
using WAR;

namespace WAR.Game {
	public class WARControlGameplay : Manager<WARControlGameplay> {
		
		// wether or not we are in the gamplay mode
		
		
		public void Start () {
			// when we set a mode and it's directed towards the gameplay mode
			WARGame.Mode.Where(epoch => epoch.current == GAME_MODE.gameplay)
						// call the init handler
						.Subscribe(initMode);
			
			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => WARGame.Mode.Value.current == GAME_MODE.gameplay &&
											   WARGame.Phase.Value.current == GAME_PHASE.movement)
								   .Where(_ => WARControlSelection.Selection.Count > 0)
								   .Subscribe(moveObject);
		}
		
		// called when we move to the gameplay mode
		public void initMode(Epoch<GAME_MODE> modeEpoch) {
			// start in the movement phase
			WARGame.SetPhase(GAME_PHASE.movement);
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
				// we clicked on cell so move the current select to the cell
				var list = new List<WARGridObject>();
				foreach (var selected in WARControlSelection.Selection ) {
					list.Add(selected);
				}
				
				// move the selection to the right cell
				WARControlBoard.MoveObjectsToCell(id, list);	 			
			}
		}
		
	}
	
}
