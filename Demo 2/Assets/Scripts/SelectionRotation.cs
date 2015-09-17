using UnityEngine;
using System.Collections;

public class SelectionRotation : MonoBehaviour {

	public float rotationSpeed = 5.0f;

	void Update () {
		transform.RotateAround (transform.position, transform.up, rotationSpeed * Time.deltaTime);
	}
}
