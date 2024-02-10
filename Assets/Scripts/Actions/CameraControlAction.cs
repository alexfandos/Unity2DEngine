using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


[CreateAssetMenu(menuName = "Actions/CameraControlAction", fileName = "NewCameraControlAction")]
public class CameraControlAction : GameAction
{
    public CameraAction Action;

    [ConditionalField(false, nameof(ShowTarget))]
    public string Target;
    private bool ShowTarget() => Action == CameraAction.FollowTarget || Action == CameraAction.MoveToTarget || Action == CameraAction.TeleportToTarget;

    [ConditionalField(nameof(Action), false, CameraAction.Shake)]
    public float ShakeForce, ShakeFrequency, ShakeDuration;

    [ConditionalField(false, nameof(ShowTargetPosition))]
    public Vector2 TargetPosition;

    private bool ShowTargetPosition() => Action == CameraAction.Move || Action == CameraAction.Teleport;

    [ConditionalField(nameof(Action), false, CameraAction.Move)]
    public float MoveSpeed;
    // Start is called before the first frame update
    public override IEnumerator _Execute()
    {
        IsComplete = false;
        GameObject target;

        switch (Action)
        {
            case CameraAction.FollowProtagonist:
                CameraFollow.Instance.StartFollowingProtagonist();
                IsComplete = true;
                break;
            case CameraAction.FollowTarget:
                target = GameObject.Find(Target);
                CameraFollow.Instance.StartFollowingTarget(target);
                IsComplete = true;
                break;
            case CameraAction.Move:
                CameraFollow.Instance.StartMove(TargetPosition, MoveSpeed, Finish);
                break;
            case CameraAction.Shake:
                CameraFollow.Instance.StartShake(ShakeForce, ShakeFrequency, ShakeDuration, Finish);
                break;
            case CameraAction.Teleport:
                CameraFollow.Instance.Teleport(TargetPosition);
                IsComplete = true;
                break;
            case CameraAction.MoveToTarget:
                target = GameObject.Find(Target);
                CameraFollow.Instance.StartMove(target.transform.position, MoveSpeed, Finish);
                break;
            case CameraAction.TeleportToTarget:
                target = GameObject.Find(Target);
                CameraFollow.Instance.Teleport(target.transform.position);
                IsComplete = true;
                break;

        }

        yield return null;
    }

    private void Finish()
    {
        IsComplete = true;
    }
}
