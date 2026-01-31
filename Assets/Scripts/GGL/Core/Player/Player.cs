using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    [SerializeField] private E_InputAction inputAction = E_InputAction.WASD;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ChooseInputActionToInteract();
        
    }

    private void ChooseInputActionToInteract()
    {
        switch (inputAction)
        {
            case E_InputAction.WASD:
                // Handle WASD input
                HandleWASDInput();
                break;
            case E_InputAction.Mouse:
                // Handle Mouse input
                HandleMouseInput();
                break;
            default:
                break;
        }
    }

    private void HandleMouseInput()
    {
        Debug.Log("Mouse input handling not implemented yet.");
    }

    private void HandleWASDInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(h, v).normalized;
        this.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
