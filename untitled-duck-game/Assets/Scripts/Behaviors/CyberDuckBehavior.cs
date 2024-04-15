using UnityEngine;

public class CyberDuckBehaviour : MonoBehaviour {
    public GameObject lazerPrefab;
    public float fireDuration;
    public float fireCooldown;
    public int EnemyTargetRange = 10;
    public LayerMask obstacles;

    private GameObject lazerRef;

    private Animator animator;

    private GameObject[] enemies;
    private DuckBrain duckBrain;
    private float fireEndTime;
    private float nextFire = 0;
    
    public void Start(){
        animator = GetComponent<Animator>();
        duckBrain = GetComponent<DuckBrain>();
    }
    public void Update(){
        updateEnemies();
        if (!animator.GetBool("isFiring") && nextFire < Time.time){
            searchService();
        }
        if (animator.GetBool("isFiring")){
            fireService();
        }
    }

    public void updateEnemies(){
       enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void searchService(){
        foreach (GameObject enemy in enemies){
            RaycastHit2D hit = Physics2D.Raycast(
                animator.gameObject.transform.position,
                enemy.transform.position - animator.gameObject.transform.position,
                EnemyTargetRange,
                obstacles
            );
            if (hit.collider != null && hit.collider.gameObject == enemy){
                startFiring(hit.transform.gameObject);
                break;
            }
        }
    }
    private void fireService(){
        if (fireEndTime <= Time.time){
            stopFiring();
       }
    }
    private void startFiring(GameObject target){
        animator.SetBool("isFiring", true);
        fireEndTime = Time.time + fireDuration;

        duckBrain.isMoving = false;
        Vector3 direction = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = lookRotation;
        lazerRef = Instantiate(
            lazerPrefab, 
            transform.position + direction/2, 
            lookRotation
        );
        SpriteRenderer lazerSprite = lazerRef.GetComponent<SpriteRenderer>();
        lazerSprite.size = new Vector2(
            lazerSprite.size.x, 
            direction.magnitude
        );
        Destroy(target);
    }
    private void stopFiring(){
        animator.SetBool("isFiring", false);
        Destroy(lazerRef);
        duckBrain.isMoving = true;
        nextFire = Time.time + fireCooldown;
    }
}
