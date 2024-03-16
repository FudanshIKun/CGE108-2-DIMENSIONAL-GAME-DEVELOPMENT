using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LittleLight.GamePlay
{
    public class Bow : Weapon
    {
        Animator bowAnimator;
        [SerializeField] GameObject arrow;
        
        public override void MouseAiming()
        {
            base.MouseAiming();
        }

        public override void Fired()
        {
            base.Fired();
            Vector3 origin = transform.position;
            arrow.layer = weaponHolder.gameObject.layer;
            arrow.GetComponent<SpriteRenderer>().sortingLayerName = weaponHolder.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
            Instantiate(arrow, origin, transform.rotation);
        }
    }
}
