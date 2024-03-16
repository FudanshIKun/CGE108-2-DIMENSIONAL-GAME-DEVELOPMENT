using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleLight.GamePlay
{
    public class Weapon : MonoBehaviour
    {
        [HideInInspector]
        public MainPlayer weaponHolder;
        [HideInInspector]
        public SpriteRenderer weaponSprite;
        
        void CollectComponents(){
            weaponHolder = GamePlayManager.Instance.mainCharacter.GetComponent<MainPlayer>();
            weaponSprite = GetComponent<SpriteRenderer>();
        }

        void OnEnable()
        {
            SortLayer();
        }
        
        void Start(){
            CollectComponents();
        }

        void FixedUpdate()
        {
            MouseAiming();
        }

        public virtual void MouseAiming()
        {
            Vector3 difference = this.transform.localEulerAngles;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float zRotation = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f,0f,zRotation + 90f);
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y)
            {
                weaponSprite.sortingOrder = 2;
            }
            else
            {
                weaponSprite.sortingOrder = 3;
            }
        }
        public virtual void Fired()
        {
            Debug.Log(weaponHolder.name + " has fired " + this.name );
        }
        void SortLayer()
        {
            weaponSprite.sortingLayerName = weaponHolder.playerSprite.sortingLayerName;
        }
    }
}
