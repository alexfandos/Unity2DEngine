using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ActionRegistry : MonoBehaviour
{
    private Dictionary<string, Type> actionTypeMap;

    void Awake()
    {
        actionTypeMap = new Dictionary<string, Type>();

        // Get all types that are subclass of Action and have a non-abstract implementation
        var actionTypes = Assembly
            .GetAssembly(typeof(GameAction1)) // Replace with the correct assembly if needed
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GameAction1)) && !t.IsAbstract);


        var actionTypes1 = Assembly.GetAssembly(typeof(GameAction1));
        var actionTypes2 = actionTypes1.GetTypes();
        var actionTypes3 = actionTypes2.Where(t => t.IsSubclassOf(typeof(GameAction1)) && !t.IsAbstract);

        foreach (var type in actionTypes)
        {
            GameObject tempGameObject = new GameObject();
            var tempInstance = tempGameObject.AddComponent(type) as GameAction1;

            actionTypeMap[tempInstance.CommandName] = type;

            Destroy(tempGameObject);
        }
    }

    void Update()
    {
        GetActionType("move");
    }

    public Type GetActionType(string commandName)
    {
        if (actionTypeMap.TryGetValue(commandName, out Type actionType))
        {
            return actionType;
        }
        else
        {
            Debug.LogError($"Action type for command '{commandName}' not found.");
            return null;
        }
    }
}