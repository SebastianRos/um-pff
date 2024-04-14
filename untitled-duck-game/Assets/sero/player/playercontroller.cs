using UnityEditor.Animations;
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
    private AnimatorController anim;
    private AnimatorState state;

    void Start() {
        EventBus.Register("disablePlayer", gameObject);
        EventBus.Register("enablePlayer", gameObject);

        rb = GetComponent<Rigidbody2D>();

        anim = (AnimatorController)GetComponent<Animator>().runtimeAnimatorController;
        state = anim.layers[0].stateMachine.defaultState;
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
        AnimationClip activeAnimation;
        if (rb.velocity.magnitude != 0){
            activeAnimation = walkingAnim;
        } else {
            activeAnimation = idleAnim;
        }
        anim.SetStateEffectiveMotion(state, activeAnimation);
    }

    public void Callback(string evt) {
        if (evt.Equals("disablePlayer")) isMovementEnabled = false;
        else if (evt.Equals("enablePlayer")) isMovementEnabled = true;
    }
}