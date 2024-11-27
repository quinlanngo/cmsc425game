using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public float moveSpeed = 2f;
    public bool isInWind = false;
    public Vector3 movementDirection = Vector3.zero;

    // Update is called once per frame
    private void Update()
    {
        if (isInWind)
        {
            this.transform.position += moveSpeed * movementDirection * Time.deltaTime;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wind"))
        {
            isInWind = true;
            Wind windObject = other.gameObject.GetComponent<Wind>();
            if (windObject != null)
            {
                movementDirection = windObject.getWindDirection();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInWind = false;
    }
   
}
