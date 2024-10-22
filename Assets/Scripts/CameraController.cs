using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1; 
    public Transform player2; 
    public float smoothSpeed = 0.125f; 
    public Vector3 offset; 
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomOutFactor = 2f; 

    private Camera cam; 

    private void Start()
    {
        cam = GetComponent<Camera>(); 
    }

    private void LateUpdate()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2;

        Vector3 targetPosition = midpoint + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        float distance = Vector3.Distance(player1.position, player2.position);

        float targetSize = Mathf.Clamp(distance / zoomOutFactor, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed);
    }
}
