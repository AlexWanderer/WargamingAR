using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WAR.Board {
	public class WARActorCell : MonoBehaviour {	
		// the unique id for the cell
		public int id;
		
		// the objects in the cell
		private WARGridObject entry;
		public WARGridObject objects {
			set {
				// if we were given a new object
				if (entry != value) {
					// if that object has not been instantiated
					entry = value;
					entry.transform.position = transform.position;
				}
			}
		}
	}
}
