using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 12f;
    [SerializeField] 
    private float frequency = 2f;
    [SerializeField]
    private float waitBeforeStart = 1f;
    private Vector3 initialScale;
    private float elapsedTime;

    public Vector3 launchDirection = Vector3.forward;
    public float launchForce = 10f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (waitBeforeStart > 0) {
            waitBeforeStart -= 1;
        } else {

            elapsedTime += Time.deltaTime;
            float cycle = elapsedTime * frequency % 2f;
            float scale;
            
            if (cycle < 1f)
            {
                // Forward movement (0 to amplitude)
                scale = cycle * amplitude;
            }
            else
            {
                // Return movement (amplitude back to 0)
                float returning = cycle - 1f;
                scale = amplitude * (1f - returning);
            }
            
            transform.localScale = new Vector3(initialScale.x + scale, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidBody.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Set gizmo color
        Gizmos.color = Color.red;

        {
            // Draw an arrow representing the launch direction
            
                Vector3 start = transform.position;
                Vector3 end = start + launchDirection.normalized * 2.0f; // Scale the arrow for visibility
                Gizmos.DrawLine(start, end);

                // Draw the arrowhead
                Vector3 arrowHeadSize = Vector3.up * 0.2f;
                Gizmos.DrawLine(end, end - (launchDirection.normalized + arrowHeadSize) * 0.2f);
                Gizmos.DrawLine(end, end - (launchDirection.normalized - arrowHeadSize) * 0.2f);
            
        }
    }
}
