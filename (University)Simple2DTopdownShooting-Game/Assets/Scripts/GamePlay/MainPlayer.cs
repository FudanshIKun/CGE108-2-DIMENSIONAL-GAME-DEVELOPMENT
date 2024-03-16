using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using LittleLight.Core;

namespace LittleLight.GamePlay
{
    public class MainPlayer : MonoBehaviour
    {
        #region PlayerControls System

        [Header("Player Controls & Movement")] [HideInInspector]
        public Rigidbody2D playerRB;

        //Input component
        public PlayerControllers playerControls;
        InputAction moveAction;
        InputAction interaction;
        InputAction WeaponKeyAction;
        InputAction WeaponPadAction;
        InputAction FiredAction;

        // Values
        public float moveSpeed;
        Vector2 moveDir = Vector2.zero;
        public Vector2 lastDir = Vector2.zero;

        void EnableActions()
        {
            moveAction = playerControls.Player.Move;
            interaction = playerControls.Player.Interact;
            WeaponKeyAction = playerControls.Player.WeaponKey;
            WeaponPadAction = playerControls.Player.WeaponPad;
            FiredAction = playerControls.Player.WeaponFire;
            moveAction.Enable();
            interaction.Enable();
            WeaponKeyAction.Enable();
            WeaponPadAction.Enable();
            FiredAction.Enable();
            interaction.performed += PlayerInteraction;
            WeaponKeyAction.performed += PlayerIsAiming;
            FiredAction.performed += PlayerFired;

        }

        void DisableActions()
        {
            moveAction.Disable();
            interaction.Disable();
            WeaponKeyAction.Disable();
            FiredAction.Disable();
        }

        #endregion

        #region Player Animation System

        [Header("Player Animation")] [HideInInspector]
        public Animator playerAnimator;

        public SpriteRenderer playerSprite;

        #endregion

        #region Player Interaction System

        Interactable interacted;
        [Range(0, 1)] [SerializeField] private float interactDistance;
        public LayerMask InteractableLayer;

        #endregion

        #region Player GamePlay System

        bool isAiming = false;
        Vector2 aimDir;
        Vector2 characterDir;
        [SerializeField] GameObject playerWeapon;
        Weapon currentWeapon;

        [Range(0, 100)] public int playerHP = 100;
        [Range(0, 150)] public int playerSanity = 150;


        #endregion

        void OnEnable()
        {
            EnableActions();
        }

        void OnDisable()
        {
            DisableActions();
        }

        void CollectComponents()
        {
            playerRB = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();
            foreach (Transform transform in this.transform)
            {
                if (transform.CompareTag("Weapon"))
                {
                    playerWeapon = transform.gameObject;
                    Debug.Log(this.name + " has equit " + playerWeapon.name);
                    break;
                }
            }

            playerWeapon.SetActive(false);
            playerSprite = GetComponent<SpriteRenderer>();
            currentWeapon = playerWeapon.GetComponent<Weapon>();
            
        }

        void Awake()
        {
            playerControls = new PlayerControllers(); // Set playerControls to the class created by Unity
        }

        void Start()
        {
            CollectComponents();
        }

        void FixedUpdate()
        {
            PlayerMovement();
            AimSystem(isAiming);
        }

        void Update()
        {
            playerAnimation();

        }

        void PlayerMovement()
        {
            // Movement Process
            float speed = moveSpeed;
            moveDir = moveAction.ReadValue<Vector2>();
            if (isAiming || WeaponPadAction.ReadValue<Vector2>() != Vector2.zero)
            {
                speed -= ( moveSpeed * 0.7f );
            }

            playerRB.MovePosition(playerRB.position + moveDir * speed * Time.deltaTime);
        }

        void playerAnimation()
        {
            // Animation Process
            if (isAiming || WeaponPadAction.ReadValue<Vector2>() != Vector2.zero)
            {
                playerAnimator.SetBool("isAiming", true);
                aimDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                playerAnimator.SetFloat("Horizontal", aimDir.x);
                playerAnimator.SetFloat("Vertical", aimDir.y);
            }
            else
            {
                playerAnimator.SetBool("isAiming", false);
                if (moveDir != Vector2.zero)
                {
                    lastDir = moveDir;
                    //Debug.DrawRay(transform.position,lastDir,Color.green);
                    playerAnimator.SetBool("isMoving", true);
                }
                else
                {
                    playerAnimator.SetBool("isMoving", false);
                }

                playerAnimator.SetFloat("Horizontal", lastDir.x);
                playerAnimator.SetFloat("Vertical", lastDir.y);
            }
        }
            
        void PlayerInteraction(InputAction.CallbackContext context)
        {
            // Interaction Process & Trigger
            RaycastHit2D inRay = Physics2D.Raycast(transform.position, lastDir, interactDistance, InteractableLayer);
            if (inRay.collider.CompareTag("Interactable"))
            {
                interacted = inRay.collider.gameObject.GetComponent<Interactable>();
                if (!interacted.HasInteractedOneTimes())
                {
                    interacted.ReportInteraction(this.name);
                    interacted.InteractAction();
                }
            }
        }

        void PlayerIsAiming(InputAction.CallbackContext context)
        {
            isAiming = true;
            Debug.Log("Player is aiming");

            // Manage Performed
            WeaponKeyAction.performed -= PlayerIsAiming;
            WeaponKeyAction.performed += PlayerIsNotAiming;
        }

        void PlayerIsNotAiming(InputAction.CallbackContext context)
        {
            isAiming = false;
            Debug.Log("PlayerIsNotAiming");

            // Manage Performed
            WeaponKeyAction.performed -= PlayerIsNotAiming;
            WeaponKeyAction.performed += PlayerIsAiming;
        }
        void PlayerFired(InputAction.CallbackContext context)
        {
            if (isAiming)
            {
                currentWeapon.Fired();
            }
        }
            
        void AimSystem(bool PlayerIsAimingOrNot)
        {
            int aimParameter;
            if (PlayerIsAimingOrNot || WeaponPadAction.ReadValue<Vector2>() != Vector2.zero)
            {
                playerWeapon.SetActive(true);


            }
            else
            {
                playerWeapon.SetActive(false);

            }
        }
    }
}

