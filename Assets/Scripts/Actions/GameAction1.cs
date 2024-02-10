using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
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