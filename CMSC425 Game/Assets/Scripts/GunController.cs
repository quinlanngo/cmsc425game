using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float range = 100f;  // Range of the gun
    [SerializeField]
    public List<Color> hitColors = new List<Color> { // List of colors to change the object to when hit 
        Color.red, 
        Color.blue, 
        Color.green, 
        Color.yellow, 
        Color.magenta 
    };

    private int currentColorIndex = 0;

    // Start is called before the first frame update
    void Start() {
        // Get the camera component from the PlayerLook script
        cam = GetComponent<PlayerLook>().cam;
    }

    // shoot method to shoot a raycast from the camera
    public void Shoot() {
        RaycastHit hit;
        // Raycast(origin, direction, hitInfo, maxDistance)
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) {
            Debug.Log("Hit object: " + hit.transform.name);

            // changes color of the hit object
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            if (renderer != null) {
                renderer.material.color = GetNextColor();
            }
        }
        else {
            Debug.Log("No object hit");
        }
    }

    // Gets the next color from the list
    private Color GetNextColor() {
        Color nextColor = hitColors[currentColorIndex];
        currentColorIndex = (currentColorIndex + 1) % hitColors.Count;
        return nextColor;
    }
}
