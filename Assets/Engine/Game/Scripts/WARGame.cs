using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx;

using WAR.Tools;

namespace WAR.Game
{
	
	public enum GAME_PHASE {
		none,
		movement,
		command,
		shooting,
		assault,
		morale,
	}
	
	public enum GAME_MODE {
		none,
		setup,
		deployment,
		gameplay,
		score,
	}
	
	public struct Epoch<T> {
		public T last;
		public T current;
		
		public Epoch(T _curr) {
			this.last = default(T);
			this.current = _curr;
		}
		
		public Epoch(T _last, T _curr) {
			this.last = _last;
			this.current = _curr;
		}
	}
	
    /// <summary>
    /// Entry point of all game-wide serialized data.
    /// The WARGame is a self-regulating script.
    /// When awaken, it loads all the possible game Managers.
    /// </summary>
    [AddComponentMenu("")]
	public sealed class WARGame : MonoBehaviour
	{
		// where the managers are stored in Assets/Resources/
	    public const string PATH = "Managers/";
		// the list of managers that we have spawned, found in PATH
        private static List<IManager> managers = new List<IManager>();
		// static reference to the set of managers we have spawned
        public static IManager[] Managers
        {
            get { return managers.ToArray(); }
        }
		// singleton logic
	    private static WARGame instance;
	    public static WARGame Instance
        {
            get { return instance; }
        }
		
		// reactive collection of the players we have spawned in the game
		public ReactiveCollection<WARPlayer> players = new ReactiveCollection<WARPlayer>();
	    
		// the phase of the game, the current step in the turn
	    public static ReactiveProperty<Epoch<GAME_PHASE>> Phase = new ReactiveProperty<Epoch<GAME_PHASE>>();
		// the mode of the game we are in
	    public static ReactiveProperty<Epoch<GAME_MODE>> Mode = new ReactiveProperty<Epoch<GAME_MODE>>();
	    
	    public static void SetPhase(GAME_PHASE newPhase) {
			// set the current game phase
		    Phase.SetValueAndForceNotify(new Epoch<GAME_PHASE>(Phase.Value.current, newPhase));
	    }
	    public static void SetMode(GAME_MODE newMode) {
			// set the current game mode
		    Mode.SetValueAndForceNotify(new Epoch<GAME_MODE>(Mode.Value.current, newMode));
	    }
	    
	    private void Awake() {
	    	// start off in the deployment mode
	    	Mode.SetValueAndForceNotify(new Epoch<GAME_MODE>(GAME_MODE.setup));
	    }
	    

        private void Start()
        {
            if (instance != null) // Frankly, this should not happen. Someone made an error otherwise.
            {
                Destroy(gameObject);
                return;
            }
            else
                instance = this;

            Deserialize(this);
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Used on editor only, to load a manager.
        /// </summary>
        public static void EditorLoad(Type type)
        {
            if (Application.isPlaying || !typeof(IManager).IsAssignableFrom(type))
                return;

            GameObject go = Resources.Load(PATH + type.Name) as GameObject;
            go = Instantiate(go) as GameObject;
            IManager manager = go.GetComponent(type) as IManager;
            manager.Deserialize();
            go.hideFlags = HideFlags.HideAndDontSave;
        }

        /// <summary>
        /// Retrieve all the Manager and their data.
        /// If data is inexistant, create a new one.
        /// </summary>
	    private static void Deserialize(WARGame WAR)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IManager).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        GameObject go = Resources.Load(PATH + type.Name) as GameObject;
                        if (go != null)
                        {
                            GameObject clone = Instantiate(go) as GameObject;
                            clone.name = type.Name;
                            clone.transform.parent = Instance.gameObject.transform;

                            IManager manager = clone.GetComponent(type) as IManager;
                            if (manager != null)
                            {
                                RemoveExisting(type);
                                manager.Deserialize();
                                managers.Add(manager);
                            }
                        }
                    }
                }
            }
        }

        private static void RemoveExisting(Type type)
        {
            for (int i = 0; i < managers.Count; i++)
                if (managers[i].GetType() == type)
                    managers.RemoveAt(i);
        }
    }
}