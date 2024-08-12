using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayMovement_Mouse : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;

    private Vector2 targetPosition;
    private Vector2 movement;

    public Animator animator;
    private string currentString;

    private bool isMoving;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra nếu chuột được nhấn và không trên UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;
        }

        movement = (targetPosition - rb.position).normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", isMoving ? 1 : 0);
        
    }

    private void FixedUpdate()
    {
        if (isMoving == true)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, targetPosition) <= 0.1f)
            {
                isMoving = false;
                Debug.Log("Toi noi roi");
            }
        }
    }
}
