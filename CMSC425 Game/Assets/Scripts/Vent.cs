using UnityEngine;
public class Vent : ElementalObject
{
    private bool isActivated = false;
    [SerializeField] private Wind wind;
    private Vector3 originalPosition;

    public override void InteractWithElement(GunController.Element element, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (element == GunController.Element.Ice)
        {
            TurnOff();
        }
        else if (element == GunController.Element.Fire)
        {
            TurnOn();
        }
    }

    private void Start()
    {
        wind.gameObject.SetActive(false);
    }

    private void TurnOff()
    {
        if (wind != null)
        {
           
            wind.gameObject.SetActive(false);
        }
        isActivated = false;
    }

    private void TurnOn()
    {
        if (wind != null)
        {
            // Activate wind
            wind.gameObject.SetActive(true);
        }
        isActivated = true;
    }

    private void Update()
    {
        if (isActivated)
        {
            transform.Rotate(Vector3.left * 100f * Time.deltaTime);
        }
    }
}
