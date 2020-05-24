using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable InconsistentNaming

public class Snake {
    private readonly List<Segment> segments;

    public Snake(Func<Transform> segFactory) {
        Vector3 V(int x, int y, int z) => new Vector3(x, y, z);

        var initialSegmentPositions = new [] {
            V(0, 2, 0),
            V(0, 1, 0),
            V(0, 0, 0),
        };
        segments = new List<Segment>( initialSegmentPositions.Select(pos => 
            new Segment(pos,segFactory())));
    }

    public void Move(Vector3 offset) {
        if (offset.Equals(Vector3.zero)) return;

        // Move all but the head to the segment ahead of it
        for (int i = segments.Count - 1; i > 0; --i) {
            segments[i].MoveTo(segments[i - 1]);
        }

        // Move the head using the offset from the user’s input
        segments[0].Move(offset);
    }
}