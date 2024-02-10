using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MovementAnimation : MonoBehaviour
{
    private Vector2[] oldPositions = new Vector2[2];
    private Vector2 faceDir;
    private bool isMoving;
    private Animator animator;
    public Direction direction { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        oldPositions[0] = transform.position;
        oldPositions[1] = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement =  (Vector2)transform.position - oldPositions[0];
        if (movement != Vector2.zero)
        {
            isMoving = true;
            faceDir.x = movement.x > 0 ? 1 : (movement.x < 0 ? -1 : 0);
            faceDir.y = movement.y > 0 ? 1 : (movement.y < 0 ? -1 : 0);
            animator.SetFloat("moveX", faceDir.x);
            animator.SetFloat("moveY", faceDir.y);
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("isMoving", isMoving);

        oldPositions[0] = oldPositions[1];
        oldPositions[1] = transform.position;
    }

    public void SetFacingDirection(Vector2 facingDirection)
    {
        if (facingDirection != Vector2.zero)
            faceDir = facingDirection;
        oldPositions[0] = transform.position;
        oldPositions[1] = transform.position;
        animator.SetFloat("moveX", faceDir.x);
        animator.SetFloat("moveY", faceDir.y);
    }
}
