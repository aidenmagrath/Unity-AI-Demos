using UnityEngine;
using System.Collections;

public class Thruster : BaseObject
{
	public bool isOn;
	public float powerUsage = 0.2f;

	private Ship ship;

	public void Awake()
	{
		taskType = "TaskToggleThruster";
		ship = GameObject.Find ("Ship").GetComponent<Ship> ();
	}

	private void Start()
	{
		StartCoroutine (UsePower ());
	}

	private IEnumerator UsePower()
	{
		while(true)
		{
			if(isOn)
			{
				if(ship.power >= powerUsage)
					ship.power -= powerUsage;
			}

			yield return new WaitForSeconds(0.1f);
		}
	}
}

