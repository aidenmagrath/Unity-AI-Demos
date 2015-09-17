using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Reactor : BaseObject {

	public float generateInterval = 0.1f;
	public float generateAmount = 0.5f;
	public float decreaseAmount = 0.1f;

	public bool isGenerating = false;

	private Ship ship;

	public void Awake()
	{
		taskType = "TaskToggleReactor";
		ship = GameObject.Find ("Ship").GetComponent<Ship> ();
	}

	public void Start()
	{
		StartCoroutine (GeneratePower());
	}

	/// <summary>
	/// Generates power if the reactor is turned on, otherwise decreases power.
	/// </summary>
	/// <returns>The power.</returns>
	private IEnumerator GeneratePower()
	{
		while(true)
		{
			if(isGenerating)
			{
				if(ship.power <= (100 - generateAmount))
					ship.power += generateAmount;
			}

			yield return new WaitForSeconds(generateInterval);
		}
	}
}
