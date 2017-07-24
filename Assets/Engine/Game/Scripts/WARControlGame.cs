using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using WAR.Board;
using WAR.UI;

namespace WAR.Game {
	
	public enum GAME_PHASE {
		movement,
		command,
		shooting,
		assault,
	}
	
	public class WARControlGame : Manager<WARControlGame> {
		
		// the phase of the game, the current step in the turn
		public ReactiveProperty<GAME_PHASE> phase = new ReactiveProperty<GAME_PHASE>();
		
		public static void SetPhase(GAME_PHASE newPhase) {
			// set the current phase to the desired phase
			WARControlGame.Instance.phase.Value = newPhase;
		}
		public static void NextPhase() {
			
			SetPhase(WARControlGame.Instance.phase.Value + 1);
		}
		
		public static ReactiveProperty<GAME_PHASE> Phase {
			// return the current phase
			get { 
				return WARControlGame.Instance.phase; 
			}
		}
		
		public void Awake () {
			phase.Value =	GAME_PHASE.assault;
		}

		public void Start () {
			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => phase.Value == GAME_PHASE.movement)
								   .Where(_ => WARControlSelection.Selection.Count > 0)
								   .Subscribe(moveObject);
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
