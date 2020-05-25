using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static SnakeAgent.State;

public class SnakeAgent : MonoBehaviour {
    public Color color = Color.green;
    public Vector3 direction;
    public float delayBetweenAnimations = 0.75f;
    public float animationDuration = 0.25f;
    public GameObject segmentPrefab;
    public List<Vector3> segmentPositions;
    public List<GameObject> segmentGraphics = new List<GameObject>();
    public enum State { Waiting, Animating }
    public State state;
    public int initialNumSegments = 3;
    public float maxSegmentScale = 1f;
    public float minSegmentScale = 0.5f;

    private float timer;

    void Start() {
        for (int i = 0; i < initialNumSegments; ++i) {
            segmentPositions.Add(Vector3.zero);
            var seg = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity);
            seg.GetComponent<Renderer>().material.color = color;
            seg.transform.localScale =
                Vector3.one * Mathf.Lerp(maxSegmentScale, minSegmentScale, (float) i / initialNumSegments);
            seg.transform.position += Vector3.left * i;
            segmentGraphics.Add(seg);
        }
    }

    void Update() {
        timer += Time.deltaTime;
        switch (state) {
            case Waiting:
                if (timer >= delayBetweenAnimations) {
                    timer -= delayBetweenAnimations;
                    state = Animating;
                }

                break;
            case Animating:
                if (timer < animationDuration) {
                    float progress = timer / animationDuration;
                    Vector3 segmentGoingTo = segmentPositions[0] + direction;
                    for (int i = 0; i < segmentGraphics.Count; ++i) {
                        var segmentAt = segmentPositions[i];
                        segmentGraphics[i].transform.position = Vector3.Lerp(segmentAt, segmentGoingTo, progress);
                        segmentGoingTo = segmentAt;
                    }
                } else {
                    segmentPositions.Insert(0, segmentPositions[0] + direction);
                    segmentPositions.RemoveAt(segmentPositions.Count - 1);
                    for (int i = 0; i < segmentGraphics.Count; ++i) {
                        segmentGraphics[i].transform.position = segmentPositions[i];
                    }

                    timer -= animationDuration;
                    state = Waiting;
                }

                break;
        }
    }
}