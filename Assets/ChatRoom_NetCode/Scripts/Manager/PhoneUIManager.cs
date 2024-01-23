using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject IconPrefab;
    [SerializeField] private GameObject SentencePrefab;
    [SerializeField] private GameObject IconContent;
    [SerializeField] private GameObject SentenceContent;
    [SerializeField] private InputField InputField;
    [SerializeField] private Text inputText;
    [SerializeField] private Button sendOutButton;

    [SerializeField] private Text idText;
    public event Action<string> sendOutEvent;

    private Dictionary<int, List<string>> chatHistory;
    private List<GameObject> iconList;

    public int currentPlayerId = 0;

    [SerializeField] int ownerPlayerId;
    private void Start() {
        chatHistory = new Dictionary<int, List<string>>();
        iconList = new List<GameObject>();
        InitTialSendOutButton();
    }

    private void InitTialSendOutButton() {
        sendOutButton.onClick.AddListener(() =>{
            if (inputText.text == "") {
                Debug.Log("Input can not be empty!");
            }
            else {
                if(currentPlayerId!=ownerPlayerId)sendOutEvent?.Invoke(inputText.text);
                AddMessage(currentPlayerId,"Your: "+inputText.text);
                InputField.text = "";
                
            }

        });
    }

    public void AddMessage(int playerId, string message) {
        if (currentPlayerId == 0) {
            currentPlayerId= playerId;
            RefreshIconWithe();
        }

        if(chatHistory.ContainsKey(playerId)) {
            if (message != ""&&message!="Your: "&&message!="Other: ") {
                chatHistory[playerId].Add(message);
            }
        }
        else {
            chatHistory[playerId] = new List<string>();

            AddNewIcon(playerId);
            if (message != "" && message != "Your: " && message != "Other: ") {
                chatHistory[playerId].Add(message);
            }
        }

        if (currentPlayerId == playerId) {
            if (message != "" && message != "Your: " && message != "Other: ") {
                RefreshMessageBox(chatHistory[playerId]);
            }
        }

    }

    private void AddNewIcon(int id) {
        GameObject icon = Instantiate(IconPrefab, IconContent.transform);
        icon.GetComponentInChildren<Text>().text=id.ToString();
        icon.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPlayerId = int.Parse(icon.GetComponentInChildren<Text>().text);
            RefreshIconWithe();
            RefreshMessageBox(chatHistory[currentPlayerId]);
        });
        iconList.Add(icon);
    }

    private void RefreshMessageBox(List<string > strs) {
        for(int i=0;i<SentenceContent.transform.childCount;i++) {
            Destroy(SentenceContent.transform.GetChild(i).gameObject);
        }
        foreach(string str in strs) {
            GameObject sentence=Instantiate(SentencePrefab,SentenceContent.transform);
            sentence.GetComponent<Text>().text=str;
        }
    }

    public void SetOwnerPlayerID(int id) {
        ownerPlayerId = id;
        idText.text="ID: "+id.ToString();
    }

    private void RefreshIconWithe() {
        foreach (var icon in iconList) {
            if (currentPlayerId == int.Parse(icon.GetComponentInChildren<Text>().text)) {
                icon.GetComponent<Image>().color = Color.grey;
            }
            else {
                icon.GetComponent<Image>().color = Color.white;

            }
        }
    }

}
