using UnityEngine;
// ReSharper disable InconsistentNaming

public partial class Game : MonoBehaviour {
    public Transform arena = default;
    public Transform prefabSegment = default;
    private Snake snake;
    private float secondsPerMove = 1f;
    private float nextMoveTime;
    private Vector3 direction = Vector3.zero;

    void Start() {
        snake = new Snake(() => Instantiate(prefabSegment, arena));
        nextMoveTime = Time.time + secondsPerMove;
    }

    void Update() {
        if (Time.time > nextMoveTime) {
            snake.Move(direction);
            nextMoveTime = Time.time + secondsPerMove;
        }

        if (Input.GetKey(KeyCode.RightArrow)) direction = Vector3.left; // Backwards for the moment
        else if (Input.GetKey(KeyCode.LeftArrow)) direction = Vector3.right;
        else if (Input.GetKey(KeyCode.UpArrow)) direction = Vector3.up;
        else if (Input.GetKey(KeyCode.DownArrow)) direction = Vector3.down;
    }
}
