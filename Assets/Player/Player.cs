using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] SplineContainer path;
    [SerializeField] float speed;
    [SerializeField] float accelTime = 0.1f;
    [SerializeField] float decelTime = 0.1f;
    [Header("Firing")]
    [SerializeField] float firingInterval = 0.2f;
    [SerializeField] GameObject bulletType;
    [SerializeField] float bulletSpeed = 1f;
    [Header("Damage & Health")]
    [SerializeField] int maxHealth;
    [SerializeField] HealthBar healthBar;
    [SerializeField] LayerMask takeDamageLayers;
    [SerializeField] float takeDamageRadius; //NOTE: may be changing collision shape depending on final design for player sprite

    private Rigidbody2D rb;

    private float dir = 0;
    private float vel = 0;
    private float pathPos = 0;
    private float firingCooldown = 0;

    public int CurrHealth { get; private set; }
    public int MaxHealth {get { return maxHealth; } }

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

        CurrHealth = maxHealth;
        if (healthBar != null) healthBar.Init(maxHealth);
    }

    private void Update()
    {
        dir = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }

        CheckForDamage();
    }

    private void FixedUpdate()
    {
        DecrementTimers();
        vel = calcVel(vel);
        pathPos += vel * Time.fixedDeltaTime;
        pathPos = wrapPath(pathPos);

        float normalizedPos = pathPos / path.Spline.GetLength();
        path.Evaluate(normalizedPos, out var pos, out var tangent, out var normal);
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

        rb.MovePosition(new Vector2(pos.x, pos.y));
        rb.MoveRotation(angle);
    }

    private void DecrementTimers()
    {
        if (firingCooldown > 0) firingCooldown -= Time.fixedDeltaTime;
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

    private void Fire()
    {
        Debug.Log("Firing");
        if (firingCooldown <= 0)
        {
            if (bulletType == null)
            {
                Debug.LogError("No bullet type assigned!");
                return;
            }
            GameObject bullet = Instantiate(bulletType, transform.position, Quaternion.identity);
            // Calculate the direction towards (0, 0)
            Vector2 direction = ((Vector2)Vector3.zero - (Vector2)transform.position).normalized;
            // Apply velocity to the bullet's Rigidbody2D
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet prefab is missing a Rigidbody2D component!");
            }
            firingCooldown = firingInterval;
        }
    }

    private void CheckForDamage() {
        //NOTE: checking in a basic circular radius for now. May switch to using another casting method depending on final shape of player sprite
        Collider2D[] contacts = Physics2D.OverlapCircleAll(transform.position, takeDamageRadius, LayerMask.GetMask("Projectiles"));
        Debug.Log($"player made contact with {contacts.Length} damaging objects");

        foreach (Collider2D contact in contacts) {
            Damage damage = contact.GetComponent<Damage>();
            if (damage != null){
                TakeDamage(damage.Value);
            }
        }
    }

    public void TakeDamage(int damage) {
        Debug.Log($"Player took {damage} damage.");
        CurrHealth -= damage;
        if (healthBar != null) healthBar.TakeDamage(damage);
        if (CurrHealth <= 0) Debug.Log("Player died!");
    }
}
