using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
	[SerializeField]
	private List<Task> tasks;

	/// <summary>
	/// Adds a task to the tasks list which will be assigned to the next avaliable Crew member. 
	/// </summary>
	/// <param name="newTask">The task to be added.</param>
	public void AddTask(Task newTask)
	{
		print ("AddTask method called");
		print ("Tasks Size: " + tasks.Count);
		if (tasks.Count > 0) {
			print("Tasks count > 0");
			for (int i = 0; i < tasks.Count; i++) {
				print ("Checking 1: " + i);
				Task task = tasks [i];
				if (task.priority < newTask.priority) {
					print("Inserting task");
					tasks.Insert (i, newTask);
				} else if (i == tasks.Count - 1) {
					print("Adding task");
					tasks.Add (newTask);
				}

			}
		} else
		{
			print ("Else called");
			tasks.Add(newTask);
			print("Task added - count: " + tasks.Count);
		}
	}

	/// <summary>
	/// Gets the next task waiting to be completed which corresponds to the same job as the specified CrewType. 
	/// The tasks are ordered by their specified priority, and will be removed from the tasks list after returning.
	/// If no tasks are waiting to be completed with the given CrewType, null will be return.
	/// </summary>
	/// <returns>The task.</returns>
	/// <param name="type">The CrewType of the task to be fetched.</param>
	public Task GetTask(CrewType type)
	{

		for(int i = 0; i < tasks.Count; i++)
		{
			print("Getting tasks: " + i);
			Task task = tasks[i];
			if(task.crewTypes.Contains(CrewType.Anyone) || task.crewTypes.Contains(type))
			{
				print("Return task");
				tasks.RemoveAt(i);
				return task;
			}
		}

		return null;
	}

	/// <summary>
	/// Returns an instance of TaskManager.
	/// </summary>
	/// <returns>The manager.</returns>
	public static TaskManager GetManager()
	{
		return GameObject.FindGameObjectWithTag ("GameManager").GetComponent<TaskManager> ();
	}
}


