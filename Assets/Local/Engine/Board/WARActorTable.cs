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
            // find a random planet from the list of prefabs
            GameObject planet = planetPrefabs[UnityEngine.Random.Range(0, planetPrefabs.Count)];

            // attach the planet to the table
            planet = GameObject.Instantiate(planet, transform);
        }
	}
}