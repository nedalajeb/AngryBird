using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the cube's movement

    void Update()
    {
        // Get input from arrow keys or WASD
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        // Move the cube
        transform.Translate(moveDirection);
    }
}