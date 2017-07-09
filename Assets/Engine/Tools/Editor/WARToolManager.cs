using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using Sirenix.OdinInspector;

#if !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine.SceneManagement;
#endif

namespace WAR {
    /// <summary>
    /// This simple tool is there to guaranty that one WAR exist at all time.
    /// Should a new Manager be found, it saves it as an Asset.
    /// </summary>
    [InitializeOnLoad]
	public class WARToolManager {
	    static WARToolManager() {
            EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
            PlaymodeStateChanged();
	    }
		
		private static void DeleteDirectory(string target_dir) {
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);
			
			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}
			
			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}
			
			Directory.Delete(target_dir, false);
		}
		
		[MenuItem("WAR/InitResources")]
		public static void InitPrefabs() {
			
			// the directory with all of our managers
			var managerDir = Application.dataPath + "/Resources/Managers/";
			
			// if we have an old version of the managers dir
			if (Directory.Exists(managerDir)) {
				// delete the directory and its contents
				DeleteDirectory(managerDir);
			} 
			  
			// make an empty directory for our managers
			Directory.CreateDirectory(managerDir);
			
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (Type type in assembly.GetTypes())  {
					if (typeof(IManager).IsAssignableFrom(type) && !type.IsAbstract) {
						GameObject go = Resources.Load(WARGame.PATH + type.Name) as GameObject;
						
						// if we were able to instantiate an object from the manager, it already exists
						if (go != null) continue;
							
						go = new GameObject(type.Name);
						go.AddComponent(type);
						
						PrefabUtility.CreatePrefab("Assets/Resources/Managers/" + type.Name + ".prefab", go.gameObject);
						GameObject.DestroyImmediate(go);
					}
				}
			}
		}
		
        private static void PlaymodeStateChanged() {
            if (Application.isPlaying) {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                if (Application.loadedLevel != 0)
#else
                if (SceneManager.GetActiveScene().buildIndex != 0) 
#endif
                {
	                GameObject go = new GameObject("WARGame");
	                go.AddComponent<WARGame>();
                }
            }
        }
    }
}