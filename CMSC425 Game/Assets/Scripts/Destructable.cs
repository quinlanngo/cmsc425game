using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
