using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.GamePlay
{
    public class Monster : MonoBehaviour
    {
        private Animator MonsterAnimator;
        [Range(0, 100)] public float MonsterHP;
        public bool hasDied;

        private void Start()
        {
            MonsterAnimator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (MonsterHP <= 0)
            {
                hasDied = true;
                MonsterAnimator.SetBool("Died", true);
                Destroy(gameObject,2f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ammo"))
            {
                if (hasDied)
                {
                    return;
                }
                MonsterAnimator.SetTrigger("Hitted");
                MonsterHP -= other.GetComponent<Ammo>().hitDMG;
                Destroy(other.gameObject);
            }
        }
    }
}
