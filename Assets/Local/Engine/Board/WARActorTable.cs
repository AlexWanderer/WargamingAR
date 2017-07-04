using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.UI;

namespace WAR.Engine.Board {

    public enum GRID_TYPE {
        hex,
        square,
        grav
    };
	
    [RequireComponent(typeof(WARControlBoard))]
	public class WARActorTable : MonoBehaviour {

		public void initialize(UIPlane plane, GRID_TYPE cellType){
			
        }
	}
}