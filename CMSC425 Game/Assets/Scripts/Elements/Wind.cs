using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector3 windDirection;
    void Start()
    {
        
    }

    public Vector3 getWindDirection()
    {
        return windDirection;
    }
}
