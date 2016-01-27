using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float jumpHeight;
    public float jumpTime;
    public float maxSlideSpeed = 2.0f;

    public float recoveryTime = 1.0f;

    public Vector3 normalScale;
    public int tilesWide;

    public float rotationOnDie = 1f;
    public float scaleOnDie = 2f;

    public RuntimeAnimatorController normalAnimator;
    public RuntimeAnimatorController fastAnimator;
    public RuntimeAnimatorController stickyAnimator;

    public float bigScale = 2.0f;

    public float jumpTimeNormal = 1f;
    public float jumpTimeFast = 0.5f;

    public float maxSlideSpeedSNormal = 1.5f;
    public float maxSlideSpeedSticky = 0.3f;

    private Vector2 _jumpVelocity;
    private float _gravity;
    private Vector3 _velocity;
    private float horizontalAcceleration = 10;
    private bool onWallClimb = false;
    private float jumpSide;
    public Vector2 jumpWallVelocity;

    private Controller2D characterController;
    private Animator animator;
    private SpriteRenderer renderer;

    private bool isAlive = true;
    private bool inRecovery = false;
    private bool thrownBySpring = false;
    private bool doubleJump = true;
    private int jumpCount = 0;

    private float _rotateDir;

    private enum State { Normal, Big, Fast, Sticky}
    private State state = State.Normal;

    private enum Place { Left, Air, Right }
    private Place place = Place.Air;

    private Vector2 screenSize;

    void Start () {
        jumpTime = jumpTimeNormal;
        maxSlideSpeed = maxSlideSpeedSNormal;

        screenSize = new Vector2();
        screenSize.y = Camera.main.orthographicSize * 2.0f;
        screenSize.x = Camera.main.aspect * screenSize.y;

        CalculatePhysics();

        transform.localScale = normalScale;

        characterController = GetComponent<Controller2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = normalAnimator;
        animator.Play("Idle");
    }

    void CalculatePhysics() {
        float desiredWidth = screenSize.x / tilesWide;
        float realJumpHeight = desiredWidth * jumpHeight;
        float distanceX = screenSize.x - 2 * desiredWidth;

        _gravity = 2 * realJumpHeight / (jumpTime * jumpTime);
        _jumpVelocity.x = distanceX / jumpTime;
        _jumpVelocity.y = _gravity * jumpTime;
    }

    void Update() {
        if(place == Place.Air && characterController.collisionState.left) {
            place = Place.Left;
            animator.Play("PlayerLeftArrive");
        }
        else if (place == Place.Air && characterController.collisionState.right) {
            place = Place.Right;
            animator.Play("PlayerRightArrive");
        }

        if (isAlive)
        {
            bool wallSliding = false;
            float playerSide;

            if (characterController.collisionState.hasHorizontalCollision() && !characterController.collisionState.hasVerticalCollision())
            {
                wallSliding = true;
                if (_velocity.y < -maxSlideSpeed)
                {
                    _velocity.y = -maxSlideSpeed;
                }
                playerSide = characterController.collisionState.left ? -1.0f : 1.0f;
                _velocity.x = playerSide * 0.1f;

                jumpCount = 0;
                onWallClimb = false;
            }

            if (characterController.collisionState.hasHorizontalCollision() && _velocity.y > 0)
            {
                _velocity.y = 0;
            }

            if (Input.GetMouseButtonDown(0) && (characterController.collisionState.hasCollision() || (doubleJump && jumpCount < 2)))
            {
                _velocity = _jumpVelocity;
                if (Input.mousePosition.x <= Camera.main.pixelWidth / 2)
                {
                    _velocity.x *= -1;
                    animator.Play("PlayerRightArrive");
                }
                else
                {
                    animator.Play("PlayerLeftArrive");
                }
                place = Place.Air;
                
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && (characterController.collisionState.hasCollision() || (doubleJump && jumpCount < 2)))
            {
                onWallClimb = false;
                if (characterController.collisionState.left)
                {
                    onWallClimb = true;
                    jumpSide = -1;
                    _velocity.x = jumpWallVelocity.x;
                    _velocity.y = jumpWallVelocity.y;
                }
                else
                {
                    _velocity.x = -_jumpVelocity.x;
                    _velocity.y = _jumpVelocity.y;
                }

                jumpCount++;

                place = Place.Air;
                animator.Play("PlayerRightArrive");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && (characterController.collisionState.hasCollision() || (doubleJump && jumpCount < 2)))
            {
                onWallClimb = false;
                if (characterController.collisionState.right)
                {
                    onWallClimb = true;
                    jumpSide = 1;
                    _velocity.x = -jumpWallVelocity.x;
                    _velocity.y = jumpWallVelocity.y;
                }
                else
                {
                    _velocity.x = _jumpVelocity.x;
                    _velocity.y = _jumpVelocity.y;
                }

                jumpCount++;

                place = Place.Air;
                animator.Play("PlayerLeftArrive");
            }
        }
        else {
            transform.Rotate(Vector3.forward * rotationOnDie * _rotateDir * Time.deltaTime);
        }

        if(thrownBySpring)
        {
            _velocity.y = _jumpVelocity.y;
            _velocity.x = 3.0f * _jumpVelocity.x * Mathf.Sign(transform.position.x) * -1f;
            thrownBySpring = false;
        }

        if(onWallClimb)
        {
            _velocity.x += horizontalAcceleration * jumpSide * Time.deltaTime;
        }
        _velocity.y -= _gravity * Time.deltaTime;

        characterController.Move(_velocity * Time.deltaTime);
    }

    public bool IsAlive() {
        return isAlive;
    }

    void SetNormalState() {
        transform.localScale = normalScale;
        state = State.Normal;
        animator.runtimeAnimatorController = normalAnimator;

        jumpTime = jumpTimeNormal;
        maxSlideSpeed = maxSlideSpeedSNormal;
        CalculatePhysics();

    }

    void Die() {
        if (inRecovery) return;
        if(state != State.Normal) {
            SetNormalState();
            inRecovery = true;
            InvokeRepeating("RecoveryAnimation", 0, .1f);
            Invoke("StopRecoveryAnimation", recoveryTime);
            return;
        }

        onWallClimb = false;

        characterController.collisionMask = new LayerMask();
        isAlive = false;
        _velocity = new Vector2(-Mathf.Sign(transform.position.x), 7);
        _rotateDir = -Mathf.Sign(transform.position.x);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void RecoveryAnimation()
    {
        renderer.enabled = !renderer.enabled;
    }

    void StopRecoveryAnimation()
    {
        inRecovery = false;
        renderer.enabled = true;
        CancelInvoke("RecoveryAnimation");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Spike") {
            Die();
        }
        else if(collision.gameObject.tag == "Bomb") {
            Die();
            collision.gameObject.GetComponent<Bomb>().explode();
            Destroy(collision.gameObject);
            Camera.main.GetComponent<CameraShake>().Shake();
        }
        else if(collision.gameObject.tag == "Laser") {
            if(collision.gameObject.GetComponent<Laser>().isActive())
            {
                Die();
                Camera.main.GetComponent<CameraShake>().Shake(0.1f, 0.002f);
            }
        }
        else if(collision.gameObject.tag == "Spring") {
            if (!collision.gameObject.GetComponent<ActionTile>().HasBeenActivated())
            {
                if (state == State.Sticky)
                {
                    animator.Play(transform.position.x < 0 ? "PlayerLeftArrive" : "PlayerRightArrive");
                }
                else
                {
                    thrownBySpring = true;

                }
            }
        }
        else if(collision.gameObject.tag == "Big")
        {
            if (!collision.gameObject.GetComponent<ActionTile>().HasBeenActivated() && state != State.Big)
            {
                state = State.Big;
                animator.runtimeAnimatorController = normalAnimator;

                // TODO: Refactor This! DRY!
                if (transform.position.x < 0)
                {
                    float originX = GetComponent<BoxCollider2D>().bounds.min.x;
                    transform.localScale = normalScale * bigScale;
                    float newX = GetComponent<BoxCollider2D>().bounds.min.x;
                    transform.position += new Vector3(originX - newX, 0, 0);
                }
                else
                {
                    float originX = GetComponent<BoxCollider2D>().bounds.max.x;
                    transform.localScale = normalScale * bigScale;
                    float newX = GetComponent<BoxCollider2D>().bounds.max.x;
                    transform.position += new Vector3(originX - newX, 0, 0);
                }

                jumpTime = jumpTimeNormal;
                maxSlideSpeed = maxSlideSpeedSNormal;
                CalculatePhysics();
            }
        }
        else if (collision.gameObject.tag == "Fast")
        {
            if (!collision.gameObject.GetComponent<ActionTile>().HasBeenActivated() && state != State.Fast)
            {
                // TODO: Refactor This! DRY!
                if (state == State.Big)
                {
                    if (transform.position.x < 0)
                    {
                        float originX = GetComponent<BoxCollider2D>().bounds.min.x;
                        transform.localScale = normalScale;
                        float newX = GetComponent<BoxCollider2D>().bounds.min.x;
                        transform.position += new Vector3(originX - newX, 0, 0);
                    }
                    else
                    {
                        float originX = GetComponent<BoxCollider2D>().bounds.max.x;
                        transform.localScale = normalScale;
                        float newX = GetComponent<BoxCollider2D>().bounds.max.x;
                        transform.position += new Vector3(originX - newX, 0, 0);
                    }
                }

                state = State.Fast;
                animator.runtimeAnimatorController = fastAnimator;
                animator.Play(transform.position.x < 0 ? "PlayerLeftArrive" : "PlayeRightArrive");

                jumpTime = jumpTimeFast;
                maxSlideSpeed = maxSlideSpeedSNormal;
                CalculatePhysics();
            }
        }
        else if (collision.gameObject.tag == "Sticky")
        {
            if (!collision.gameObject.GetComponent<ActionTile>().HasBeenActivated() && state != State.Sticky)
            {
                // TODO: Refactor This! DRY!
                if(state == State.Big)
                {
                    if (transform.position.x < 0)
                    {
                        float originX = GetComponent<BoxCollider2D>().bounds.min.x;
                        transform.localScale = normalScale;
                        float newX = GetComponent<BoxCollider2D>().bounds.min.x;
                        transform.position += new Vector3(originX - newX, 0, 0);
                    }
                    else
                    {
                        float originX = GetComponent<BoxCollider2D>().bounds.max.x;
                        transform.localScale = normalScale;
                        float newX = GetComponent<BoxCollider2D>().bounds.max.x;
                        transform.position += new Vector3(originX - newX, 0, 0);
                    }
                }

                state = State.Sticky;
                animator.runtimeAnimatorController = stickyAnimator;
                animator.Play(transform.position.x < 0 ? "PlayerLeftArrive" : "PlayeRightArrive");

                jumpTime = jumpTimeNormal;
                maxSlideSpeed = maxSlideSpeedSticky;
                CalculatePhysics();
            }
        }
    }
	
}
