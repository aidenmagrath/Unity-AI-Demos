using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour
{
	private Player player;

	private void Awake()
	{
		player = GetComponent<Player> ();
	}

	private void Update()
	{
		if(player.human)
		{
			if(Input.GetMouseButtonDown(0))
			{
				LeftMouseClick();
			} else if(Input.GetMouseButtonDown(1))
			{
				RightMouseClick();
			}
		} 
		else
		{

		}
	}

	private void LeftMouseClick()
	{
		if(player.findingPlacement)
		{
			if(player.CanBuild())
			{
				player.StartConstruction();
			}
		}
		else
		{
			GameObject hitObject = FindHitObject();
			Vector3 hitPoint = FindHitPoint();
			if (hitObject && hitPoint != ResourceManager.INVALID_POSITION) {
				if(player.selectedObject)
				{
					player.selectedObject.MouseClick(hitObject, hitPoint, player);
				} else if(hitObject.tag != "Ground")
				{
					WorldObject worldObject = hitObject.transform.GetComponent<WorldObject>();
					if(worldObject)
					{
						player.selectedObject = worldObject;
						worldObject.SetSelected(true);
					}
				}
			}
		}

	}

	private void RightMouseClick()
	{
		player.selectedObject.SetSelected(false);
		player.selectedObject = null;
		HUD.GetHUD ().ShowStats (false);
	}

	private GameObject FindHitObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			return hit.collider.gameObject;
		}
		return null;
	}

	private Vector3 FindHitPoint()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			return hit.point;
		}
		return ResourceManager.INVALID_POSITION;
	}
}

