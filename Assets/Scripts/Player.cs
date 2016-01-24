using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Vector2 _jumpVelocity;
    public float _gravity;

    private Vector3 _velocity;

    private Controller2D characterController;

	void Start () {
        characterController = GetComponent<Controller2D>();
    }

    void Update() {
        if (characterController.collisionState.hasCollision()) {
            _velocity.y = 0;
            _velocity.x = 0;
        }

        if(Input.GetMouseButtonDown(0)) {
            _velocity = _jumpVelocity;
            if(Input.mousePosition.x <= Camera.main.pixelWidth / 2) {
                _velocity.x *= -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) /*&& characterController.collisionState.hasCollision()*/) {
            _velocity.x = -_jumpVelocity.x;
            _velocity.y = _jumpVelocity.y;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) /*&& characterController.collisionState.hasCollision()*/) {
            _velocity = _jumpVelocity;
        }

        _velocity.y -= _gravity * Time.deltaTime;

        characterController.Move(_velocity * Time.deltaTime);
    }
	
}
