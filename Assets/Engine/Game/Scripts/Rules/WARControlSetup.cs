using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using WAR.UI;
using WAR.Board;
using WAR.Tools;

namespace WAR.Game {
	public class WARControlSetup : Manager<WARControlSetup> {
		public void Start () {
			// when we set a mode and it's directed towards the gameplay mode
			WARGame.Mode.Where(epoch => epoch.current == GAME_MODE.setup)
				// call the init handler
				.Subscribe(setup);
			// initialize the board at the appropriate time
			StartCoroutine(WaitForUIInput());
			
		}
		public void setup(Epoch<GAME_MODE> epoch){
			// setup the game
		}
		
		IEnumerator WaitForUIInput() {
			while (UIInput.Instance == null) yield return null;
			UIInput.Instance.onBoardInit += initBoard;
		}
		
		public void initBoard (UIPlane plane) {
			// create a new table with the given plane
			WARControlBoard.CreateTable(plane);
			// create the players
			
			// we're done with the setup mode so move to the deployment mode
			WARGame.SetMode(GAME_MODE.deployment);
		}
	}
}