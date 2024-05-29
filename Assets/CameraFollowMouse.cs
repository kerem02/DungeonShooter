using UnityEngine;
using Cinemachine;

public class CameraFollowMouse : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public float followSpeed = 2f; // Speed at which the camera follows the mouse
    public float offsetDistance = 1f; // Distance offset for the camera follow

    private Transform playerTransform; // Reference to the player's transform
    private Vector3 initialCameraPositionOffset; // Initial offset between the camera and the player

    private void Start()
    {
        if (virtualCamera != null)
        {
            playerTransform = virtualCamera.Follow; // Get the player's transform
            initialCameraPositionOffset = virtualCamera.transform.position - playerTransform.position; // Calculate the initial offset
        }
    }

    private void Update()
    {
        if (virtualCamera != null && playerTransform != null)
        {
            Vector3 playerPosition = playerTransform.position; // Get the player's position
            Vector3 targetCameraPosition = playerPosition + initialCameraPositionOffset; // Calculate the target camera position

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
            mousePosition.z = 0; // Set the z position to 0

            Vector3 direction = (mousePosition - targetCameraPosition).normalized; // Calculate the direction from the camera to the mouse

            Vector3 mouseOffsetPosition = targetCameraPosition + direction * offsetDistance; // Calculate the new camera position with offset

            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, mouseOffsetPosition, followSpeed * Time.deltaTime); // Smoothly move the camera to the new position
        }
    }
}

