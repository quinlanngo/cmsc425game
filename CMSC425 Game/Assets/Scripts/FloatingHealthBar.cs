using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Transform target;
    public Camera camera;
    public Vector3 offset;
    public Slider slider;


    public void updateHealth(float health, float maximumHealth)
    {
        slider.value = health / maximumHealth;
    }
    void Update()
    {
        transform.position = target.position + offset;
    }

}