using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/ChangeGameState", fileName = "NewChangeGameState")]
public class ChangeGameState : GameAction
{
    // Start is called before the first frame update
    public string Operation;
    public override IEnumerator _Execute()
    {
        IsComplete = false;

        string[] parts = Operation.Split(';');

        if (parts.Length != 3)
        {
            throw new ArgumentException($"Operation {Operation} is illformed.");
        }

        if (!StateManager.Instance.States.TryGetValue(parts[0], out object state))
        {
            throw new ArgumentException($"{parts[0]} does not exist.");
        }
        IGameStateOperation operation = GamteStateOperationFactory.GetOperator(state);

        object newState = operation.Run(state, parts[1], parts[2]);

        StateManager.Instance.States[parts[0]] = newState;

        IsComplete = true;
        yield return null;
    }
}