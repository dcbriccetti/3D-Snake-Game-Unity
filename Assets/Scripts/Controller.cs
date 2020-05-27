using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour {
    public Transform target;
    [FormerlySerializedAs("foodItem")] public Transform foodItemPrefab;
    public Vector2 cameraPitchAndYaw;

    public Vector3 mouseSensitivity = Vector3.one * 5;
    public float distanceFromTarget = 10;
    public SnakeAgent snake;

    private Transform cachedTransform;
    private Vector2 look = Vector2.zero;
    private float zoom;
    private Transform foodItem;

    void Start() {
        cachedTransform = transform; // cached, because 'transform' is actually a function call with a cost that grows with component count.
        foodItem = Instantiate(foodItemPrefab);
        var arenaScript = target.GetComponent<Arena>();
        var asz = arenaScript.size;
        foodItem.position = new Vector3(Random.Range(0, asz.x), Random.Range(0, asz.y), Random.Range(0, asz.z));
    }

    public void OnLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();

    public void OnZoom(InputAction.CallbackContext context) => zoom = context.ReadValue<float>();

    private void Update() {
        SetSnakeNextDirectionFromInput();

        cameraPitchAndYaw.x -= look.y * mouseSensitivity.y; // inverting y axis, 
        cameraPitchAndYaw.y += look.x * mouseSensitivity.x;
        distanceFromTarget += zoom * mouseSensitivity.z;
    }

    private void SetSnakeNextDirectionFromInput() {
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
    }

    private void
        LateUpdate() // happens every frame JUST before the render call. will cause stuttering if this takes too long.
    {
        cachedTransform.rotation = Quaternion.identity * Quaternion.Euler(cameraPitchAndYaw);
        cachedTransform.position = target.position - cachedTransform.forward * distanceFromTarget;
    }
}