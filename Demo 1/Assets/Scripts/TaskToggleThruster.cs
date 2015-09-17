using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TaskMove))]
public class TaskToggleThruster : Task
{
	private TaskMove taskMove;
	private GameObject particles;
	private Thruster thruster;
	
	private void Awake()
	{
		taskMove = GetComponent<TaskMove> ();
		particles = transform.FindChild ("Afterburner").gameObject;
		thruster = GetComponent<Thruster> ();
	}
	
	public override IEnumerator Execute ()
	{
		taskMove.SetCrew (crew);
		StartCoroutine (taskMove.Execute());
		while(!taskMove.done)
		{
			yield return new WaitForSeconds(1);
		}

		taskMove.done = false;

		thruster.isOn = !thruster.isOn;

		particles.SetActive (thruster.isOn);
		
		done = true;
		Finish ();
	}
}

