using UnityEngine;

public class Wick : ElementalObject
{
    public GameObject flamePrefab;
    public Burner TargetObject; // The first burner in the chain

    private Flame flameInstance;
    public void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);  // Set the first point
        lineRenderer.SetPosition(1, TargetObject.transform.position);
    }
    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Fire)
        {
            StartPuzzle();
        }
    }
    public void StartPuzzle()
    {
        if (flameInstance == null && flamePrefab != null && TargetObject != null)
        {
            GameObject flameObject = Instantiate(flamePrefab, transform.position, Quaternion.identity);
            flameInstance = flameObject.GetComponent<Flame>();
            flameInstance.SetTarget(TargetObject);
        }
    }

    private void OnDrawGizmos()
    {
        
        
        Gizmos.color = Color.red;  // Set line color (optional)
        Gizmos.DrawLine(transform.position, TargetObject.transform.position);
        
    }

}
