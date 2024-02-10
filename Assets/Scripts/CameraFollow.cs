using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraFollowMode{
    NoFollow,
    Protagonist,
    OtherTarget
}

public class CameraFollow : MonoBehaviour
{
    public Transform Protagonist;

    [HideInInspector]
    public GameObject OtherTarget;

    public CameraFollowMode Mode = CameraFollowMode.Protagonist;

    public static CameraFollow Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

        // Update is called once per frame
        void Update()
    {
        Vector3 newPos;
        switch (Mode)
        {
            case CameraFollowMode.Protagonist:
                newPos = new Vector3(Protagonist.position.x, Protagonist.position.y, -10f);
                transform.position = newPos;
                break;
            case CameraFollowMode.OtherTarget:
                newPos = new Vector3(OtherTarget.transform.position.x, OtherTarget.transform.position.y, -10f);
                transform.position = newPos;
                break;
        }
       
    }

    IEnumerator Move(Vector2 targetPos, float moveSpeed, Action finishedAction)
    {
        Vector3 cameraTargetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        while ((targetPos - (Vector2)transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraTargetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = cameraTargetPos;
        finishedAction?.Invoke();
    }

    IEnumerator Shake(float force, float frequency, float duration, Action finishedAction)
    {
        Vector3 currentPosition = transform.position;
        float totalDuration = 0;
        float period = 1f / frequency;
        while (totalDuration < duration)
        {
            Vector2 randomVector2 = UnityEngine.Random.insideUnitCircle * force;
            Vector3 randomVector3 = new Vector3(randomVector2.x, randomVector2.y, currentPosition.z);
            transform.position = currentPosition + randomVector3;
            yield return new WaitForSeconds(period);
            totalDuration += period;
        }
        transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
        finishedAction?.Invoke();
    }

    public void StartMove(Vector2 targetPos, float moveSpeed, Action finishedAction)
    {
        Mode = CameraFollowMode.NoFollow;
        StartCoroutine(Move(targetPos, moveSpeed, finishedAction));
    }

    public void StartShake(float force, float frequency, float duration, Action finishedAction)
    {
        Mode = CameraFollowMode.NoFollow;
        StartCoroutine(Shake(force, frequency, duration, finishedAction));
    }

    public void StartFollowingProtagonist()
    {
        Mode = CameraFollowMode.Protagonist;
    }

    public void StartFollowingTarget(GameObject target)
    {
        OtherTarget = target;
        Mode = CameraFollowMode.OtherTarget;
    }

    public void Teleport(Vector2 targetPos)
    {
        Mode = CameraFollowMode.NoFollow;
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }
}
