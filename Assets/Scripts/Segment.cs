using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable ParameterHidesMember

public class Segment {
    private Vector3 position;
    private readonly Transform uSegment;

    public Segment(Vector3 position, Transform uSegment) {
        this.position = position;
        this.uSegment = uSegment;

        SetUnityPosition(position);
        uSegment.localScale = Vector3.one * 0.9f;
    }

    private void SetUnityPosition(Vector3 position) {
        uSegment.localPosition = position + Vector3.up * 0.5f;
    }

    public void Move(Vector3 offset) {
        position += offset;
        SetUnityPosition(position);
    }

    public void MoveTo(Segment segment) {
        position = segment.position;
        SetUnityPosition(position);
    }
}