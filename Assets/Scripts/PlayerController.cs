using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f;      // Forward movement speed
    public float lateralSpeed = 10f;     // Left/right movement speed

    // Editable boundaries for horizontal movement
    public float minX = -5f;             // Left boundary
    public float maxX = 5f;              // Right boundary

    void Update()
    {
        // Store current position in a temporary variable
        Vector3 pos = transform.position;

        // Move forward continuously
        pos += Vector3.forward * forwardSpeed * Time.deltaTime;

        // Calculate lateral movement based on horizontal input (A/D or Left/Right)
        float moveX = Input.GetAxis("Horizontal") * lateralSpeed * Time.deltaTime;
        pos.x += moveX;

        // Clamp the x-position so it doesn't go beyond the boundaries
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        // Apply the new position to the player
        transform.position = pos;
    }
}
