using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using WAR.Game;

namespace WAR.Equipment {
	public enum WeaponType {
		ranged,
		melee,
		pistol,
		sword,
	}
	
	public class WARWeapon : WAREquipment {
		[SerializeField]
		public int range;
		
		[SerializeField]
		private List<Damage> damage;
		
		public WeaponType type;
	}
}