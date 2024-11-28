using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector3 windDirection;

    public Vector3 GetWindDirection()
    {
        return windDirection;
    }
}