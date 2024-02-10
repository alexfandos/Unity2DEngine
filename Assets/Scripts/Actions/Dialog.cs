using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Actions/Dialog", fileName = "NewDialog")]
public class Dialog : GameAction
{
    public List<Message> DialogLines = new List<Message>();
    public List<Character> Characters = new List<Character>();

    public void StartDialogue()
    {
        DialogManager.instance.OpenDialogue(DialogLines, Characters, CompleteConversation);
    }

    public override IEnumerator _Execute()
    {
        IsComplete = false;
        StartDialogue();
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
