using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WARDamagable : MonoBehaviour {
	
	[SerializeField]
	public int maxHealth;
	private int currentHealth;
	
	public void Start() {
		// start off at max health
		currentHealth = maxHealth;
	}
	
	public void takeDamage(int damage) {
		// for now, just decrement the health
		maxHealth -= damage;
	}
}
