using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10;
    float damage = 1;
    public LayerMask collisionMask;

    float lifetime = 3;
    float skinWidth = .1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);

        if(initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);   
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        // print(hit.collider.gameObject.name);
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }

    private void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        GameObject.Destroy(gameObject);
    }
}
