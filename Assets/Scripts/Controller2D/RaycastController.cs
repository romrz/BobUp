using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    [Range(2, 20)]
    public int horizontalRayCount = 4;
    [Range(2, 20)]
    public int verticalRayCount = 4;

    protected float _skinWidth = 0.015f;
    protected float _horizontalRaySpacing;
    protected float _verticalRaySpacing;

    protected BoxCollider2D _collider;
    protected RaycastOrigins _raycastOrigins;

    protected virtual void Start () {
        _collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    protected void UpdateRaycastOrigins() {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    protected void CalculateRaySpacing() {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        _horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    protected struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

}
