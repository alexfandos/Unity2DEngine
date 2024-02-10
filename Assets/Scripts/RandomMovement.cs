using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private Coroutine randomWalkCoroutine;
    public bool IsMoving { get; protected set; }

    public float randomWalkPeriod = 1f;
    public float moveSpeed = 3;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask playerLayer;

    public bool activeWalk = true;

    public PolygonCollider2D walkableArea;


    // Start is called before the first frame update
    void Start()
    {
        randomWalkCoroutine = StartCoroutine(RandomWalkScheduler(randomWalkPeriod));
    }


    void StopRandomWalk()
    {
        if (randomWalkCoroutine != null)
        {
            StopCoroutine(randomWalkCoroutine);
        }
    }

    private IEnumerator RandomWalkScheduler(float delay)
    {
        while (true)
        {
            WalkRandomly();
            yield return new WaitForSeconds(delay);
        }
    }

    void WalkRandomly()
    {
        if (!activeWalk)
            return;
        float randomNumber = Random.value;
        if (randomNumber < 0.5f)
        {
            Vector2 input = new Vector2(0, 0);
            float randomDirection = Random.value;
            if (randomDirection < 0.25f)
            {
                input.x = 1;
            }
            else if (randomDirection < 0.5f)
            {
                input.y = 1;
            }
            else if (randomDirection < 0.75f)
            {
                input.x = -1;
            }
            else
            {
                input.y = -1;
            }
            Vector2 targetPos = (Vector2)transform.position + input;


            if (IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (Physics2D.OverlapCircle(targetPos, 0.2f, (solidObjectsLayer | interactableLayer) | playerLayer) != null)
        {
            return false;
        }

        if (walkableArea == null)
            return true;

        
        if (!walkableArea.OverlapPoint(targetPos))
        {
            return false;
        }

        return true;
    }
}

