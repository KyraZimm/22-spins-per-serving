using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] SplineContainer path;

    [SerializeField] float speed;
    [SerializeField] float accelTime = 0.1f;
    [SerializeField] float decelTime = 0.1f;

    private Rigidbody2D rb;

    private float dir = 0;
    private float vel = 0;
    private float pathPos = 0;

    private void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float initialPos = 0;
        path.Evaluate(initialPos, out var pos, out var tangent, out var _);
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

        rb.MovePosition(new Vector2(pos.x, pos.y));
        rb.MoveRotation(angle);
    }

    private void Update()
    {
        dir = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        vel = calcVel(vel);
        pathPos += vel * Time.fixedDeltaTime;
        pathPos = wrapPath(pathPos);

        float normalizedPos = pathPos / path.Spline.GetLength();
        path.Evaluate(normalizedPos, out var pos, out var tangent, out var normal);
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

        rb.MovePosition(new Vector2(pos.x, pos.y));
        rb.MoveRotation(angle);
    }

    private float calcVel(float vel)
    {
        float targetVel = dir * speed;

        if (accelTime <= 0 && decelTime <= 0)
            return targetVel;

        // Accelerate
        if (Mathf.Abs(targetVel) > Mathf.Abs(vel))
        {
            if (accelTime <= 0)
                return targetVel;

            float acceleration = speed / accelTime;
            float sign = Mathf.Sign(targetVel - vel);
            float newVel = vel + sign * acceleration * Time.fixedDeltaTime;

            return Mathf.Clamp(newVel, -speed, speed);
        }
        // Decelerate
        else if (Mathf.Abs(targetVel) < Mathf.Abs(vel))
        {
            if (decelTime <= 0)
                return targetVel;

            float deceleration = speed / decelTime;
            float sign = Mathf.Sign(targetVel - vel);
            float newVel = vel + sign * deceleration * Time.fixedDeltaTime;

            // Clamp between target velocity and current velocity
            float minVel = Mathf.Min(targetVel, vel);
            float maxVel = Mathf.Max(targetVel, vel);
            return Mathf.Clamp(newVel, minVel, maxVel);
        }
        else
        {
            return targetVel;
        }
    }

    private float wrapPath(float pos)
    {
        if (pos < 0)
        {
            return path.Spline.GetLength() + (pos % path.Spline.GetLength());
        }
        return pos % path.Spline.GetLength();
    }
}
