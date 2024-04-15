using UnityEngine;

public class playercontroller : MonoBehaviour, IListener
{
    public int maxSpeed;
    public int accelartion;
    public int decceleration;
    public bool isMovementEnabled = true;

    public AnimationClip idleAnim;
    public AnimationClip walkingAnim;

    private Rigidbody2D rb;
    private Animator animator;

    void Start() {
        EventBus.Register("disablePlayer", gameObject);
        EventBus.Register("enablePlayer", gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        moveService();
        animationService();
    }

    private void moveService(){
        Vector2 inputDirection = isMovementEnabled
            ? new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            ).normalized 
            : new Vector2(0, 0);
        Vector2 velocity = inputDirection * maxSpeed;
        
        rb.velocity = velocity;
    }
    private void animationService(){
        if (rb.velocity.magnitude != 0){
            animator.SetBool("isWaling", true);
        } else {
            animator.SetBool("isWaling", false);
        }
    }

    public void Callback(string evt) {
        if (evt.Equals("disablePlayer")) isMovementEnabled = false;
        else if (evt.Equals("enablePlayer")) isMovementEnabled = true;
    }
}