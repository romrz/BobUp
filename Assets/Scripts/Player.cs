using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float jumpHeight;
    public float jumpTime;
    public float maxSlideSpeed = 2.0f;

    public int tilesWide;

    public float rotationOnDie = 1f;
    public float scaleOnDie = 2f;

    private Vector2 _jumpVelocity;
    private float _gravity;
    private Vector3 _velocity;

    private Controller2D characterController;

    private bool isAlive = true;
    private bool thrownBySpring = false;

    private float _rotateDir;

	void Start () {
        CalculatePhysics();
        characterController = GetComponent<Controller2D>();
    }

    void CalculatePhysics() {
        float cameraHeight = Camera.main.orthographicSize * 2.0f;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        float desiredWidth = cameraWidth / tilesWide;
        float realJumpHeight = desiredWidth * jumpHeight;
        float distanceX = cameraWidth - 2 * desiredWidth;

        _gravity = 2 * realJumpHeight / (jumpTime * jumpTime);
        _jumpVelocity.x = distanceX / jumpTime;
        _jumpVelocity.y = _gravity * jumpTime;
    }

    void Update() {
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
            }

            if (characterController.collisionState.hasHorizontalCollision() && _velocity.y > 0)
            {
                _velocity.y = 0;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _velocity = _jumpVelocity;
                if (Input.mousePosition.x <= Camera.main.pixelWidth / 2)
                {
                    _velocity.x *= -1;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) /*&& characterController.collisionState.hasCollision()*/)
            {
                _velocity.x = -_jumpVelocity.x;
                _velocity.y = _jumpVelocity.y;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) /*&& characterController.collisionState.hasCollision()*/)
            {
                _velocity = _jumpVelocity;
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

        _velocity.y -= _gravity * Time.deltaTime;

        characterController.Move(_velocity * Time.deltaTime);
    }

    public bool IsAlive() {
        return isAlive;
    }

    void Die() {
        characterController.collisionMask = new LayerMask();
        isAlive = false;
        _velocity = new Vector2(-Mathf.Sign(transform.position.x), 7);
        _rotateDir = -Mathf.Sign(transform.position.x);
        GetComponent<Rigidbody2D>().isKinematic = true;
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
                thrownBySpring = true;
            }
        }
    }
	
}
