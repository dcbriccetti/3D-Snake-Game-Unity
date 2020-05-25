using UnityEngine;
using static UnityEngine.Vector3;

public class Arena : MonoBehaviour {
    //                                             [X,     Y,  Z,       -X,   -Y,   -Z]
    public static readonly Vector3[] Directions = { right, up, forward, left, down, back };
    private static readonly string[] FaceNames = "right ceiling front left floor back".Split(' ');
    public Vector3 size = one * 10;

    void CreateFaces() {
        for (int i = 0; i < FaceNames.Length; ++i) {
            int axisIndex = i % 3;
            bool isZAligned = i != 2;
            var faceQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            faceQuad.name = FaceNames[i];
            var faceTransform = faceQuad.transform;
            faceTransform.position = Directions[i] * size[axisIndex];
            faceTransform.rotation = Quaternion.LookRotation(Directions[i], isZAligned ? up : forward);
            faceTransform.localScale = Scale(axisIndex);
        }
    }

    private Vector3 Scale(int dir) {
        float[] A(float a, float b) => new [] {a, b};
        var c = new [] {
            A(size.z, size.y),
            A(size.x, size.z),
            A(size.x, size.y)
        }[dir];

        return new Vector3(c[0], c[1], 1) * 2;
    }

    void Start() {
        CreateFaces();
    }
}