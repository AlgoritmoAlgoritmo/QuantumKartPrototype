using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineHelper : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        // Initialize the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    public void DrawRay(Vector3 start, Vector3 direction, float duration=1f)
    {
        // Set the color and positions of the line
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + direction);

        // Start a coroutine to disable the line after the specified duration
        StartCoroutine(DisableLineAfterDuration(duration));
    }

    private IEnumerator DisableLineAfterDuration(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);
        
        // Clear the line positions to hide the line
        lineRenderer.positionCount = 0;
    }
}
