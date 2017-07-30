using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using WAR.UI;
using WAR;
using WAR.Board;
using WAR.Game;
using WAR.Tools;
using WAR.Units;

public class WARModeDeployment : Manager<WARModeDeployment> {
	
	// the number of models we've created
	private int numDeployed = 1;
	
	void Start () {
		// when clicking on a cell in the deployment phase
		UIInput.TouchObservable.Where(_ => WARGame.Mode.Value.current == GAME_MODE.deployment)
			.Subscribe(addObject).AddTo(disposables);
		
	}
	
	// the response to clicking when in deployment places a cell owned by the current user
	public void addObject(Vector3 pos) {
		// find the cell underneath the point we clicked
		RaycastHit hit;
		int layerMask = 1 << (int)Layers.TableTile;
		
		// if there is an object under the vector
		if (Physics.Raycast(ray: Camera.main.ScreenPointToRay(pos), hitInfo: out hit, maxDistance: 5, layerMask: layerMask)) {
			// the id of the cell we clicked on 
			var id = hit.collider.GetComponent<WARActorCell>().id;
			
			
			// add a ship to play with
			var ship = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			).GetComponent<WARUnit>() as WARUnit;
			ship.owner = numDeployed;
			WARControlBoard.AddObjectsToCell(id,new List<WARGridObject>{ship as WARGridObject});
			// if we have deployed a model for both players
			if (numDeployed == 2) {
				StartCoroutine(DelayPhaseTransition());
			}
			// otherwise we need to generate a model for the second player
			else {
				numDeployed++;
			}
		}
	}
	
	IEnumerator DelayPhaseTransition() {
		yield return new WaitForSeconds(1);
		// we're done deploying
		WARGame.SetMode(GAME_MODE.gameplay);
	}
}
