using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 12f;
    [SerializeField] 
    private float frequency = 2f;
    [SerializeField]
    private float waitBeforeStart = 1f;
    private Vector3 initialScale;
    private float elapsedTime;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (waitBeforeStart > 0) {
            waitBeforeStart -= 1;
        } else {

            elapsedTime += Time.deltaTime;
            float cycle = elapsedTime * frequency % 2f;
            float scale;
            
            if (cycle < 1f)
            {
                // Forward movement (0 to amplitude)
                scale = cycle * amplitude;
            }
            else
            {
                // Return movement (amplitude back to 0)
                float returning = cycle - 1f;
                scale = amplitude * (1f - returning);
            }
            
            transform.localScale = new Vector3(initialScale.x + scale, transform.localScale.y, transform.localScale.z);
        }
    }
}
