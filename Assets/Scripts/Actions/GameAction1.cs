using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class AndConditions1
{
    public List<string> andConditions = new List<string>();
}

public abstract class GameAction1 : MonoBehaviour
{
    public abstract string CommandName { get; }
    public bool IsComplete { get; protected set; } = false;

    public bool Executed = false;

    protected abstract IEnumerator _Execute(object arguments);

    public void Execute(object arguments)
    {
        IsComplete = false;
        StartCoroutine(_Execute(arguments));
    }


}



public interface ICompare
{
    bool Compare(object state, string operation, string value);
}

public class StringCompare : ICompare
{
    public bool Compare(object state, string operation, string value)
    {
        switch (operation)
        {
            case "=":
                return (string)state == value;
            case "!=":
                return (string)state != value;
            default:
                throw new ArgumentException($"Unsupported operation type for string: {operation}");
        }
    }
}

public class IntCompare : ICompare
{
    public bool Compare(object state, string operation, string value)
    {
        switch (operation)
        {
            case "=":
                return (int)state == int.Parse(value);
            case "!=":
                return (int)state != int.Parse(value);
            case ">":
                return (int)state > int.Parse(value);
            case ">=":
                return (int)state >= int.Parse(value);
            case "<":
                return (int)state < int.Parse(value);
            case "<=":
                return (int)state <= int.Parse(value);
            default:
                throw new ArgumentException($"Unsupported operation type for int: {operation}");
        }
    }
}

public class BoolCompare : ICompare
{
    public bool Compare(object state, string operation, string value)
    {
        switch (operation)
        {
            case "=":
                return (bool)state == bool.Parse(value);
            case "!=":
                return (bool)state != bool.Parse(value);
            default:
                throw new ArgumentException($"Unsupported operation type for bool {operation}");
        }
    }
}

public static class CompareFactory
{
    public static ICompare GetComparer(object state)
    {
        if (state is int)
            return new IntCompare();
        else if (state is bool)
            return new BoolCompare();
        else if (state is string)
            return new StringCompare();
        else
            throw new ArgumentException($"Unsupported type: {state.GetType().ToString()}");
    }
}

public static class Compare
{
    private static Dictionary<string, object> demo = new Dictionary<string, object>();

    public static bool Run(string condition)
    {
        string[] parts = condition.Split(';');

        if (parts.Length != 3)
        {
            throw new ArgumentException($"Condition {condition} is illformed.");
        }

        if (!StateManager.Instance.States.TryGetValue(parts[0], out object state))
        {
            throw new ArgumentException($"{parts[0]} does not exist.");
        }

        return _Run(state, parts[1], parts[2]);
    }
    public static bool _Run(object state, string operation, string value)
    {
        ICompare comparer = CompareFactory.GetComparer(state);
        return comparer.Compare(state, operation, value);
    }
}