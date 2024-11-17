using UnityEngine;

public class GunController : IInventoryItem {
    public Bullet bullet;
    public int damage;
    public float shootForce, upwardForce;
    public float fireRate, spread, range, reloadtime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    private bool shooting, readyToShoot, reloading;
    public Camera cam;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    private PlayerUi playerUI;
    public enum Element {
        Fire,
        Ice,
        Air,
        Default
    }
    public Element currElement = Element.Default;

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

        if (Input.GetButtonDown("Fire2")) {
            SwitchElement();
        }
    }

    private void Awake() {
        Initialize();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update() {
        playerUI = GetComponentInParent<PlayerUi>();
        PlayerInventory playerInventory = GetComponentInParent<PlayerInventory>();
        input();
        if(playerInventory.isActive(this)) {
            // Element= faceColor/OutlineColors for the ammoText
            // Fire = red/orange, Ice = cyan/white, Air = white/grey, Default = black/white
            Color black = new Color(0, 0, 0, 1);
            Color cyan = new Color(0, 1, 1, 1);
            Color grey = new Color(0.5f, 0.5f, 0.5f, 1);
            Color white = new Color(1, 1, 1, 1);
            Color red = new Color(1, 0, 0, 1);
            Color orange = new Color(1, 0.5f, 0, 1);
            Color faceColor =  red;
            Color outlineColor = orange;

            switch (currElement) {
                case Element.Fire:
                    faceColor = red;
                    outlineColor = orange;
                    break;
                case Element.Ice:
                    faceColor = cyan;
                    outlineColor = white;
                    break;
                case Element.Air:
                    faceColor = grey;
                    outlineColor = white;
                    break;
                case Element.Default:
                    faceColor = black;
                    outlineColor = white;
                    break;
            }
            playerUI.UpdateInfoText("[" + bulletsLeft + "/" + magazineSize + "]", faceColor, outlineColor);
        }
    }

    public override void Interact() {
        base.Interact();
    }

    // shoot method to shoot a raycast from the camera
    public override void Use() {
        base.Use();
        // is shooting not ready to shoot
        readyToShoot = false;
        // Find the exact hit position using raycast
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center of the screen
        RaycastHit hit;
        
        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, range)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(range);
        }

        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // instantiate bullet/projectile
        Bullet currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.GetComponent<Bullet>().SetDamage(damage);
        currentBullet.GetComponent<Bullet>().SetElement(currElement);
        currentBullet.GetComponent<Bullet>().AssignMaterial(currElement);

        // rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot--;

        // Invoke(function to invoke, delay)
        Invoke("ResetShot", fireRate);
        if (bulletsShot > 0 && bulletsLeft > 0) {
            Invoke("Use", timeBetweenShots);
        }
    }

    //Switches the current element. Currently doesn't have a GUI representation
    private void SwitchElement()
    {
   
        Element[] elements = (Element[])System.Enum.GetValues(typeof(Element));

        int currentIndex = System.Array.IndexOf(elements, currElement);
        currentIndex = (currentIndex + 1) % elements.Length;
        currElement = elements[currentIndex];

     
        print("Switched to Element: " + currElement);


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
}
