using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

namespace WAR.Board {
	public class WARActorCell : MonoBehaviour {	
		// the unique id for the cell
		public int id;
		
		// materials to use for the selected state
		public Material highlightedMaterial;
		public Material defaultMaterial;
		
		// the objects in the cell
		private WARGridObject entry;
		public ReactiveCollection<WARGridObject> objects = new ReactiveCollection<WARGridObject>();
		public BoolReactiveProperty highlighted = new BoolReactiveProperty(false);
		
		// store our neighbors for pathing reference
		public List<int> neighbors = new List<int>();
		
		Color defaultColor;
		
		public WARActorCell Init() {
			objects.ObserveAdd().Subscribe(PlaceObjectOnCell);
			highlighted.Subscribe(onHighlightedChanged);
	
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
		
		private void onHighlightedChanged(bool newVal) {
			// if we are supposed to highlight the cell
			if (newVal) {
				GetComponent<MeshRenderer>().material = highlightedMaterial;
			}
			// otherwise we are supposed to remove the highlighting style
			else {
				GetComponent<MeshRenderer>().material = defaultMaterial; 
			}
		}
	
	}
}
