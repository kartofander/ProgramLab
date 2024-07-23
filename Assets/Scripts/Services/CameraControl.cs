using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private float distanceToTarget = 10;
    private Camera cam;
    
    private Vector3 previousPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();

        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);
        
        AdjustCamera(Vector3.zero);
    }
    
    private void Update() 
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            var newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            var direction = previousPosition - newPosition;
            AdjustCamera(direction);
            previousPosition = newPosition;
        }
    }

    private void AdjustCamera(Vector3 direction)
    {
        var rotationAroundYAxis = -direction.x * 180;
        var rotationAroundXAxis = direction.y * 180;
            
        transform.position = target.position;
            
        transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
        transform.Translate(new Vector3(0, 0, -distanceToTarget));
    }
}
