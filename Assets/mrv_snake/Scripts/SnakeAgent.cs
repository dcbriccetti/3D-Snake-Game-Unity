using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAgent : MonoBehaviour
{
	public Color color = Color.green;
	public Vector3 direction;
	public float delayBetweenAnimations = 0.75f;
	public float animationDuration = 0.25f;

	float timer;

	public GameObject segmentPrefab;
	
	/// position of snake segments
	public List<Vector3> positions;
	/// graphical objects showing snake segments
	public List<GameObject> segmentGraphics = new List<GameObject>();

	public enum State { waiting, animating }
	public State state;

	public int size = 3;

	public float maxSegmentScale = 1.25f;
	public float minSegmentScale = 0.5f;

	void RescaleSegments()
	{
		for(int i = 0; i < segmentGraphics.Count; ++i) {
			segmentGraphics[i].transform.localScale = Vector3.one * Mathf.Lerp(maxSegmentScale, minSegmentScale, (float)i / segmentGraphics.Count);
		}
	}

    void Start()
    {
        for(int i = 0; i < size; ++i) {
			positions.Add(Vector3.zero);
			GameObject seg = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity);
			seg.GetComponent<Renderer>().material.color = color;
			segmentGraphics.Add(seg);
		}
		RescaleSegments();
	}

    void Update()
    {
		timer += Time.deltaTime;
		switch (state) {
			case State.waiting:
				if(timer >= delayBetweenAnimations) {
					timer -= delayBetweenAnimations;
					state = State.animating;
				}
				break;
			case State.animating:
				if(timer < animationDuration) {
					float progress = timer / animationDuration;
					Vector3 segmentGoingTo = positions[0] + direction, segmentAt;
					for (int i = 0; i < segmentGraphics.Count; ++i)
					{
						segmentAt = positions[i];
						segmentGraphics[i].transform.position = Vector3.Lerp(segmentAt, segmentGoingTo, progress);
						segmentGoingTo = segmentAt;
					}
				} else {
					positions.Insert(0, positions[0] + direction);
					positions.RemoveAt(positions.Count - 1);
					for (int i = 0; i < segmentGraphics.Count; ++i)
					{
						segmentGraphics[i].transform.position = positions[i];
					}
					timer -= animationDuration;
					state = State.waiting;
				}
				break;
		}
    }
}
