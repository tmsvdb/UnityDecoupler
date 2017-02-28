using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Unity_Decoupler
{
    public interface IUnityDecoupler
    {
        /*
            Add an update frame event listsener

            @param onUpdateFrame is a methode that is called each frame
            and passes a parameter deltaTime as a float
        */
        void AddUpdateTrigger(Action<float> onUpdateFrame);

        /*
            Remove an update frame event listsener

            @param onUpdateFrame:
                is the methode that needs to be removed. By calling this the methode is 
                no longer updated each frame if it was previously added.
        */
        void RemoveUpdateTrigger(Action<float> onUpdateFrame);

        /*
            Link an existing object in the scene the the local spawn list

            The scen might contain game object that are not spawned by this system,
            but might have alllready been added to the scene. For example the Main Camara.
            By using this methode the example camera is added to the local spawn list,
            an can be extracted from this list. Using Get spawn.

            @param gameObjectName:
                is the name of the object in the scene that needs to be added to the spawn 
                list. NOTE: The name of this object will not be changed and there can be no 
                double names in the spawnlist!

            @returns:
                the game object from the scene for fast reference. It is quicker to use
                the returned object and store its reference where you need it, than to call the 
                GetSpawn methode, which needs to itterate through all existing spawns in the local
                spawn list.

            @returns:
                game object from spawn list if a game object with the given name allready 
                exists in the spawn list, instead of linking the scene object!

            @returns:
                NULL if there is no game object in the scene with the given name 
        */
        GameObject Link(string gameObjectName);

        /*
            Spawn a new object into the scene

            This methode takes a gameobject from the local prefab list and instantiate's it 
            into the scene.

            @param prefabName:
                the name of the GameObject that is stored in the local prefap
                list which you are trying to spawn into the scene.

            @paran instanceName:
                is the name of the spawn in the local spawn list

            @returns:
                the newly created instance of an existing prefab.

            @returns:
                a previously spawned game object if a spawn with the same instance name 
                allready exist. In this case no new instance is created.

            @returns:
                NULL if prefab does not exist.

        */
        GameObject Spawn(string prefabName, string instanceName);

        /*
            Add a new object into the local prefab list

            you can create your own game object (or use returned gameo bjects) and add them
            to the local prefab list, so you can spawn instances of this prefab on the fly.

            @param prefabObject:
                the GameObject added to the local prefab list. The name of this
                game object must be unique in order for this methode to work.

            @returns:
                true if the game object was succesfully added to the local prefab list.

            @returns:
                false if a game object with the same name allready exists within the
                local prefab list.
        */
        bool AddPrefab(GameObject prefabObject);

        /*
            Get a spawned object from the scene

            @param instanceName:
                name of the spawn, that has been stored within the local spawn list, you 
                are trying to get.

            @returns:
                the requested game object from the local spaw list.
            
            @returns:
                NULL if the spawn with the requested name was not found!
        */
        GameObject GetSpawn(string instanceName);

        /*
            Get a prefab from the local prefab list

            @param prefabName:
                the name of the game object (gameobject.name), that has been stored within the 
                local prefab list, you are trying to get.
            
            @returns:
                the requested prefab game object from the local prefab list.
            
            @returns:
                NULL if the prefab with the requested name was not found!
        */
        GameObject GetPrefab(string prefabName);

        /*
            Remove a prefab from the scene

            @param instanceName:
                the name of the previously spawned instance that needs to be removed.
                if this instance does not exist in the local spawn list, nothing will happen.
        */
        void Remove(string name);

        /*
            Load a prefab from recources into local prefab list

            @param path:
                path name within the resources folder see unity's documentation on
                Resources.Load
            
            @returns:
                the newly loaded game object fetched from the resources folder.

            @returns:
                if a prefab allready exists within the local prefab list, this existing
                game object will be returned and the load process will be skipped!
        */
        GameObject LoadPrefab(string path);
    }

    public class UnityDecoupler : MonoBehaviour, IUnityDecoupler
    {
        private Action<float> updateTrigger;
        public List<GameObject> prefabList; 
        private Dictionary<string, GameObject> spawnList;

        void Start ()
        {
            spawnList = new Dictionary<string, GameObject>();

            if (prefabList == null)
                prefabList = new List<GameObject>();

            Main main = new Main(this);
        }

        void Update ()
        {
            if (updateTrigger != null)
                updateTrigger(Time.deltaTime);
        }

        public void AddUpdateTrigger(Action<float> onUpdateFrame)
        {
            updateTrigger += onUpdateFrame;
        }

        public void RemoveUpdateTrigger(Action<float> onUpdateFrame)
        {
            updateTrigger -= onUpdateFrame;
        }

        public GameObject Link(string gameObjectName)
        {
            GameObject go;

            if (!spawnList.TryGetValue(gameObjectName, out go))
            {
                go = GameObject.Find(gameObjectName);

                if (go != null)
                    spawnList.Add(gameObjectName, go);
            }

            return go;
        }

        public GameObject Spawn(string prefabName, string instanceName)
        {
            GameObject prefab = GetPrefab(prefabName);
            GameObject go = GetSpawn(instanceName);

            if (prefab != null && go == null)
            {
                go = Instantiate(prefab);
                spawnList.Add(instanceName, go);
            }

            return go;
        }

        public GameObject GetSpawn(string instanceName)
        {
            GameObject go;
            spawnList.TryGetValue(instanceName, out go);
            return go;
        }

        public GameObject GetPrefab(string prefabName)
        {
            GameObject go = null;

            foreach (GameObject prefab in prefabList)
                if (prefab.name == name)
                    go = prefab;

            return go;
        }

        public void Remove(string name)
        {
            GameObject go = GetSpawn(name);

            if (go != null)
            {
                Destroy(go);
                spawnList.Remove(name);
            }
        }

        public GameObject LoadPrefab(string path)
        {
            GameObject prefab = GetPrefab(path);

            if (prefab == null)
            {
                prefab = Resources.Load(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                    AddPrefab(prefab);
            }

            return prefab;
        }

        public bool AddPrefab(GameObject prefabObject)
        {
            // check if prefab allready exists, if so return false;
            foreach (GameObject prefab in prefabList)
                if (prefab.name == prefabObject.name)
                    return false;

            prefabList.Add(prefabObject);
            return true;
        }
    }
}
