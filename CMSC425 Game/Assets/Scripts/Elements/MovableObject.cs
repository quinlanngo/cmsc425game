using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovableObject : ElementalObject
{
    public float launchForce = 20f;    // Force applied to the object
    public float launchAngle = 40f;    // Angle in degrees to launch away
    public int lineResolution = 20;    // Resolution of the line (number of points)
    public float timeStep = 0.1f;      // Time interval for each point in the line

    private LineRenderer lineRenderer;
    private bool isHovering = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineResolution;
        lineRenderer.enabled = false; // Hide the line initially
    }
    /*
    void OnMouseEnter()
    {
        isHovering = true;
        lineRenderer.enabled = true;
        ShowProjectilePath();
    }

    void OnMouseExit()
    {
        isHovering = false;
        lineRenderer.enabled = false;
    }*/

    // Method to launch the object at an angle relative to the hit normal
    public void LaunchObjectAway(Vector3 hitPoint, Vector3 hitNormal)
    {
        Vector3 launchDirection = new Vector3(-hitNormal.x, Mathf.Sin(Mathf.Deg2Rad * launchAngle), -hitNormal.z).normalized;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }

    // Method to show the projectile path
    private void ShowProjectilePath()
    {
        Vector3 startPos = transform.position;
        Vector3 launchDirection = Quaternion.Euler(launchAngle, 0, 0) * Vector3.forward;
        launchDirection *= launchForce;

        Vector3 velocity = launchDirection;
        Vector3 position = startPos;

        lineRenderer.positionCount = lineResolution;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < lineResolution; i++)
        {
            points.Add(position);
            position += velocity * timeStep;
            velocity += Physics.gravity * timeStep;
        }

        lineRenderer.SetPositions(points.ToArray());
    }

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Air)
        {
            LaunchObjectAway(hitPoint, hitNormal);
        }
    }
}
