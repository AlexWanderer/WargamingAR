using UnityEngine;
using UniRx;

namespace WAR.Tools
{
    /// <summary>
    /// Base manager class that inforces a self-referencing singleton pattern.
    /// Your class should derive from this, not directly from the non-generic Manager.
    /// </summary>
    /// <typeparam name="T">Self</typeparam>
    public abstract class Manager<T> : MonoBehaviour, IManager where T : Manager<T>
    {
	    public static T Instance;
	    
	    // collect all of our disposables together so we can disable them as a group
	    protected CompositeDisposable disposables = new CompositeDisposable();

        protected Manager() { }

        /// <summary>
        /// Called when a deserialized version is loaded.
        /// </summary>
        public void Deserialize()
        {
	        Instance = (T)this;
        }
	    // when the object is destroyed
	    public void OnDestroy() {
			// make sure to clean up any subscriptions
		    disposables.Dispose();
	    }
    }
}