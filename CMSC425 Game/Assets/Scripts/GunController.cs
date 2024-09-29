using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : IInventoryItem
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float fireRate, spread, range, reloadtime, timeBetweenShots;
    [SerializeField]
    private int magazineSize, bulletsPerTap;
    [SerializeField]
    private bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    private bool shooting, readyToShoot, reloading;
    [SerializeField]
    private Camera cam;
    private RaycastHit hit;
    // private LayerMask mask;  // add a mask if needed.
    private List<Color> hitColors = new List<Color> { // List of colors to change the object to when hit 
        Color.red, 
        Color.blue, 
        Color.green, 
        Color.yellow, 
        Color.magenta 
    };
    [SerializeField]
    private TextMeshProUGUI text;
    private PlayerInventory inventory;

    private void input() {
        if (allowButtonHold) {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) {
            Reload();
        } 

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = bulletsPerTap;
            Use();
        }
    }

    // Start is called before the first frame update
    private void Awake() {
        // Get the camera component from the PlayerLook script
        inventory = FindObjectOfType<PlayerInventory>();
        Initialize();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update() {
        input();
        text.SetText("[" + bulletsLeft + "/" + magazineSize + "]");
    }

    public override void Interact() {
        base.Interact();
        AddToInventory();
    }

    // shoot method to shoot a raycast from the camera
    public override void Use() {
        base.Use();
        // is shooting not ready to shoot
        readyToShoot = false;
        // spread 
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0); // set director for spread

        // Raycast(origin, direction, hitInfo, maxDistance, filter mask)
        if (Physics.Raycast(cam.transform.position, direction, out hit, range)) {
            Debug.Log("Hit object: " + hit.transform.name);

            // implement take damage in enemy class.
            // set tag to Enemy
            // if (hit.collider.CompareTag("Enemy")) {
                // hit.collider.GetComponent<Enemy_class>().TakeDamage(damage);
            // }

            // changes color of the hit object
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            if (renderer != null) {
                renderer.material.color = GetNextColor();
            }
        }
        else {
            Debug.Log("No object hit");
        }

        bulletsLeft--;
        bulletsShot--;

        // Invoke(function to invoke, delay)
        Invoke("ResetShot", fireRate);
        if (bulletsShot > 0 && bulletsLeft > 0) {
            Invoke("Use", timeBetweenShots);
        }
    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinished", reloadtime); // reloadtime defines how long it takes to reload.
    }

    private void ReloadFinished() {
        // set bulletsleft to the size of magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void ResetShot() {
        readyToShoot = true;
    }

    // Gets the next color from the list
    private Color GetNextColor() {
        Color nextColor = hitColors[Random.Range(0, 4)];
        return nextColor;
    }

    public override void AddToInventory() {
        inventory.AddItem(this);
    }

    public override void RemoveFromInventory() {
        inventory.RemoveItem(this);
    }
}
