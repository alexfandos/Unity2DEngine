using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class MovementArgs
{
    [JsonProperty(Required = Required.Always)]
    public string Subject { get; set; }

    private float _speed;
    [JsonProperty(Required = Required.Always)]
    public float Speed
    {
        get => _speed;
        set
        {
            if (value <= 0)
                throw new System.Exception("Speed must be greater than 0.");
            _speed = value;
        }
    }
    public List<Step> Steps { get; set; }
}

public class MovementsArgs
{
    [JsonProperty(Required = Required.Always)]
    public List<MovementArgs> Movements { get; set; }
}

public class Step
{
    private string _direction;
    [JsonProperty(Required = Required.Always)]
    public string Direction
    {
        get => _direction;
        set
        {
            if (!TeleportGameObjects1.directionVectors.ContainsKey(((string)value).ToLower()))
            {
                throw new System.Exception($"{(string)value} is not a valid direction");
            }
            _direction = ((string)value).ToLower();
        }
    }

    private float _speed;
    public float? Speed
    {
        get => _speed;
        set
        {
            if (value <= 0)
                throw new System.Exception("Speed must be greater than 0.");
            _speed = (float)value;
        }
    }
}


public class MoveGameObjects1 : GameAction1
{
    private int activeMovements = 0;
    private List<bool> isMoving = new List<bool>();

    public override string CommandName => "move";

    private Dictionary<string, Vector2> directionVectors = new Dictionary<string, Vector2> {
        { "up", Vector2.up },
        { "down", Vector2.down },
        { "left", Vector2.left },
        { "right", Vector2.right },
        { "none", Vector2.zero }
    };

    protected override IEnumerator _Execute(object arguments)
    {
        IsComplete = false;
        isMoving = new List<bool>();

        int index = 0;
        var movementArguments = (MovementsArgs)arguments;
        foreach (var gameObjectMovment in movementArguments.Movements)
        {
            isMoving.Add(false);
            GameObject gameObject = GameObject.Find(gameObjectMovment.Subject);

            StartCoroutine(MoveGameObject(gameObject, gameObjectMovment.Steps, gameObjectMovment.Speed, index));

            index++;
        }

        while(activeMovements > 0)
        {
            yield return null;
        }

        IsComplete = true;
    }

    private IEnumerator MoveGameObject(GameObject gameObject, List<Step> steps, float speed, int index)
    {
        activeMovements++;

        foreach (Step step in steps)
        {
            var stepSpeed = step.Speed ?? speed;

            if (step.Direction == "none")
            {
                yield return new WaitForSeconds(1f/stepSpeed);
            }
            else
            {
                Vector2 targetPos = (Vector2) gameObject.transform.position + directionVectors[step.Direction];

                while ((targetPos - (Vector2)gameObject.transform.position).sqrMagnitude > Mathf.Epsilon)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, stepSpeed * Time.deltaTime);
                    yield return null;
                }
                gameObject.transform.position = targetPos;
            }
            yield return null;
        }

        activeMovements--;
    }
}
