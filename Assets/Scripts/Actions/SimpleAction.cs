using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Actions/SimpleAction", fileName = "NewSimpleAction")]
public class SimpleAction : GameAction
{
    public override IEnumerator _Execute()
    {
        IsComplete = false;
        IsComplete = true;
        yield return null;
    }
}
