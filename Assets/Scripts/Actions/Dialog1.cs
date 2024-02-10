using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogArgs
{
    public List<Message> Messages { get; set; }
    public List<Character> Characters { get; set; }
}

public class Dialog1 : GameAction1
{
    public override string CommandName => "Dialog";


    protected override IEnumerator _Execute(object argumets)
    {
        var dialogArguments = (DialogArgs)argumets;
        IsComplete = false;
        DialogManager.instance.OpenDialogue(dialogArguments.Messages , dialogArguments.Characters, CompleteConversation);
        while (!IsComplete)
        {
            yield return null;
        }
    }

    public void CompleteConversation()
    {
        IsComplete = true;
        Debug.Log("Finished");
    }
}
