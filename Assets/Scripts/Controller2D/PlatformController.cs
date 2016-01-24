using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : Controller2D {

    public LayerMask passengerMask;

    protected int passengerCount = 0;

    protected override void Start () {
        base.Start();
    }

    public bool HasPassengers() {
        return passengerCount > 0;
    }

    public void MovePlatform(Vector3 distance) {
        UpdateRaycastOrigins();

        if (distance.y > 0) {
            MovePassengers(distance);
            Move(distance);
        }
        else {
            Move(distance);
            MovePassengers(distance);
        }
    }

    protected void MovePassengers(Vector3 distance)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(distance.x);
        float directionY = Mathf.Sign(distance.y);

        if (distance.y != 0)
        {
            float rayLength = Mathf.Abs(distance.y) + _skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (_horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float moveX = (directionY == 1) ? distance.x : 0;
                        float moveY = distance.y - (hit.distance - _skinWidth) * directionY;

                        hit.transform.GetComponent<Controller2D>().Move(new Vector3(moveX, moveY), true);
                    }
                }
            }
        }

        if (distance.x != 0)
        {
            float rayLength = Mathf.Abs(distance.x) + _skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float moveX = distance.x - (hit.distance - _skinWidth) * directionX;
                        float moveY = 0;

                        hit.transform.GetComponent<Controller2D>().Move(new Vector3(moveX, moveY), true);
                    }
                }
            }
        }

        if (directionY == -1 || distance.y == 0)
        {
            float rayLength = _skinWidth * 2;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = _raycastOrigins.topLeft + Vector2.right * (_horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float moveX = distance.x;
                        float moveY = distance.y;

                        hit.transform.GetComponent<Controller2D>().Move(new Vector3(moveX, moveY), true);
                    }
                }
            }
        }

        passengerCount = movedPassengers.Count;
    }
}
