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
using WAR.Equipment;

namespace WAR.Game {
	
	public class WARPhaseShooting : Manager<WARPhaseShooting> {
		
		public void Start () {
			
			// a move order is issued when there is a click with a non-zero selection
			UIInput.TouchObservable.Where(_ => 
				WARGame.Mode.Value.current == GAME_MODE.gameplay &&
				WARGame.Phase.Value.current == GAME_PHASE.shooting
			)
				.Where(_ => WARControlSelection.Selection.Count > 0)
				.Subscribe(shootTarget).AddTo(disposables);
		}
		
		// when a move order is issued
		public void shootTarget(Vector3 pos) {
			// find the cell underneath the point we clicked
			RaycastHit hit;
			int layerMask = 1 << (int)Layers.TableTile;
			
			// if there is an object under the vector
			if (Physics.Raycast(ray: Camera.main.ScreenPointToRay(pos), hitInfo: out hit, maxDistance: 5, layerMask: layerMask)) {
				// the id of the cell we clicked on 
				var id = hit.collider.GetComponent<WARActorCell>().id;
				
				// the object we current have selected
				var source = WARControlSelection.Selection[0] as WARUnit;
				
				// the objects in the cell that the current player does not own
				var targets = WARControlBoard.Grid.GetCell(id).objects
					.Where(obj => (obj as WARUnit).owner != WARGame.CurrentPlayer)
					// only grab the units in the cell
					.Where(obj => obj is WARUnit)
					// cast the final targets to a WARUnit
					.Select(obj => (WARUnit)obj)
					// do not target invulnerable objects
					.Where(obj => obj.GetComponent<WARRuleInvulnerable>() == null);
				
				// grab the weapon we are shooting with
				var weap = source.GetComponent<WARRangedWeapon>();
				
				// the basic attack profile of the shooter and weapon
				var attack = new WARShootingAttack(source, weap);
				
				// damage each target 
				foreach (var target in targets) {	
					// compute the final attack profile for the weapon against the target
					var profile = attack.computeFinalAttack(target);
					
					// the chance to hit is related to the weapon's accuracy
					var chanceToHit = 100 / (4 - profile.accuracy + profile.weaponSkill);
					
					// if we generate a random number from 0 to 100 below the chance
					if (Random.value * 100 < chanceToHit) {
						// perform the right amount of damage according to the source
						target.takeDamage(profile);
					}
					// otherwise we missed a hit
					else { 
						print("missing hit!");
					}
				}
			}
		}
	}
}
