using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectMovement
{
    public string gameObjectName;
    public List<GameObjectMovementStep> steps = new List<GameObjectMovementStep>();
}

[System.Serializable]
public class GameObjectMovementStep
{
    public Direction direction;
    public float time = 0.33f;
}


[CreateAssetMenu(menuName = "Actions/MoveGameObjects", fileName = "NewMoveGameObjects")]
public class MoveGameObjects : GameAction
{
    private int activeMovements = 0;
    public List<GameObjectMovement> gameObjectMovements = new List<GameObjectMovement>();
    private List<bool> isMoving = new List<bool>();

    private Dictionary<Direction, Vector2> directionVectors = new Dictionary<Direction, Vector2> {
        { Direction.Up, Vector2.up },
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left },
        { Direction.Right, Vector2.right },
        { Direction.None, Vector2.zero }
    };

    public override IEnumerator _Execute()
    {
        IsComplete = false;
        isMoving = new List<bool>();
        for (int i = 0; i < gameObjectMovements.Count; i++)
        {
            isMoving.Add(false);
        }

        int index = 0;
        foreach (GameObjectMovement gameObjectMovment in gameObjectMovements)
        {
            GameObject gameObject = GameObject.Find(gameObjectMovment.gameObjectName);

            CoroutineRunner.Instance.RunCoroutine(MoveGameObject(gameObject, gameObjectMovment.steps, index));

            index++;
        }

        while(activeMovements > 0)
        {
            yield return null;
        }

        IsComplete = true;
    }

    private IEnumerator MoveGameObject(GameObject gameObject, List<GameObjectMovementStep> steps, int index)
    {
        activeMovements++;

        foreach (GameObjectMovementStep step in steps)
        {
            if (step.time == 0)
                step.time = 0.33f;
            if (step.direction == Direction.None)
            {
                yield return new WaitForSeconds(step.time);
            }
            else
            {
                Vector2 targetPos = (Vector2) gameObject.transform.position + directionVectors[step.direction];

                while ((targetPos - (Vector2)gameObject.transform.position).sqrMagnitude > Mathf.Epsilon)
                {
                    float speed = 1f / step.time;
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, speed * Time.deltaTime);
                    yield return null;
                }
                gameObject.transform.position = targetPos;
            }
            yield return null;
        }

        activeMovements--;
    }
}
