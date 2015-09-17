using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour
{
	private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
	public static Vector3 INVALID_POSITION {get{return invalidPosition;}}

	private static int nextObjectId = 0;

	private static GameObjectList gameObjectList;

	public static void SetGameObjectList(GameObjectList objectList)
	{
		gameObjectList = objectList;
	}
	
	public static GameObject GetUnit(string name) {
		return gameObjectList.GetUnit(name);
	}

	public static GameObject GetBuilding(string name)
	{
		return gameObjectList.GetBuilding (name);
	}
	
	public static GameObject GetWorldObject(string name) {
		return gameObjectList.GetWorldObject(name);
	}
	
	public static GameObject GetPlayerObject() {
		return gameObjectList.GetPlayerObject();
	}

	public static int GetNewObjectId()
	{
		nextObjectId++;
		return nextObjectId;
	}
}

