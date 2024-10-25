using UnityEngine;

public class MovableObject : ElementalObject
{
    public float trajectoryForceMultiplier = 10f;  // Force applied when the object is launched
    public LineRenderer trajectoryLineRenderer;    // Line renderer to visualize the trajectory
    public int trajectoryPointsCount = 30;         // Number of points to display in the trajectory

    private bool isHovered;                        // Whether the object is being hovered over
    private bool isTrajectoryVisible = false;      // Track if the trajectory is visible
    private Vector3 currentForceDirection;         // Direction the object will be launched in

    // Method to interact with the object when hit by a bullet
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Air)
        {
            // Launch the object with the direction opposite to where the player is aiming
            LaunchObject(hitPoint);
        }
    }

    // Launch the object by applying force based on the aiming direction
    private void LaunchObject(Vector3 hitPoint)
    {
        // Calculate the direction from the object to the hit point
        Vector3 directionToHitPoint = (hitPoint - transform.position).normalized;

        // Reverse the direction to aim on the opposite side
        currentForceDirection = -directionToHitPoint;

        Rigidbody rb = GetComponent<Rigidbody>();

        // Apply force to the object to launch it
        rb.AddForce(currentForceDirection * trajectoryForceMultiplier, ForceMode.Impulse);

        // Optionally clear the trajectory display after launching
        ClearTrajectory();
    }

    // Display the trajectory if hovering over the object and using Air element
    void Update()
    {
        if (isHovered)
        {
            if (!isTrajectoryVisible)
            {
                ShowTrajectory();
                isTrajectoryVisible = true;
            }

            // Optionally hide the trajectory if not hovering or using a different element
            if (!isHovered)
            {
                ClearTrajectory();
            }
        }
        else if (isTrajectoryVisible)
        {
            ClearTrajectory();
        }
    }

    // Display the calculated trajectory
    private void ShowTrajectory()
    {
        Vector3 startPosition = transform.position;
        Vector3 initialVelocity = currentForceDirection * trajectoryForceMultiplier;

        trajectoryLineRenderer.positionCount = trajectoryPointsCount;

        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            float time = i * 0.1f;  // Time intervals between points
            Vector3 point = CalculateTrajectoryPoint(startPosition, initialVelocity, time);
            trajectoryLineRenderer.SetPosition(i, point);
        }
    }

    // Calculate the point in the trajectory
    private Vector3 CalculateTrajectoryPoint(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        // Standard physics equation for projectile motion with gravity
        Vector3 position = startPosition + initialVelocity * time + 0.5f * Physics.gravity * time * time;
        return position;
    }

    // Clear the trajectory display
    private void ClearTrajectory()
    {
        trajectoryLineRenderer.positionCount = 0;
        isTrajectoryVisible = false;
    }

    // Trigger when the mouse is hovering over the object
    private void OnMouseEnter()
    {
        isHovered = true;
    }

    // Trigger when the mouse stops hovering over the object
    private void OnMouseExit()
    {
        isHovered = false;
        ClearTrajectory();
    }
}
