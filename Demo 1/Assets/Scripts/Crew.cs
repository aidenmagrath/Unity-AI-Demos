using UnityEngine;
using System.Collections;

public class Crew : MonoBehaviour
{

	public bool idle = true;
	public CrewType type;

	public GameObject rubish;
	public float rubishFrequency = 10.0f;

	private NavMeshAgent navAgent;

	private void Awake()
	{
		navAgent = GetComponent<NavMeshAgent> ();
	}

	private void Start()
	{
		if(type != CrewType.Cleaner)
		{
			StartCoroutine (CreateRubish ());
		}
	}

	/// <summary>
	/// Randomly instantiates Rubish objects while the Crew member is walking. rubishFrequency is used to calculate how often rubish should be spawnened.
	/// </summary>
	/// <returns>The rubish.</returns>
	private IEnumerator CreateRubish()
	{
		while(true)
		{
			if(navAgent.acceleration > 0.1)
			{
				if(Random.Range(0, 100) < rubishFrequency)
				{
					Instantiate(rubish, new Vector3(transform.position.x, -0.4f,transform.position.z), Quaternion.identity);
				}
			}
			yield return new WaitForSeconds(1);
		}
	}
}

public enum CrewType
{
	Anyone,
	Medical,
	Engineer,
	Cleaner
}

