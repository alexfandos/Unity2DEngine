using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameObjectTeleport
{ 
    public string gameObjectName;
    public Vector2 newPosition;
    public Direction newDirection = Direction.None;
}

[CreateAssetMenu(menuName = "Actions/TeleportGameObjects", fileName = "NewTeleportGameObjects")]
public class TeleportGameObjects : GameAction
{
    public List<GameObjectTeleport> gameObjectTeleports = new List<GameObjectTeleport>();

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
        
        foreach (GameObjectTeleport gameObjectTeleport in gameObjectTeleports)
        {
            GameObject gameObject = GameObject.Find(gameObjectTeleport.gameObjectName);
            if (gameObject == null || gameObjectTeleport.newPosition == null)
                continue;

            gameObject.transform.position = gameObjectTeleport.newPosition;

            MovementAnimation movementAnimator = gameObject.GetComponent<MovementAnimation>();

            if (movementAnimator != null)
            {
                movementAnimator.SetFacingDirection(directionVectors[gameObjectTeleport.newDirection]);

            }

            yield return null;
        }

        IsComplete = true;
    }
}
