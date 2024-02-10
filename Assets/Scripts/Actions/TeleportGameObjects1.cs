using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class TeleportArg
{
    [JsonProperty(Required = Required.Always)]
    public string Subject { get; set; }
    [JsonProperty(Required = Required.Always)]
    public int X { get; set; }
    [JsonProperty(Required = Required.Always)]
    public int Y { get; set; }
    private string _direction;
    
    public string Direction
    {
        get => _direction;
        set
        {
            if (!TeleportGameObjects1.directionVectors.ContainsKey(value.ToLower()))
            {
                throw new System.Exception($"{value} is not a valid direction");
            }
            _direction = value.ToLower();
        }
    }
}

public class TeleportsArgs
{
    [JsonProperty(Required = Required.Always)]
    public List<TeleportArg> Teleports { get; set; }
}



public class TeleportGameObjects1 : GameAction1
{
    public override string CommandName => "teleport";

    public static Dictionary<string, Vector2> directionVectors = new Dictionary<string, Vector2> {
        { "up", Vector2.up },
        { "down", Vector2.down },
        { "left", Vector2.left },
        { "right", Vector2.right },
        { "none", Vector2.zero }
    };

    protected override IEnumerator _Execute(object arguments)
    {
        IsComplete = false;

        var teleportArguments = (TeleportsArgs)arguments;

        var teleports = teleportArguments.Teleports;

        foreach (var teleport in teleports)
        {
            GameObject subject = GameObject.Find(teleport.Subject);
            if (subject == null)
                continue;

            subject.transform.position = new Vector3(teleport.X, teleport.Y, subject.transform.position.z);

            MovementAnimation movementAnimator = subject.GetComponent<MovementAnimation>();

            if (movementAnimator != null)
            {
                string directionString = teleport.Direction ?? "none";
                movementAnimator.SetFacingDirection(directionVectors[directionString]);
            }

            yield return null;
        }
        IsComplete = true;
    }
}
