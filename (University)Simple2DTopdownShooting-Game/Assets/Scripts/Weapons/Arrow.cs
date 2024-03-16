using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleLight.GamePlay;
using Vector2 = UnityEngine.Vector2;

public class Arrow : Ammo
{
    [Range(1,20)]
    [SerializeField] float arrowVelocity;
    SpriteRenderer arrowSprite;

    private void Start()
    {
        arrowSprite = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (!(ammoRB == null))
        {
            ammoRB.velocity =  -transform.up * arrowVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MainPlayer>() != null)
        {
            return;
        }
        ammoRB = null;
        Debug.Log("Arrow Hit Something");
        Destroy(gameObject, 0.1f);
    }
}
