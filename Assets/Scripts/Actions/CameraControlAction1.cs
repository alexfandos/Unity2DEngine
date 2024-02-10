using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Newtonsoft.Json;

[System.Serializable]
public enum CameraAction
{
    FollowProtagonist,
    FollowTarget,
    Shake,
    Move,
    Teleport,
    MoveToTarget,
    TeleportToTarget
}

public class Position
{
    [JsonProperty(Required = Required.Always)]
    public int X { get; set; }
    [JsonProperty(Required = Required.Always)]
    public int Y { get; set; }
}

public class ShakeArgs
{
    private float _force;
    [JsonProperty(Required = Required.Always)]
    public float Force
    {
        get => _force;
        set
        {
            if (value <= 0)
                throw new System.Exception("Shake force must be greater than 0.");
            _force = value;
        }
    }
    [JsonProperty(Required = Required.Always)]
    private float _frequency;
    public float Frequency
    {
        get => _frequency;
        set
        {
            if (value <= 0)
                throw new System.Exception("Shake frequency must be greater than 0.");
            _frequency = value;
        }
    }
    private float _duration;
    [JsonProperty(Required = Required.Always)]
    public float Duration
    {
        get => _duration;
        set
        {
            if (value <= 0)
                throw new System.Exception("Shake duration must be greater than 0.");
            _duration = value;
        }
    }
}

public class CameraControlArgs
{
    private string _action;
    [JsonProperty(Required = Required.Always)]
    public string Action
    {
        get => _action;
        set
        {
            if (!CameraControlAction1.CameraActionTranslation.ContainsKey(value.ToLower())) {
                throw new System.Exception($"{value} is not a valid camera action");
            }
            _action = value.ToLower();
        }
    }
    public string Target { get; set; }
    public Position TargetPosition { get; set; }
    public float Speed { get; set; }
    public ShakeArgs Shake { get; set; }
}
    

public class CameraControlAction1 : GameAction1
{
    public override string CommandName => "camera";
    public static Dictionary<string, CameraAction> CameraActionTranslation = new Dictionary<string, CameraAction>
    {
        { "follow_protagonist", CameraAction.FollowProtagonist },
        { "follow_target", CameraAction.FollowTarget },
        { "shake", CameraAction.Shake },
        { "move", CameraAction.Move },
        { "teleport", CameraAction.Teleport },
        { "move_to_target", CameraAction.MoveToTarget },
        { "teleport_to_target", CameraAction.TeleportToTarget }
    };


    protected override IEnumerator _Execute(object arguments)
    {
        CameraControlArgs cameraArguments = (CameraControlArgs)arguments;
        IsComplete = false;
        GameObject target;
        Vector2 targetPosition;
        if (cameraArguments.Speed == null)
            cameraArguments.Speed = 3;

        CameraAction action = CameraActionTranslation[cameraArguments.Action];

        switch (action)
        {
            case CameraAction.FollowProtagonist:
                CameraFollow.Instance.StartFollowingProtagonist();
                IsComplete = true;
                break;
            case CameraAction.FollowTarget:
                if (cameraArguments.Target == null)
                    throw new System.Exception("Camera cannot follow target without target");
                target = GameObject.Find(cameraArguments.Target);
                CameraFollow.Instance.StartFollowingTarget(target);
                IsComplete = true;
                break;
            case CameraAction.Move:
                if (cameraArguments.TargetPosition == null)
                    throw new System.Exception("Camera cannot move without target position");
                targetPosition = new Vector2(cameraArguments.TargetPosition.X, cameraArguments.TargetPosition.Y);
                CameraFollow.Instance.StartMove(targetPosition, cameraArguments.Speed, Finish);
                break;
            case CameraAction.Shake:
                if (cameraArguments.Shake == null)
                    throw new System.Exception("Camera cannot shake without shake parameters");
                CameraFollow.Instance.StartShake(cameraArguments.Shake.Force, cameraArguments.Shake.Frequency, cameraArguments.Shake.Duration, Finish);
                break;
            case CameraAction.Teleport:
                if (cameraArguments.TargetPosition == null)
                    throw new System.Exception("Camera cannot teleport without target position");
                targetPosition = new Vector2(cameraArguments.TargetPosition.X, cameraArguments.TargetPosition.Y);
                CameraFollow.Instance.Teleport(targetPosition);
                IsComplete = true;
                break;
            case CameraAction.MoveToTarget:
                if (cameraArguments.Target == null)
                    throw new System.Exception("Camera cannot move to target without target");
                target = GameObject.Find(cameraArguments.Target);
                CameraFollow.Instance.StartMove(target.transform.position, cameraArguments.Speed, Finish);
                break;
            case CameraAction.TeleportToTarget:
                if (cameraArguments.Target == null)
                    throw new System.Exception("Camera cannot teleport to target without target");
                target = GameObject.Find(cameraArguments.Target);
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
