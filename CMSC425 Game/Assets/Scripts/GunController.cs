using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GunController : IInventoryItem
{
    public Bullet bullet;
    public int damage;
    public float shootForce, upwardForce;
    public float fireRate, spread, range, reloadtime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsShot;

    public float shotVolume;

    // Static flags for element availability, just eable after each level complition
    public bool fireEnabled = true;
    public bool iceEnabled = true;
    public bool airEnabled = false;
    // Note: No flag for default as it's always enabled

    // Dictionary to store bullets for each element
    private Dictionary<Element, int> elementBullets;
    private float reloadStartTime;
    private bool isReloadAnimating;
    private int bulletsAtReloadStart;

    private bool shooting, readyToShoot, reloading;
    public Camera cam;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    private PlayerUi playerUI;
    public Slider energyBar;
    public enum Element {
        Fire,
        Ice,
        Air,
        Default
    }
    public Element currElement = Element.Default;
//SFX 
    [SerializeField] private AudioClip defaultShot;
    [SerializeField] private AudioClip fireShot;
    [SerializeField] private AudioClip iceShot;
    [SerializeField] private AudioClip airShot;
    [SerializeField] private AudioClip recharge;

    [SerializeField] private AudioClip defaultSwitch;
    [SerializeField] private AudioClip fireSwitch;
    [SerializeField] private AudioClip iceSwitch;
    [SerializeField] private AudioClip airSwitch;



    // Helper method to check if an element is enabled
    private bool IsElementEnabled(Element element) {
        switch (element) {
            case Element.Fire:
                return fireEnabled;
            case Element.Ice:
                return iceEnabled;
            case Element.Air:
                return airEnabled;
            case Element.Default:
                return true; // Default is always enabled
            default:
                return false;
        }
    }

    private void input() {
        if (allowButtonHold) {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Only reload current element if its bullets are less than magazine size
        if (Input.GetKeyDown(KeyCode.R) && elementBullets[currElement] < magazineSize && !reloading) {
            SFXManager.instance.PlaySFXClip(recharge, this.transform, 1f);
            Reload();
        }

        if (readyToShoot && shooting && !reloading && elementBullets[currElement] > 0) {
            bulletsShot = bulletsPerTap;
            Use();
        }

        if (Input.GetButtonDown("Fire2")) {
            SwitchElement();
        }
    }

    private void Awake() {
        Initialize();
        // Initialize dictionary with full magazine for each element
        elementBullets = new Dictionary<Element, int>();
        foreach (Element element in System.Enum.GetValues(typeof(Element))) {
            elementBullets[element] = magazineSize;
        }
        readyToShoot = true;
        isReloadAnimating = false;
    }

    private void Update() {
        playerUI = GetComponentInParent<PlayerUi>();
        playerUI.updateInfoText(String.Empty, Color.white, Color.white);
        PlayerInventory playerInventory = GetComponentInParent<PlayerInventory>();
        input();
        if (playerInventory.isActive(this)) {
            UpdateEnergyBar();
        }
    }

    private void UpdateEnergyBar() {
        // Element colors
        Color black = new Color(0, 0, 0, 1);
        Color cyan = new Color(0, 1, 1, 1);
        Color grey = new Color(0.5f, 0.5f, 0.5f, 1);
        Color white = new Color(1, 1, 1, 1);
        Color red = new Color(1, 0, 0, 1);
        Color orange = new Color(1, 0.5f, 0, 1);
        Color sliderColor = red;

        switch (currElement) {
            case Element.Fire:
                sliderColor = orange;
                break;
            case Element.Ice:
                sliderColor = cyan;
                break;
            case Element.Air:
                sliderColor = grey;
                break;
            case Element.Default:
                sliderColor = black;
                break;
        }
        energyBar.fillRect.GetComponent<Image>().color = sliderColor;
        if (isReloadAnimating) {
            float reloadProgress = (Time.time - reloadStartTime) / reloadtime;
            if (reloadProgress >= 1f) {
                // Reload animation complete
                isReloadAnimating = false;
                energyBar.value = 1f;
            }
            else {
                // Reload animation in progress
                float startValue = (float)bulletsAtReloadStart / magazineSize;
                energyBar.value = Mathf.Lerp(startValue, 1f, reloadProgress);
                
            }
        }
        else {
            // When not reloading, show current element's bullets
            energyBar.value = (float)elementBullets[currElement] / magazineSize;
        }
    }

    public override void Interact() {
        base.Interact();
    }

    // Creates a physical bullets and assigns properties.
    public override void Use() {
        base.Use();
        // is shooting not ready to shoot
        readyToShoot = false;
        // Find the exact hit position using raycast
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center of the screen
        RaycastHit hit;

        // check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, range)) {
            targetPoint = hit.point;
        }
        else {
            targetPoint = ray.GetPoint(range);
        }

        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // calculate spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        // calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // instantiate bullet
        Bullet currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.GetComponent<Bullet>().SetDamage(damage); // Set bullet damage
        currentBullet.GetComponent<Bullet>().SetElement(currElement); // Set bullet element
        currentBullet.GetComponent<Bullet>().AssignMaterial(currElement);  // Assign material based on element

        // rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        
        //play sound depending on the current element
        if (currElement == Element.Default)
        {
            SFXManager.instance.PlaySFXClip(defaultShot, this.transform, 0.5f);
        }
        else if (currElement == Element.Fire)
        {
            SFXManager.instance.PlaySFXClip(fireShot, this.transform, 0.5f);
        }
        else if (currElement == Element.Ice)
        {
            SFXManager.instance.PlaySFXClip(iceShot, this.transform, 0.5f);
        }
        else if (currElement == Element.Air)
        {
            SFXManager.instance.PlaySFXClip(airShot, this.transform, 0.5f);
        }

        // Decrease bullets for current element only
        elementBullets[currElement]--;
        bulletsShot--;

        // Invoke(function to invoke, delay)
        Invoke("ResetShot", fireRate);
        if (bulletsShot > 0 && elementBullets[currElement] > 0) {
            Invoke("Use", timeBetweenShots);
        }
    }

    // Switches the current element, skipping disabled elements
    private void SwitchElement() {
        Element[] elements = (Element[])System.Enum.GetValues(typeof(Element));
        int currentIndex = System.Array.IndexOf(elements, currElement);

        // Keep trying next elements until an enabled one is found
        do {
            currentIndex = (currentIndex + 1) % elements.Length;
        } while (!IsElementEnabled(elements[currentIndex]) && elements[currentIndex] != currElement);

        // Only switch if we found a different enabled element
        if (IsElementEnabled(elements[currentIndex])) {
            currElement = elements[currentIndex];
            print("Switched to Element: " + currElement);
        }
        //play sound depending on the current element
        if (currElement == Element.Default)
        {
            SFXManager.instance.PlaySFXClip(defaultSwitch, this.transform, 0.5f);
        }
        else if (currElement == Element.Fire)
        {
            SFXManager.instance.PlaySFXClip(fireSwitch, this.transform, 0.5f);
        }
        else if (currElement == Element.Ice)
        {
            SFXManager.instance.PlaySFXClip(iceSwitch, this.transform, 0.5f);
        }
        else if (currElement == Element.Air)
        {
            SFXManager.instance.PlaySFXClip(airSwitch, this.transform, 0.5f);
        }
    }

    private void Reload() {
        reloading = true;
        isReloadAnimating = true;
        reloadStartTime = Time.time;
        // Store current element's bullets for the reload animation
        bulletsAtReloadStart = elementBullets[currElement];

        Invoke("ReloadFinished", reloadtime);
    }

    private void ReloadFinished() {
        // Only reload the current element's bullets
        elementBullets[currElement] = magazineSize;
        reloading = false;
    }

    private void ResetShot() {
        readyToShoot = true;
    }
}