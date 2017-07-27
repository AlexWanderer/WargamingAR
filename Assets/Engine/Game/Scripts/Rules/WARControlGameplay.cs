using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UniRx;
using UnityEngine;
using WAR.Board;
using WAR.UI;
using WAR.Tools;
using WAR;

namespace WAR.Game {
	public class WARControlGameplay : Manager<WARControlGameplay> {
		// the current player in the game
		public int currentPlayer;
		public static int CurrentPlayer {
			get {
				// return the current player of the game
				return Instance.currentPlayer;
			}
			set {
				Instance.currentPlayer = value;
			}
		}
		
		public void Start () {
			// when we set a mode and it's directed towards the gameplay mode
			WARGame.Mode.Where(epoch => epoch.current == GAME_MODE.gameplay)
						// call the init handler
				.Subscribe(initMode).AddTo(disposables);
			
			// when the phase changes to movement and the last phase was morale, we have swapped turns
			WARGame.Phase.Where(epoch => epoch.current == GAME_PHASE.end)
				.Subscribe(nextTurn).AddTo(disposables);
				
			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => WARGame.Mode.Value.current == GAME_MODE.gameplay &&
											   WARGame.Phase.Value.current == GAME_PHASE.movement)
								   .Where(_ => WARControlSelection.Selection.Count > 0)
				.Subscribe(moveObject).AddTo(disposables);
		}
		
		// called when we move to the gameplay mode
		public void initMode(Epoch<GAME_MODE> modeEpoch) {
			// start in the movement phase
			WARGame.SetPhase(GAME_PHASE.movement);

		}
		public void nextPhase() {
			// move to the next phase of the game
			GAME_PHASE nextPhase = WARGame.Phase.Value.current + 1;
			var phase = GAME_PHASE.GetValues(typeof(GAME_PHASE)).Cast<GAME_PHASE>().Last();
			phase = nextPhase > phase ? default(GAME_PHASE) + 1 : nextPhase;
			WARGame.SetPhase(phase);
		}
		
		// called when the game phase returns to movement from morale
		public void nextTurn(Epoch<GAME_PHASE> phaseEpoch) {
			// move to the next players turn, mod by numbers of players to cycle
			Instance.currentPlayer = ((Instance.currentPlayer) % WARGame.Players.Count) + 1;
			nextPhase();
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
				// add the current selected objects to a list
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
