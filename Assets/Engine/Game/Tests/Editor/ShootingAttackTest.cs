using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WAR.Units;
using WAR.Tools;
using WAR.Equipment;

namespace WAR.Game.Tests {
	
	public class TestModifier : MonoBehaviour, IWARShootingModifier {
		public ShootingAttack modifyShootingAttack(WARUnit source, ShootingAttack attack) {
			var newAttack = new ShootingAttack();
			
			// set the range to something we know
			newAttack.range = 1 + attack.range;
			
			return newAttack;
		}
	}
	
	public class TestTargetModifier : MonoBehaviour, IWARShootingTargetModifier {
		public ShootingAttack modifyShootingAttackTarget(WARUnit target, ShootingAttack defense) {
			var newAttack = new ShootingAttack();
			
			// set the range to something we know
			newAttack.range = 1 + defense.range;
			
			return newAttack;
		}
	}
	
	public class ShootingAttackTest {
		
		[Test]
		public void InitializesWithWeapon() {
			// a weapon to test with
			var weap = new GameObject().AddComponent<TestModifier>() as TestModifier;
			// the shooter
			var shooter = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			).GetComponent<WARUnit>() as WARUnit;;
			
			// a place to store the result
			var attack = new WARShootingAttack(shooter, weap);
			
			// make sure the internal tracker has the right values
			Assert.AreEqual(1, attack.attack.range);
		} 
		
		[Test]
		public void CanAggregateModifiers() {
			// a weapon to test with
			var weap = new GameObject().AddComponent<TestModifier>() as TestModifier;
			// the shooter
			var shooter = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			);
			// attach a few more instances of the modifier to the object
			shooter.AddComponent<TestModifier>();
			shooter.AddComponent<TestModifier>();
			shooter.AddComponent<TestModifier>();
			
			// a place to store the result
			var attack = new WARShootingAttack(shooter.GetComponent<WARUnit>() , weap);
			
			// compute the final shooting attack
			var final = attack.computeAttack();
			
			// make sure the internal tracker has the right values
			Assert.AreEqual(4, final.range);
		}
		
		[Test]
		public void IgnoresWeapons() {
			// a weapon to test with
			var weap = new GameObject().AddComponent<TestModifier>() as TestModifier;
			// the shooter
			var shooter = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			);
			// attach a few more instances of the modifier to the object
			shooter.AddComponent<TestModifier>();
			shooter.AddComponent<TestModifier>();
			// attach a weapon to the object that modifies the range which will be ignored
			var extraWep = shooter.AddComponent<WARRangedWeapon>() as WARRangedWeapon;
			extraWep.range = 1;
			
			// a place to store the result
			var attack = new WARShootingAttack(shooter.GetComponent<WARUnit>() , weap);
			
			// compute the final shooting attack
			var final = attack.computeAttack();
			
			// make sure the internal tracker has the right values
			Assert.AreEqual(3, final.range);
		}
		
		[Test]
		public void AddsDefense() {
			// a weapon to test with
			var weap = new GameObject().AddComponent<TestModifier>() as TestModifier;
			// the shooter
			var shooter = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			);
			// attach a few more instances of the modifier to the object
			shooter.AddComponent<TestModifier>();
			shooter.AddComponent<TestModifier>();
			
			// a place to store the result
			var attack = new WARShootingAttack(shooter.GetComponent<WARUnit>() , weap);
			
			// the target
			var target = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop")
			);
			// attach a piece of shooting armor to the target
			target.AddComponent<TestTargetModifier>();
			target.AddComponent<TestTargetModifier>();
			
			// a place to store the result
			var final = attack.computeFinalAttack(target.GetComponent<WARUnit>());
			
			// make sure the internal tracker has the right values
			Assert.AreEqual(5, final.range);
		}
	}
}
