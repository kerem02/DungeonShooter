using UnityEngine;
using Cinemachine;

public class CameraFollowMouse : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float followSpeed = 2f;
    public float offsetDistance = 1f;

    private Transform playerTransform;
    private Vector3 initialCameraPositionOffset;

    private void Start()
    {
        if (virtualCamera != null)
        {
            playerTransform = virtualCamera.Follow;
            initialCameraPositionOffset = virtualCamera.transform.position - playerTransform.position;
        }
    }

    private void Update()
    {
        if (virtualCamera != null && playerTransform != null)
        {
            
            Vector3 playerPosition = playerTransform.position;
            Vector3 targetCameraPosition = playerPosition + initialCameraPositionOffset;

 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

  
            Vector3 direction = (mousePosition - targetCameraPosition).normalized;


            Vector3 mouseOffsetPosition = targetCameraPosition + direction * offsetDistance;


            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, mouseOffsetPosition, followSpeed * Time.deltaTime);
        }
    }
}
