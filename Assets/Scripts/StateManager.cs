using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[System.Serializable]
public enum StateType
{
    Int,
    String,
    Bool
}

[System.Serializable]
public class State
{
    public StateType Type;
    public string Key;
    [ConditionalField(nameof(Type), false, StateType.Int)]
    public int IntValue;
    [ConditionalField(nameof(Type), false, StateType.String)]
    public string StringValue;
    [ConditionalField(nameof(Type), false, StateType.Bool)]
    public bool BoolValue;

}

public class StateManager : MonoBehaviour
{
    public List<State> InitialStates = new List<State>();
    public static StateManager Instance;
    public Dictionary<string, object> States = new Dictionary<string, object>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (State state in InitialStates)
        {
            object value = new object();
            switch (state.Type)
            {
                case StateType.Bool:
                    value = state.BoolValue;
                    break;
                case StateType.Int:
                    value = state.IntValue;
                    break;
                case StateType.String:
                    value = state.StringValue;
                    break;
            }
            States.Add(state.Key, value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
