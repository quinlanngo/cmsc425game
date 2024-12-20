using UnityEngine;

public class Burner : ElementalObject
{
    public bool IsBurning { get; private set; } = false;
    public bool IsShot { get; private set; } = false;
    public MonoBehaviour TargetObject; // Next object, either another Burner or a Bomb


    public void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);  // Set the first point
        lineRenderer.SetPosition(1, TargetObject.transform.position);
    }
    // Called when the flame reaches this burner
    public void OnFlameReached(Flame flame)
    {
        IsBurning = true;
        flame.StartBurning(this);
    }



    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            Ignite();
        }
    }
    // Called by fire bullet to reignite burner if it's burning
    public void Ignite()
    {
        if (IsBurning && !IsShot)
        {
            IsShot = true;
            MoveFlameToNext();
        }
    }

    // Move the flame to the next target
    private void MoveFlameToNext()
    {
        if (TargetObject != null)
        {
            if (TargetObject is Burner nextBurner)
            {
                FindObjectOfType<Flame>()?.SetTarget(nextBurner);
            }
            else if (TargetObject is Bomb bomb)
            {
                FindObjectOfType<Flame>()?.SetTarget(bomb);
            }
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red; 
        Gizmos.DrawLine(transform.position, TargetObject.transform.position);

    }
}
