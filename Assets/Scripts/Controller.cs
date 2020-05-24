using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CompareOfFloatsByEqualityOperator

public class Controller : MonoBehaviour {
    public GameObject controlTile;
    public Transform target;
    Transform t;

    /// pitch, yaw (roll omitted)
    public Vector2 cameraRotation;

    public Vector3 mouseSensitivity = Vector3.one * 5;
    public float distanceFromTarget = 10;

    private bool controlTileDirectionNeedsCalculation = true;

    public SnakeAgent snake;
    private Vector2 movement;

    void Start() {
        t = transform; // cached, because 'transform' is actually a function call with a cost that grows with component count.
        if (!snake) {
            Debug.LogWarning(name + " controller has no snake to control!");
        }
    }

    public void OnMove(InputAction.CallbackContext context) => movement = context.ReadValue<Vector2>();

    Vector3 BestCardinalDirection(Vector3 lookDirection) {
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

    void FixedUpdate() {
        // happens at very regular intervals, usually slower than Update
        if (controlTileDirectionNeedsCalculation) {
            Vector3 f = BestCardinalDirection(t.forward);
            Vector3 u = BestCardinalDirection(t.up);
            Quaternion moveRotation = Quaternion.LookRotation(f, u);
            controlTile.transform.rotation = moveRotation;
            controlTileDirectionNeedsCalculation = false;
        }

        if (!snake) return;

        bool movingH = movement.x != 0;
        bool movingV = movement.y != 0;

        if (movingH != movingV) {
            int Sign(float value) => value > 0 ? 1 : -1;
            snake.direction = controlTile.transform.rotation *
                              (movingH ? Vector3.right * Sign(movement.x) : Vector3.up * Sign(movement.y));
            controlTile.transform.position = snake.positions[0] + snake.direction;
        }
    }

    void Update() {
        // Needs converting to new input scheme
        
        //happens every animation frame, not terribly time bound
        /*
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical   = Input.GetAxis("Mouse Y");
        if (mouseHorizontal != 0 || mouseVertical != 0) {
            controlTileDirectionNeedsCalculation = true;
        }
        
        cameraRotation.x -= mouseVertical   * mouseSensitivity.y; // inverting y axis, 
        cameraRotation.y += mouseHorizontal * mouseSensitivity.x;
        distanceFromTarget += Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity.z;
        */
    }

    private void
        LateUpdate() // happens every frame JUST before the render call. will cause stuttering if this takes too long.
    {
        t.rotation = Quaternion.identity * Quaternion.Euler(cameraRotation);
        t.position = target.position - t.forward * distanceFromTarget;
    }
}