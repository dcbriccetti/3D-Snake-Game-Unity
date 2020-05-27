using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// ReSharper disable CompareOfFloatsByEqualityOperator

public class Controller : MonoBehaviour {
    public Transform target;

    [FormerlySerializedAs("cameraRotation")]
    public Vector2 cameraPitchAndYaw;

    public Vector3 mouseSensitivity = Vector3.one * 5;
    public float distanceFromTarget = 10;
    public SnakeAgent snake;

    private Transform cachedTransform;
    private Vector2 look = Vector2.zero;
    private float zoom;

    void Start() {
        cachedTransform =
            transform; // cached, because 'transform' is actually a function call with a cost that grows with component count.
    }

    public void OnLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();

    public void OnZoom(InputAction.CallbackContext context) => zoom = context.ReadValue<float>();

    private static Vector3 BestCardinalDirection(Vector3 lookDirection) {
        Vector3 dir = Arena.Directions[0];
        float bestAlignment = Vector3.Dot(lookDirection, dir);
        for (int i = 1; i < Arena.Directions.Length; ++i) {
            float alignment = Vector3.Dot(lookDirection, Arena.Directions[i]);
            if (alignment > bestAlignment) {
                bestAlignment = alignment;
                dir = Arena.Directions[i];
            }
        }

        return dir;
    }

    private void Update() {
        // happens every animation frame, not terribly time bound
        var k = Keyboard.current;
        var movementX = k.leftArrowKey.wasPressedThisFrame ? -1 : k.rightArrowKey.wasPressedThisFrame ? 1 : 0;
        var movementY = k.downArrowKey.wasPressedThisFrame ? -1 : k.upArrowKey.wasPressedThisFrame ? 1 : 0;
        var movementZ = k.tKey.wasPressedThisFrame ? -1 : k.gKey.wasPressedThisFrame ? 1 : 0;
        bool movingX = movementX != 0;
        bool movingY = movementY != 0;
        bool movingZ = movementZ != 0;

        if (movingX || movingY || movingZ) {
            snake.nextDirection =
                movingX ? Vector3.right * movementX :
                movingY ? Vector3.up * movementY :
                Vector3.back * movementZ;
        }

        cameraPitchAndYaw.x -= look.y * mouseSensitivity.y; // inverting y axis, 
        cameraPitchAndYaw.y += look.x * mouseSensitivity.x;
        distanceFromTarget += zoom * mouseSensitivity.z;
    }

    private void
        LateUpdate() // happens every frame JUST before the render call. will cause stuttering if this takes too long.
    {
        cachedTransform.rotation = Quaternion.identity * Quaternion.Euler(cameraPitchAndYaw);
        cachedTransform.position = target.position - cachedTransform.forward * distanceFromTarget;
    }
}