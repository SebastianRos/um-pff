using UnityEditor.Animations;
using UnityEngine;

public class playercontroller : MonoBehaviour
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
}
