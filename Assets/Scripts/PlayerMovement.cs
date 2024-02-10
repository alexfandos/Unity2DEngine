using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private bool IsMoving;

    private Vector2 input, facingDir;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public static PlayerMovement instance;

    private Animator animator;
    bool activeControl = true;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        animator = GetComponent<Animator>();
    }

    public void ActivateControl()
    {
        activeControl = true;
    }

    public void DeactivateControl()
    {
        activeControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving && activeControl)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            

            if (input != Vector2.zero)
            {
                facingDir = input;
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                Vector2 targetPos = transform.position;
                targetPos.x += input.x > 0 ? 1 : (input.x < 0 ? -1 : 0);
                targetPos.y += input.y > 0 ? 1 : (input.y < 0 ? -1 : 0);

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                else
                {
                    MovementAnimation movementAnimator = gameObject.GetComponent<MovementAnimation>();

                    if (movementAnimator != null)
                    {
                        movementAnimator.SetFacingDirection(facingDir);

                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
    }

    void Interact()
    {
        var interactPos = (Vector2)transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        if (collider)
        {
            ActionTriggerer actionTriggerer = collider.GetComponent<ActionTriggerer>();
            if (actionTriggerer != null)
            {
                // Call the TriggerActions method
                actionTriggerer.TriggerActions(transform.position);
            }
        }
    }

    IEnumerator Move(Vector2 targetPos)
    {
        IsMoving = true;
        while ((targetPos - (Vector2)transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
}
