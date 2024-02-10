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
    public static void Execute()
    {
        string json = @"
        [
            {   
                ""type"": ""condition"",
                ""args"": { 
                    ""if"": [
                        [""test.condition.1;>;4""],
                        [
                            ""test.condition.2;=;true"",
                            ""test.condition.3;=;true"",
                        ]
                    ],
                    ""then"": [
                        {
                            ""type"": ""move"",
                            ""args"": {   
                                ""movements"": [
                                    {
                                        ""subject"": ""NPC"",
                                        ""speed"": 5,
                                        ""steps"": [
                                            { ""direction"":""left"" },
                                            { ""direction"":""left"", ""speed"": 1 },
                                            { ""direction"":""left"" }
                                        ]
                                    }
                                ]
                            }
                        },
                        {
                            ""type"": ""dialog"",
                            ""args"": {
                                ""messages"": [
                                    { ""id"":0, ""text"": ""hey!"" },
                                    { ""id"":1, ""text"": ""back off!"" }
                                ],
                                ""characters"": [
                                    {
                                        ""name"": ""Protagonist"",
                                        ""face"": ""art/char/protagonist/face""
                                    },
                                    {
                                        ""name"": ""NPC"",
                                        ""face"": ""art/char/npc1/face""
                                    }
                                ]
                            }
                        }
                    ],
                    ""else"": [
                        {
                            ""type"": ""operation"",
                            ""args"": {
                                ""operations"": [""1.NPC.times"", ""+"", ""1""],
                            }
                        }
                    ]
                }
            }
        ]";

        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };
        var actionItems = JsonConvert.DeserializeObject<List<ActionItem>>(json, settings);

        foreach (var actionItem in actionItems)
        {
            DeserializeActionItem(actionItem);
        }

        Debug.Log(actionItems);
    }

    private static void DeserializeActionItem(ActionItem actionItem)
    {
        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        switch (actionItem.Type)
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

    // Update is called once per frame
    void Update()
    {
        ActionParser.Execute();
        if (Input.GetKeyDown(KeyCode.J))
        {

            DialogArgs dialogArgs = new DialogArgs();

            dialogArgs.Characters = new List<Character>();
            dialogArgs.Messages = new List<Message>();

            dialogArgs.Characters.Add(new Character { Name = "Protagonist", Face = "art/char/protagonist/face" });
            dialogArgs.Characters.Add(new Character { Name = "NPC", Face = "art/char/npc1/face" });

            dialogArgs.Messages.Add(new Message { Id = 0, Text = "Hola!" });
            dialogArgs.Messages.Add(new Message { Id = 1, Text = "Ey!" });

            GameObject gameObject = new GameObject("New GameObject");

            // Add ExampleMonoBehaviour to this new GameObject
            Dialog1 example = gameObject.AddComponent<Dialog1>();

            example.Execute(dialogArgs);
        }

    }
}
