using UnityEngine;

public class CameraFollowBehind : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;  // The target for the camera to follow
    public float distance = 5f;  // Distance behind the target
    public float height = 2f;    // Height above the target

    [Header("Smooth Damping Settings")]
    public float smoothTime = 0.3f; // Time to smoothly reach the target position

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set for CameraFollowBehind script.");
            return;
        }

        // Calculate the desired position behind the target
        Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Make the camera look at the target
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
