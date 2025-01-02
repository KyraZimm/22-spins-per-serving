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

    private void Start() {
        Vector3 startPos = floor.EvaluatePosition(0);
        transform.position = startPos;
    }

    private void Update() {
        floorLength = floor.Spline.GetLength();

        float dir = Input.GetAxis("Horizontal");
        currSplinePos = Mathf.Clamp(currSplinePos + (dir * speed * Time.deltaTime), 0, floorLength);

        var normalizedPos = currSplinePos / floorLength;
        floor.Evaluate(normalizedPos, out var pos, out var tangent, out var normal);
        transform.position = pos;
    }
}
