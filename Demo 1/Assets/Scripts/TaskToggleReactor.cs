using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TaskMove))]
public class TaskToggleReactor : Task
{
	private TaskMove taskMove;
	
	private void Awake()
	{
		taskMove = GetComponent<TaskMove> ();
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
		
		Material m = GetComponent<Renderer> ().material;

		Reactor reactor = GetComponent<Reactor> ();
		reactor.isGenerating = !reactor.isGenerating;

		if (reactor.isGenerating) {
			m.color = Color.green;
		} else {
			m.color = Color.red;
		}

		done = true;
		Finish ();
	}
}

