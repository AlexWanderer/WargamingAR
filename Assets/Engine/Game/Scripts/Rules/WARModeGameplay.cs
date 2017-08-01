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
	public class WARModeGameplay : Manager<WARModeGameplay> {
		
		public void Start () {
			// when we set a mode and it's directed towards the gameplay mode
			WARGame.Mode.Where(epoch => epoch.current == GAME_MODE.gameplay)
						// call the init handler
				.Subscribe(setupMode).AddTo(disposables);
			
			// when the phase changes to movement and the last phase was morale, we have swapped turns
			WARGame.Phase.Where(epoch => epoch.current == GAME_PHASE.end)
				.Subscribe(nextTurn).AddTo(disposables);
		}
		
		// called when we move to the gameplay mode
		public void setupMode(Epoch<GAME_MODE> modeEpoch) {
			// clear any selections that were made in deployment
			WARControlSelection.ClearSelection();
			// start in the movement phase
			WARGame.SetPhase(GAME_PHASE.shooting);

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
			WARGame.CurrentPlayer = ((WARGame.CurrentPlayer) % WARGame.Players.Count) + 1;
			nextPhase();
		}
	
	}
	
}
