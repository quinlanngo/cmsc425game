using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Rigidbody rb;
    public GameObject impactEffect;
    public LayerMask whatIsEnemies;
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public float explosionRadius;

    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    private int collisions;
    private ContactPoint contact;
    private PhysicMaterial mat;
    private int damage;
    private GunController.Element element;

    private void Start(){
        Initialize();
    }

    private void Update(){
        if(collisions > maxCollisions) {
            Explode();
        }
        maxLifeTime -= Time.deltaTime;
        if(maxLifeTime <= 0){
            Explode();
        }
    }

    private void Initialize(){
        mat = new PhysicMaterial();
        mat.bounciness = bounciness;
        mat.frictionCombine = PhysicMaterialCombine.Minimum;
        mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = mat;

        rb.useGravity = useGravity;

    }

    public int GetDamage(){
        return damage;
    }

    public int SetDamage(int damage){
        this.damage = damage;
        return damage;
    }

    public GunController.Element GetElement(){
        return element;
    }

    public GunController.Element SetElement(GunController.Element element){
        this.element = element;
        return element;
    }

    public void AssignMaterial(GunController.Element currElement){
        // Reference to all elemental materials
        Material fireMaterial = Resources.Load<Material>("FireMaterial");
        Material iceMaterial = Resources.Load<Material>("IceMaterial");
        Material airMaterial = Resources.Load<Material>("AirMaterial");
        // Default material in unity
        Material defaultMaterial = new Material(Shader.Find("Standard"));
        defaultMaterial.color = Color.black;

        // Assign the correct material based on the current element
        switch (currElement)
        {
            case GunController.Element.Fire:
                this.GetComponent<Renderer>().material = fireMaterial;
                this.GetComponent<TrailRenderer>().startColor = fireMaterial.color;
                this.GetComponent<TrailRenderer>().endColor = Color.white;
                break;
            case GunController.Element.Ice:
                this.GetComponent<Renderer>().material = iceMaterial;
                this.GetComponent<TrailRenderer>().startColor = iceMaterial.color;
                this.GetComponent<TrailRenderer>().endColor = Color.white;
                break;
            case GunController.Element.Air:
                this.GetComponent<Renderer>().material = airMaterial;
                this.GetComponent<TrailRenderer>().startColor = airMaterial.color;
                this.GetComponent<TrailRenderer>().endColor = Color.white;
                break;
            case GunController.Element.Default:
                this.GetComponent<Renderer>().material = defaultMaterial;
                this.GetComponent<TrailRenderer>().startColor = defaultMaterial.color;
                this.GetComponent<TrailRenderer>().endColor = Color.white;
                break;
            default:
                Debug.LogWarning("Unknown element type: " + currElement);
                break;
        }
    }

    private void Explode(){
        if(impactEffect != null){
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, whatIsEnemies);
        for(int i = 0; i < enemies.Length; i++){
            GruntAI gruntAI = enemies[i].GetComponentInParent<GruntAI>();
            if (gruntAI != null) {
                gruntAI.TakeDamage(damage);
            }
            PlayerHealth playerHealth = enemies[i].GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                Debug.Log("Player Hit by Enemy Bullet");
                playerHealth.TakeDamage(damage);
            }
        }

        Invoke("Delay", 0.05f);
    }

    private void Delay() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        collisions++;

        ElementalObject elementalObject = collision.gameObject.GetComponent<ElementalObject>();
        if (elementalObject != null)
        {
            // Get the contact point and normal from the collision
            contact = collision.GetContact(0);
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            elementalObject.InteractWithElement(element, contact.point, contact.normal);

            if (element == GunController.Element.Fire)
            {
                if (collision.gameObject.CompareTag("IceSheet") || collision.gameObject.CompareTag("Burnable"))
                {

                    Invoke("Delay", 0.05f);
                }
                else if (collision.gameObject.CompareTag("Bomb"))
                {
                    // don't impart a lot of force any force
                    Bomb bomb = collision.gameObject.GetComponent<Bomb>();
                    bomb.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Invoke("Delay", 0.05f);
                }
            }
            if (element == GunController.Element.Ice)
            {
                if (collision.gameObject.CompareTag("Bomb"))
                {
                    // don't impart a lot of force any force
                    Bomb bomb = collision.gameObject.GetComponent<Bomb>();
                    bomb.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Invoke("Delay", 0.05f);
                }
            }
        }
        if (collision.gameObject.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }

        if (collision.gameObject.CompareTag("Puzzle"))
        {
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            TargetBehavior targetBehavior = collision.gameObject.GetComponentInParent<TargetBehavior>();
            if (targetBehavior != null)
            {
                Debug.Log("targetBehavior Found");
                targetBehavior.puzzleCollision();
                Invoke("Delay", 0.05f);
            }
            else if (targetBehavior == null)
            {
                Debug.Log("targetBehavior Not Found");
            }

        }
    }

    private void OnTriggerEnter(Collider collision){
        ElementalObject elementalObject = collision.gameObject.GetComponent<ElementalObject>();
        if (elementalObject != null) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position - transform.forward, 
                                transform.forward, out hit)) {
                elementalObject.InteractWithElement(element, hit.point, hit.normal);
            }
            Invoke("Delay", 0.05f);
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
