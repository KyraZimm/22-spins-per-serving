using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [SerializeField] float simulatedMass;
    [SerializeField] LayerMask influencedLayers;

    HashSet<Rigidbody2D> influencedRBs = new HashSet<Rigidbody2D>();
    private const float GRAVITATIONAL_CONSTANT = 6.67f * (10 ^ -11);

    private void FixedUpdate() {
        foreach (Rigidbody2D rb in influencedRBs) {
            Vector2 dir = rb.transform.position - transform.position;
            float force = (GRAVITATIONAL_CONSTANT * rb.mass * simulatedMass) / dir.sqrMagnitude;
            rb.velocity += force * dir.normalized * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if ((influencedLayers.value & (1 << col.gameObject.layer)) != 0) {
            influencedRBs.Add(col.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        influencedRBs.Remove(col.attachedRigidbody);
    }

}
