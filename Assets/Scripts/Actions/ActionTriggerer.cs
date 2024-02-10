using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerer : MonoBehaviour
{
    [TextArea(3, 1000)]
    public string actionsJson;

    List<ActionItem> deseralizedActions;

    [SerializeField]
    private RandomMovement randomMovement;
    private ExecutionStack isExecutingAction = new ExecutionStack();
    public bool lookAtTriggerSource;
    private Animator animator;



    void Awake()
    {
        randomMovement = GetComponent<RandomMovement>();
        animator = GetComponent<Animator>();
        deseralizedActions = ActionParser.Deserialize(actionsJson);
    }

    private IEnumerator PerformActions()
    {
        isExecutingAction.StartActionList();
        PlayerMovement.instance.DeactivateControl();
        if (randomMovement != null)
        {
            randomMovement.activeWalk = false;
        }
        foreach (var action in deseralizedActions)
        {

            var actionType = ActionRegistry.Instance.GetActionType(action.Type);

            GameObject actionExecutor = new GameObject("ActionExecutor");

            GameAction1 gameAction = (GameAction1)actionExecutor.AddComponent(actionType);

            gameAction.Execute(action.Args);

            while (!gameAction.IsComplete)
            {
                yield return null;
            }

            Destroy(actionExecutor);
        }

        if (randomMovement != null)
        {
            randomMovement.activeWalk = true;
        }
        PlayerMovement.instance.ActivateControl();
        CameraFollow.Instance.StartFollowingProtagonist();
        isExecutingAction.EndActionList();
    }

    // Call this method to start executing the predefined actions
    public void TriggerActions(Vector2 trigerSourcePosition)
    {
        if (randomMovement != null && randomMovement.IsMoving)
        {
            Debug.LogWarning("Cannot interact while moving.");
            return;
        }
        if (isExecutingAction.AreActionsExecuting())
            return;

        if (lookAtTriggerSource && animator != null)
        {
            var sourceDirection = trigerSourcePosition - (Vector2)transform.position;
            animator.SetFloat("moveX", sourceDirection.x);
            animator.SetFloat("moveY", sourceDirection.y);
        }

        StartCoroutine(PerformActions());
    }
}

public class ExecutionStack
{
    int stackSize = 0;

    public void StartActionList()
    {
        stackSize++;
    }
    public void EndActionList()
    {
        stackSize--;
        if (stackSize < 0)
        {
            stackSize = 0;
        }
    }
    public bool AreActionsExecuting()
    {
        return stackSize > 0;
    }
}