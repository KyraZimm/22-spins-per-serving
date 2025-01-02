using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SplineContainer floor;
    [SerializeField] float speed;

    float floorLength;
    float currSplinePos;
    float dir;

    private void Start() {
        Vector3 startPos = floor.EvaluatePosition(0);
        rb.MovePosition(startPos);
    }

    private void Update() {
        dir = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        floorLength = floor.Spline.GetLength();
        currSplinePos = Mathf.Clamp(currSplinePos + (dir * speed * Time.fixedDeltaTime), 0, floorLength);

        var normalizedPos = currSplinePos / floorLength;
        floor.Evaluate(normalizedPos, out var pos, out var tangent, out var normal);
        rb.MovePosition(new Vector2(pos.x, pos.y));
        //TO-DO: edit player rotation
    }


}
