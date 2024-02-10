using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;




public class ActionItem
{
    [JsonProperty(Required = Required.Always)]
    public string Type { get; set; }
    [JsonProperty(Required = Required.Always)]
    public dynamic Args { get; set; } // 'dynamic' is used for flexibility with different argument structures
}

public class ConditionArgs
{
    [JsonProperty(Required = Required.Always)]
    public List<List<string>> If { get; set; }
    [JsonProperty(Required = Required.Always)]
    public List<ActionItem> Then { get; set; }

    public List<ActionItem> Else { get; set; }
}


public class ActionParser
{

    public static List<ActionItem> Deserialize(string json)
    {
        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        var actionItems = JsonConvert.DeserializeObject<List<ActionItem>>(json, settings);

        foreach (var actionItem in actionItems)
        {
            DeserializeActionItem(actionItem);
        }
        return actionItems;
    }

    private static void DeserializeActionItem(ActionItem actionItem)
    {
        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        switch (actionItem.Type.ToLower())
        {
            case "condition":
                var conditionArgs = JsonConvert.DeserializeObject<ConditionArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = conditionArgs;
                // Process each action in the then and else clauses
                foreach (var thenAction in conditionArgs.Then)
                {
                    DeserializeActionItem(thenAction);
                }
                foreach (var elseAction in conditionArgs.Else ?? new List<ActionItem>())
                {
                    DeserializeActionItem(elseAction);
                }
                break;

            case "move":
                var moveArgs = JsonConvert.DeserializeObject<MovementsArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = moveArgs;
                break;

            case "dialog":
                var dialogArgs = JsonConvert.DeserializeObject<DialogArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = dialogArgs;
                break;

            case "operation":
                var operationArgs = JsonConvert.DeserializeObject<OperationArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = operationArgs;
                break;

            case "teleport":
                var teleportArgs = JsonConvert.DeserializeObject<TeleportsArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = teleportArgs;
                break;
            case "camera":
                var cameraControlArgs = JsonConvert.DeserializeObject<CameraControlArgs>(actionItem.Args.ToString(), settings);
                actionItem.Args = cameraControlArgs;
                break;

            // Add additional cases as needed

            default:
                Debug.Log($"Unknown action type: {actionItem.Type}");
                break;
        }
    }
}


public class JsonParserTest : MonoBehaviour
{
    // Start is called before the first frame update
    [TextArea(3, 100)]
    public string json;
    void Start()
    {

    }
}
