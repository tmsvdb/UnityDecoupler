# UnityDecoupler
The unity decoupler is a game object manager used for decoupling your game code from the Unity scene. To decouple your script from the unity scene the UnityDecoupler calls a Main class that users have to implement treirselves. The main class can then be used to write your own code. The instance of the UnityDecoupler that is passed through the Main class can be used to speak with the unity graphics. 

## Main class 
in order for this tool to work, users have to add a custom class named Main.cs to their scripts in unity. The Main class is provided with an interface of the UnityDecoupler, so users can communicate with GameObject's within Unity.
This class should look the following:
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Unity_Decoupler;

    public class Main {

      public Main (IUnityDecoupler decoupler)
        {
            decoupler.AddUpdateTrigger(GameLoop);
        }

        public void GameLoop (float deltaTime)
        {
            Debug.Log("game loop: " + deltaTime.ToString());
        }
    }


## Interface
In order to use the UnityDecoupler to manage your game's graphics their are a couple of methodes availeble for use.

Methode | Description
------------ | -------------
void AddUpdateTrigger(Action\<float\> onUpdateFrame) | Add an update frame event listsener
void RemoveUpdateTrigger(Action\<float\> onUpdateFrame) | Remove an update frame event listsener
GameObject Link(string gameObjectName) | Link an existing object in the scene the the local spawn list
GameObject Spawn(string prefabName, string instanceName) | Spawn a new instance of a prefab into the scene
bool AddPrefab(GameObject prefabObject) | Add a new object into the local prefab list
GameObject GetSpawn(string instanceName) | Get a spawned object from the scene
GameObject GetPrefab(string prefabName) | Get a prefab from the local prefab list
void Remove(string name) | Remove a prefab from the scene
GameObject LoadPrefab(string path) | Load a prefab from recources into local prefab list

###void AddUpdateTrigger(Action\<float\> onUpdateFrame)
  Add an update frame event listsener

  @param <b>onUpdateFrame</b>:
    is a methode that is called each frame
    and passes a parameter deltaTime as a float

###void RemoveUpdateTrigger(Action\<float\> onUpdateFrame)
  Remove an update frame event listsener

  @param <b>onUpdateFrame</b>:
      is the methode that needs to be removed. By calling this the methode is 
      no longer updated each frame if it was previously added.

###GameObject Link(string gameObjectName)
  Link an existing object in the scene the the local spawn list

  The scen might contain game object that are not spawned by this system,
  but might have alllready been added to the scene. For example the Main Camara.
  By using this methode the example camera is added to the local spawn list,
  an can be extracted from this list. Using Get spawn.

  @param <b>gameObjectName</b>:
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

###GameObject Spawn(string prefabName, string instanceName)
  Spawn a new instance of a prefab into the scene

  This methode takes a gameobject from the local prefab list and instantiate's it 
  into the scene.

  @param <b>prefabName</b>:
      the name of the GameObject that is stored in the local prefap
      list which you are trying to spawn into the scene.

  @paran <b>instanceName</b>:
      is the name of the spawn in the local spawn list

  @returns:
      the newly created instance of an existing prefab.

  @returns:
      a previously spawned game object if a spawn with the same instance name 
      allready exist. In this case no new instance is created.

  @returns:
      NULL if prefab does not exist.

###bool AddPrefab(GameObject prefabObject)
  Add a new object into the local prefab list

  you can create your own game object (or use returned gameo bjects) and add them
  to the local prefab list, so you can spawn instances of this prefab on the fly.

  @param <b>prefabObject</b>:
      the GameObject added to the local prefab list. The name of this
      game object must be unique in order for this methode to work.

  @returns:
      true if the game object was succesfully added to the local prefab list.

  @returns:
      false if a game object with the same name allready exists within the
      local prefab list.

###GameObject GetSpawn(string instanceName)
  Get a spawned object from the scene

  @param <b>instanceName</b>:
      name of the spawn, that has been stored within the local spawn list, you 
      are trying to get.

  @returns:
      the requested game object from the local spaw list.

  @returns:
      NULL if the spawn with the requested name was not found!

###GameObject GetPrefab(string prefabName)
  Get a prefab from the local prefab list

  @param <b>prefabName</b>:
      the name of the game object (gameobject.name), that has been stored within the 
      local prefab list, you are trying to get.

  @returns:
      the requested prefab game object from the local prefab list.

  @returns:
      NULL if the prefab with the requested name was not found!

###void Remove(string name)
  Remove a prefab from the scene

  @param <b>instanceName</b>:
      the name of the previously spawned instance that needs to be removed.
      if this instance does not exist in the local spawn list, nothing will happen.

###GameObject LoadPrefab(string path)
  Load a prefab from recources into local prefab list

  @param <b>path</b>:
      path name within the resources folder see unity's documentation on
      Resources.Load

  @returns:
      the newly loaded game object fetched from the resources folder.

  @returns:
      if a prefab allready exists within the local prefab list, this existing
      game object will be returned and the load process will be skipped!
