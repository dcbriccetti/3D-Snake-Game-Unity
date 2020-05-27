using System.Collections.Generic;
using UnityEngine;

public class SnakeAgent : MonoBehaviour {
    public Vector3 nextDirection;
    public Color color = Color.green;
    public float delayBetweenAnimations = 0.75f;
    public float animationDuration = 0.25f;
    public GameObject segmentPrefab;
    public List<Vector3> segmentPositions;
    public List<GameObject> segmentGraphics = new List<GameObject>();
    public int initialNumSegments = 3;
    public float maxSegmentScale = 1f;
    public float minSegmentScale = 0.5f;

    private float timer;
    private float timeInMovementCycle;
    private Vector3 direction;

    void Start() {
        for (int i = 0; i < initialNumSegments; ++i) {
            segmentPositions.Add(Vector3.left * i);
            var seg = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity);
            seg.GetComponent<Renderer>().material.color = color;
            seg.transform.localScale =
                Vector3.one * Mathf.Lerp(maxSegmentScale, minSegmentScale, (float) i / initialNumSegments);
            seg.transform.position += Vector3.left * i;
            segmentGraphics.Add(seg);
        }
    }

    void Update() {
        if (direction == Vector3.zero) direction = nextDirection;
        if (direction == Vector3.zero) return;

        timeInMovementCycle += Time.deltaTime;
        if (timeInMovementCycle < animationDuration) {
            MoveSegments(timeInMovementCycle / animationDuration);
        } else if (timeInMovementCycle > animationDuration + delayBetweenAnimations) {
            AdvanceSegmentPositions();
            timeInMovementCycle = 0;
            direction = nextDirection;
        }
    }

    private void MoveSegments(float progress) {
        for (int i = 0; i < segmentGraphics.Count; ++i) {
            var goingToPos = i == 0 ? segmentPositions[0] + direction : segmentPositions[i - 1];
            segmentGraphics[i].transform.position =
                Vector3.Lerp(segmentPositions[i], goingToPos, progress);
        }
    }
    
    private void AdvanceSegmentPositions() {
        segmentPositions.Insert(0, segmentPositions[0] + direction);
        segmentPositions.RemoveAt(segmentPositions.Count - 1);
        for (int i = 0; i < segmentGraphics.Count; ++i) {
            segmentGraphics[i].transform.position = segmentPositions[i];
        }
    }
}