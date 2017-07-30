using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using WAR.UI;
using WAR.Board;
using WAR.Tools;

namespace WAR.Game {
	public class WARModeSetup : Manager<WARModeSetup> {
		public void Start () {
			// when we set a mode and it's directed towards the gameplay mode
			WARGame.Mode.Where(epoch => epoch.current == GAME_MODE.setup)
				// call the init handler
				.Subscribe(setup).AddTo(disposables);
			// initialize the board at the appropriate time
			StartCoroutine(WaitForUIInput());
			
		}
		public void setup(Epoch<GAME_MODE> epoch){
			// spawn and add the players to the WARGame
			WARGame.Players.Add(new WARPlayer(1));
			WARGame.Players.Add(new WARPlayer(2));
			// determine who goes first, player one for now?
			WARGame.CurrentPlayer = 1;
		}
		
		IEnumerator WaitForUIInput() {
			while (UIInput.Instance == null) yield return null;
			UIInput.Instance.onBoardInit += initBoard;
		}
		
		public void initBoard (UIPlane plane) {
			// create a new table with the given plane
			WARControlBoard.CreateTable(plane);

			// we're done with the setup mode so move to the deployment mode
			WARGame.SetMode(GAME_MODE.deployment);
		}
	}
}