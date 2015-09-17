using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Crew))]
public class TaskAssigner : MonoBehaviour
{
	private Crew crew;	

	private void Awake()
	{
		crew = GetComponent<Crew> ();
	}

	private void Update()
	{
		if(crew.idle)
		{
			print("I'm idle");
			Task task = TaskManager.GetManager().GetTask(crew.type);
			if(task != null)
			{
				print("Found Task");
				crew.idle = false;
				task.SetCrew(crew);
				StartCoroutine(task.Execute());
			}
		}
	}
}

