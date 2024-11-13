using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour
{
    public float speed = 2f;
    public float burnLifetime = 3f;
    private float currentLifetime;
    private MonoBehaviour currentTarget;

    private bool isBurning = false;

    // Set the next target, which can be a Burner or Bomb
    public void SetTarget(MonoBehaviour target)
    {
        currentTarget = target;
        isBurning = false;
        currentLifetime = burnLifetime;
    }

    // Start burning at the current burner
    public void StartBurning(Burner burner)
    {
        isBurning = true;
    }

    // Move the flame towards the current target
    private void Update()
    {
        if (isBurning || currentTarget == null) return;

        // Move towards the target
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, step);
    

        // Check if the flame has reached the target
        if (Vector3.Distance(transform.position, currentTarget.transform.position) < 0.1f)
        {
            if (currentTarget is Burner nextBurner)
            {
                nextBurner.OnFlameReached(this);
            }
            else if (currentTarget is Bomb bomb)
            {
                bomb.Explode();
                Destroy(gameObject);
            }
        }
    }
}
