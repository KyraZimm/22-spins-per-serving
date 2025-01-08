using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Attack Settings")]
    [SerializeField] float width = 0.2f;
    [Tooltip("Rate of turning in radians per second")]
    [SerializeField] float initialAngle = 0.0f;
    [SerializeField] float turnRate = Mathf.PI / 4.0f;

    private bool isActive = true;
    private float length = 5.0f;
    private float angle = 0.0f;

    void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + dir * length;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        angle += turnRate * Time.deltaTime;
        angle %= 2.0f * Mathf.PI;
    }

    public void StartAttack()
    {
        isActive = true;
        angle = initialAngle;
    }

    public void StopAttack()
    {
        isActive = false;
    }

    public bool IsAttacking()
    {
        return isActive;
    }

}
