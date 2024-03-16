using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.GamePlay
{
    public class Ammo : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D ammoRB;
        [Range(1,20)]
        public int hitDMG;

        void Awake()
        {
            ammoRB = GetComponent<Rigidbody2D>();
        }
    }
}
