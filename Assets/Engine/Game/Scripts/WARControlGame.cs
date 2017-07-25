using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
		morale,
	}
	public enum GAME_MODE {
		setup,
		deployment,
		gameplay,
		score,
	}
	public struct Epoch<T> {
		public T last;
		public T current;
		
		public Epoch(T _last, T _curr) {
			this.last = _last;
			this.current = _curr;
		}
	}

	
	
	
	public class WARControlGame : Manager<WARControlGame> {
	
		// the phase of the game, the current step in the turn
		public static ReactiveProperty<Epoch<GAME_PHASE>> Phase = new ReactiveProperty<Epoch<GAME_PHASE>>();
		// the mode of the game we are in
		public static ReactiveProperty<Epoch<GAME_MODE>> Mode = new ReactiveProperty<Epoch<GAME_MODE>>();
		
		public static void SetPhase(GAME_PHASE newPhase) {
			// set the current game phase
			Phase.Value = new Epoch<GAME_PHASE>(Phase.Value.current,newPhase);
		}
		public static void SetMode(GAME_MODE newMode) {
			// set the current game mode
			Mode.Value = new Epoch<GAME_MODE>(Mode.Value.current,newMode);
		}

		public void Start () {
			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => Mode.Value.current == GAME_MODE.gameplay &&
											   Phase.Value.current == GAME_PHASE.movement)
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
