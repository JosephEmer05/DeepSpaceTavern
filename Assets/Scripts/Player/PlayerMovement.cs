using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float MoveSmoothTime;
    public float GravityStrength;
    public float JumpStrength;
    public float WalkSpeed;
    public float RunSpeed;
    public GameObject moveSpeedShopItem;

    private ShopItem shopManager;
    private CharacterController Controller;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;

    private Vector3 CurrentForceVelocity;

    bool speedBoosted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        shopManager = moveSpeedShopItem.GetComponent<ShopItem>();
        Controller = GetComponent<CharacterController>();
        WalkSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        RunSpeed = WalkSpeed * 2;

        Vector3 PlayerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };

        if (PlayerInput.magnitude > 1f)
        {
            PlayerInput.Normalize();
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerInput);
        float CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;

        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity,
            MoveVector * CurrentSpeed,
            ref MoveDampVelocity,
            MoveSmoothTime

        );

        Controller.Move(CurrentMoveVelocity * Time.deltaTime);

        Ray groundCheckray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckray, 1.25f))
        {
            CurrentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            {
                CurrentForceVelocity.y = JumpStrength;
            }
        }
        else
        {
            CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;
        }

        Controller.Move(CurrentForceVelocity * Time.deltaTime);

        if (!speedBoosted && shopManager.currentStock == 1)
        {
            IncreaseMoveSpeed();
        }


    }

    public void IncreaseMoveSpeed()
    {
        WalkSpeed += 3;
        speedBoosted = true;
    }
}
