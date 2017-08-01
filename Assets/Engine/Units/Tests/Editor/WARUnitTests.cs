using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

using WAR.Tools;
using WAR.Units;

namespace WAR.Game.Tests {
	public class UnitTest {
		[Test]
		public void TakeDamage() {
			// create a unit to test with with known health
			var unit = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			).GetComponent<WARUnit>() as WARUnit;
			unit.currentHealth = 10;
			
			// a weapon profile to deal known damage
			var profile = new DamageProfile{
				emp = new Damage{strength = 1},
				kinetic = new Damage{strength = 2},
			};
			
			// tell the unit to take the damage
			unit.takeDamage(profile);
			
			// make sure the health is what we expect
			Assert.AreEqual(7, unit.currentHealth);
		}
	}
}
