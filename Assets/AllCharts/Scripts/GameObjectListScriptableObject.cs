using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameObjectList", menuName = "ScriptableObjects/GameObjectList", order = 1)]
public class GameObjectListScriptableObject : ScriptableObject
{
    public List<GameObject> gameObjectList = new List<GameObject>();
}
