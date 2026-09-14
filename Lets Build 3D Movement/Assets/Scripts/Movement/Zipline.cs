using UnityEngine;

public class Zipline : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float zipSpeed = 15f;
    
    // LineRenderer (Optional) - Inspector mein wire dikhane ke liye
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
