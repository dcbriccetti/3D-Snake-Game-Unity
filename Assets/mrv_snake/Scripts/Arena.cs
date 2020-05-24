using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
	// intentionally [X, Y, Z, -X, -Y, -Z]
	public static readonly Vector3[] directions = { Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back };
	string[] dirNames = { "right", "up", "forward", "left", "down", "back" };
	GameObject[] walls;

	public Vector3 size = Vector3.one * 10;

	void CreateWalls()
	{
		Vector3 extents = size / 2;
		for(int i = 0; i < directions.Length; ++i)
		{
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
			wall.name = dirNames[i];
			Transform t = wall.transform;
			int dir = i % 3;
			t.position = directions[i] * size[dir];
			bool isZAligned = i != 2;
			if (!isZAligned) {
				t.rotation = Quaternion.LookRotation(directions[i], Vector3.forward);
			} else {
				t.rotation = Quaternion.LookRotation(directions[i], Vector3.up);
			}
			Vector3 scale = Vector3.one;
			switch (dir) {
				case 0: scale = new Vector3(size.z, size.y, 1); break;
				case 1: scale = new Vector3(size.x, size.z, 1); break;
				case 2: scale = new Vector3(size.x, size.y, 1); break;
			}
			t.localScale = scale*2;
		}
	}

    void Start()
    {
		CreateWalls();        
    }
}
