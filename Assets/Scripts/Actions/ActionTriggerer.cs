using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerer : MonoBehaviour
{
    [TextArea(3, 100)]
    public string actionsJson;

    [SerializeField]
    public List<GameAction> actions = new List<GameAction>();
    private RandomMovement randomMovement;
    private int currentActionIndex = 0;
    private bool isExecutingAction = false;
    public bool lookAtTriggerSource;
    private Animator animator;



    void Awake()
    {
        randomMovement = GetComponent<RandomMovement>();
        animator = GetComponent<Animator>();
    }

    private IEnumerator PerformActions()
    {
        PlayerMovement.instance.DeactivateControl();
        while(currentActionIndex < actions.Count)
        {
            isExecutingAction = true;
            if (randomMovement != null)
            {
                randomMovement.activeWalk = false;
            }
            GameAction action = actions[currentActionIndex];
            action.Execute();

            while (!action.IsComplete)
            {
                yield return null;
            }
            if (action.Break && action.Executed)
                break;
            currentActionIndex++;
        }
       
        currentActionIndex = 0; // Reset to the first action
        isExecutingAction = false; // Stop executing if you've reached the end of the list
        randomMovement.activeWalk = true;

        PlayerMovement.instance.ActivateControl();
        CameraFollow.Instance.StartFollowingProtagonist();

    }

    // Call this method to start executing the predefined actions
    public void TriggerActions(Vector2 trigerSourcePosition)
    {
        if (randomMovement != null && randomMovement.IsMoving)
        {
            Debug.LogWarning("Cannot interact while moving.");
            return;
        }
        if (isExecutingAction)
            return;

        currentActionIndex = 0;
        isExecutingAction = true;

        if (lookAtTriggerSource && animator != null)
        {
            var sourceDirection = trigerSourcePosition - (Vector2)transform.position;
            animator.SetFloat("moveX", sourceDirection.x);
            animator.SetFloat("moveY", sourceDirection.y);
        }

        StartCoroutine(PerformActions());
    }
}