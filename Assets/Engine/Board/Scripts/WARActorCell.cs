using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

namespace WAR.Board {
	public class WARActorCell : MonoBehaviour {	
		// the unique id for the cell
		public int id;
		
		// the objects in the cell
		private WARGridObject entry;
		public ReactiveCollection<WARGridObject> objects = new ReactiveCollection<WARGridObject>();
		
		
		public WARActorCell Init() {
			objects.ObserveAdd().Subscribe(PlaceObjectOnCell);
			return this;
		}
		
		void PlaceObjectOnCell(CollectionAddEvent<WARGridObject> gridObject) {
			// if the passed object is not a WARGridObject
			if (gridObject.Value == null) {
				Debug.LogError("Tried to add a non-WARGridObject to a cell.");
				return;
			}
			
			// we know the object is a WARGridObject so apply the correct transform
			gridObject.Value.transform.position = transform.position;
		}
	
	}
}
