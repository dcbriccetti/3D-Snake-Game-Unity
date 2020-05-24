using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public GameObject controlTile;
	public Transform target;
	Transform t;

	/// pitch, yaw (roll omitted)
	public Vector2 cameraRotation;
	public Vector3 mouseSensitivity = Vector3.one * 5;
	public float distanceFromTarget = 10;

	private bool controlTileDirectionNeedsCalculation = true;

	public SnakeAgent snake;

    void Start()
    {
		t = transform; // cached, because 'transform' is actually a function call with a cost that grows with component count.
		if (!snake) {
			Debug.LogWarning(name+" controller has no snake to control!");
		}
    }

	Vector3 BestCardinalDirection(Vector3 lookDirection)
	{
		Vector3 dir = Arena.directions[0];
		float bestAlignment = Vector3.Dot(lookDirection, dir);
		for (int i = 1; i < Arena.directions.Length; ++i) {
			float alignment = Vector3.Dot(lookDirection, Arena.directions[i]);
			if(alignment > bestAlignment) {
				bestAlignment = alignment;
				dir = Arena.directions[i];
			}
		}
		return dir;
	}

	void FixedUpdate() // happens at very regular intervals, usually slower than Update
	{
		if (controlTileDirectionNeedsCalculation)
		{
			Vector3 f = BestCardinalDirection(t.forward);
			Vector3 u = BestCardinalDirection(t.up);
			Quaternion moveRotation = Quaternion.LookRotation(f, u);
			controlTile.transform.rotation = moveRotation;
			controlTileDirectionNeedsCalculation = false;
		}
		if (snake) {
			float horizontal = Input.GetAxisRaw("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");
			if((horizontal != 0) != (vertical != 0)) {
				if(horizontal != 0) {
					int sign = horizontal > 0 ? 1 : -1;
					snake.direction = controlTile.transform.rotation * (Vector3.right * sign);
				} else if(vertical != 0) {
					int sign = vertical > 0 ? 1 : -1;
					snake.direction = controlTile.transform.rotation * (Vector3.up * sign);
				}
				controlTile.transform.position = snake.positions[0] + snake.direction;
			}
		}
	}

	void Update() // happens every animation frame, not terribly time bound
    {
		float mouseHorizontal = Input.GetAxis("Mouse X");
		float mouseVertical = Input.GetAxis("Mouse Y");
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if(mouseHorizontal != 0 || mouseVertical != 0) {
			controlTileDirectionNeedsCalculation = true;
		}
		cameraRotation.x -= mouseVertical * mouseSensitivity.y; // inverting y axis, 
		cameraRotation.y += mouseHorizontal * mouseSensitivity.x;
		distanceFromTarget += mouseWheel * mouseSensitivity.z;
	}

	private void LateUpdate() // happens every frame JUST before the render call. will cause stuttering if this takes too long.
	{
		Quaternion nextRotation = Quaternion.identity;
		nextRotation *= Quaternion.Euler(cameraRotation);
		t.rotation = nextRotation;
		t.position = target.position - t.forward * distanceFromTarget;
	}
}
