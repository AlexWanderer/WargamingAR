using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WAR.Units;
using WAR.Game;
using WAR.Equipment;

namespace WAR.Game {
	public enum DamageType {
		emp,
		thermal,
		phaser,
		kinetic,
		special,
	}
	
	public struct Damage {
		public DamageType type;
		[Range(1,4)]
		public int damage;
		[Range(1,4)]
		public int armorPen;
	}
	
	public struct ShootingAttack {
		public List<Damage> damage;		
		public int range;
		public int accuracy;
		public int attacks;
		public bool requireLOS;
	}
	
	public class WARShootingAttack {
		// who is making this shooting attack
		private WARUnit shooter;
		public ShootingAttack attack;
		
		// make the initial modification with the weapon that was fired
		public WARShootingAttack(WARUnit shooter, IWARShootingModifier fired) {
			this.shooter = shooter;
			this.attack = fired.modifyShootingAttack(new ShootingAttack());
		}
		
		public WARShootingAttack() {
			this.shooter = null;
			this.attack = new ShootingAttack();
		}
		
		public ShootingAttack computeAttack() {
			// the list of equipment that contributes to a shooting event
			var equipment = shooter.GetComponents<MonoBehaviour>()
				.Where(eq => (eq as IWARShootingModifier) != null)
				// ignoring weapons since only one is applied to begin with
				.Where(eq => (eq as WARWeapon) == null)
				// cast them to the interface
				.Select(item => item as IWARShootingModifier);
			
			// start off with the initial weap modifier
			var result = attack;
			
			// for each piece of equipment we care about
			foreach (var item in equipment) {
				// apply the modifier
				result = item.modifyShootingAttack(result);
			}
			
			// return the final attack
			return result;
		}
		
		public ShootingAttack computeFinalAttack(WARUnit target) {
			// the list of equipment that contributes to a shooting event
			var equipment = target.GetComponents<MonoBehaviour>()
				.Where(eq => (eq as IWARShootingTargetModifier) != null)
				// cast them to the interface
				.Select(item => item as IWARShootingTargetModifier);
			
			// start off with the offensives profile
			var result = computeAttack();
			
			// for each piece of equipment we care about
			foreach (var item in equipment) {
				// apply the modifier
				result = item.modifyShootingAttackTarget(result);
			}
			
			// return the final attack
			return result;
		}
	}
}
