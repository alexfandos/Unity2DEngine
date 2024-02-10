using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class Message
{
    private int _id;
    [JsonProperty(Required = Required.Always)]
    public int Id
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new System.Exception("DialogLine Id cannot be negative.");
            _id = value;
        }
    }
    [JsonProperty(Required = Required.Always)]
    public string Text { get; set; }
}

[System.Serializable]
public class Character
{
    [JsonProperty(Required = Required.Always)]
    public string Name;
    [JsonProperty(Required = Required.Always)]
    public string Face;
}

public class DialogManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    List<Message> currentMessages;
    List<Character> currentCharacters;
    int activeMessage = 0;
    Action callback;

    public bool isActive = false;
    public static DialogManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.Text;
        Character characterToDisplay = currentCharacters[messageToDisplay.Id];
        actorName.text = characterToDisplay.Name;
        actorImage.sprite = Resources.Load<Sprite>(characterToDisplay.Face);
        AnimateText();
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Count)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            callback?.Invoke();
        }
    }

    void AnimateText()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    public void OpenDialogue(List<Message> messages, List<Character> characters, Action aCallback)
    {
        isActive = true;
        currentMessages = messages;
        currentCharacters = characters;
        activeMessage = 0;
        callback = aCallback;

        Debug.Log("Started conversation!");

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    private void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Z))
        {
            NextMessage();
        }
    }
}
