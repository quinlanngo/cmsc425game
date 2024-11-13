using UnityEngine;

public class Wick : ElementalObject
{
    public GameObject flamePrefab;
    public Burner TargetObject; // The first burner in the chain

    private Flame flameInstance;

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
}
