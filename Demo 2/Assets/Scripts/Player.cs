using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public string playerName;
	public bool human;
	public WorldObject selectedObject;
	public Material allowedMaterial;
	public Material notAllowedMaterial;

	private Building tempBuilding;
	public bool findingPlacement;
	public Color playerColor;

	private void Update()
	{
		if(human)
		{
			if(findingPlacement)
			{
				UpdatePlacementPosition();
				if(tempBuilding.IsColliding())
				{
					tempBuilding.SetTransparentMaterial(notAllowedMaterial, false);
				} else
				{
					tempBuilding.SetTransparentMaterial(allowedMaterial, false);
				}
			}
		}

	}

	public void CreateBuilding(string buildingName)
	{
		findingPlacement = true;
		GameObject newBuilding = (GameObject) Instantiate(ResourceManager.GetBuilding(buildingName), ResourceManager.INVALID_POSITION, Quaternion.identity);
		tempBuilding = newBuilding.GetComponent<Building> ();
		if(tempBuilding != null)
		{
			tempBuilding.objectId = ResourceManager.GetNewObjectId();
			tempBuilding.hitPoints = 0;
			tempBuilding.SetTransparentMaterial(notAllowedMaterial, true);
			tempBuilding.SetCollidersAsTrigger(true);
			findingPlacement = true;
		} else
		{
			Destroy(newBuilding);
		}
	}

	public void StartConstruction()
	{
		findingPlacement = false;
		tempBuilding.transform.parent = transform.FindChild ("Buildings");
		tempBuilding.player = this;
		tempBuilding.SetCollidersAsTrigger (false);
		tempBuilding.StartContruction ();
	}

	public bool CanBuild()
	{
		return !tempBuilding.IsColliding ();
	}

	private void UpdatePlacementPosition()
	{
		int layerMask = 1 << 8;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, Mathf.Infinity, layerMask))
		{
			if(hit.transform.tag == "Ground")
			{
				tempBuilding.transform.position = hit.point;
			}
		}
	}
}