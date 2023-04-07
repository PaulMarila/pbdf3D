using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class MoveChest : MonoBehaviour
{
    public float minX = -3.3943f;
    public float maxX = 4.61613f;
    public float minZ = -2.96116f;
    public float maxZ = 5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        // Enable mouse input actions
        InputSystem.EnableDevice(Mouse.current);
    }

    void OnDisable()
    {
        // Disable mouse input actions
        InputSystem.DisableDevice(Mouse.current);
    }

    void Update()
    {
        // Get mouse position in screen space
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Create a ray from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        // Cast the ray and get the point where it intersects with the plane at y = 0
        Plane plane = new Plane(new Vector3(0, -0.613f, 0), Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            // Set the object's position to the intersection point
            float xPos = Mathf.Clamp(ray.GetPoint(distance).x, minX, maxX);
            float zPos = Mathf.Clamp(ray.GetPoint(distance).z, minZ, maxZ);
            transform.position = new Vector3(xPos, transform.localPosition.y, zPos);
        }
    }
}
