using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Globalization;

public class OperationArgs
{
    [JsonProperty(Required = Required.Always)]
    public List<string> Operations { get; set; }
}


public class ChangeGameState1 : GameAction1
{
    // Start is called before the first frame update
    public string Operation;
    public override string CommandName => "operation";
    protected override IEnumerator _Execute(object arguments)
    {
        OperationArgs operationArguments = (OperationArgs)arguments;

        IsComplete = false;

        foreach (string operation in operationArguments.Operations)
        {
            string[] parts = operation.Split(';');

            if (parts.Length != 3)
            {
                throw new ArgumentException($"Operation {Operation} is illformed.");
            }

            if (!StateManager.Instance.States.TryGetValue(parts[0], out object state))
            {
                throw new ArgumentException($"{parts[0]} does not exist.");
            }

            IGameStateOperation aoperation = GamteStateOperationFactory.GetOperator(state);

            object newState = aoperation.Run(state, parts[1], parts[2]);

            StateManager.Instance.States[parts[0]] = newState;
        }

        IsComplete = true;
        yield return null;
    }
}


public interface IGameStateOperation
{
    object Run(object state, string operation, string value);
}

public class NumeralGameStateOperation : IGameStateOperation
{
    public object Run(object state, string operation, string value)
    {
        float _value;
        try
        {
            _value = float.Parse(value, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            if (!StateManager.Instance.States.TryGetValue(value, out object valueValue))
            {
                throw new ArgumentException($"{value} does not exist.");
            }
            _value = (float)valueValue;
        }

        switch (operation)
        {
            case "=":
                return _value;
            case "+":
                return (float)state + _value;
            case "-":
                return (float)state - _value;
            case "*":
                return (float)state * _value;
            case "/":
                return (float)state / _value;
            default:
                throw new ArgumentException($"Unsupported operation type for numeric operation: {operation}");
        }
    }
}

public static class GamteStateOperationFactory
{
    public static IGameStateOperation GetOperator(object state)
    {
        if (state is int || state is float)
            return new NumeralGameStateOperation();
        else
            throw new ArgumentException($"Unsupported type: {state.GetType()}");
    }
}