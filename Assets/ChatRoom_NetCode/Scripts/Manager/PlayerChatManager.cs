using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerChatManager : NetworkBehaviour
{
    [SerializeField]private List<PlayerChat> playerList;
    [SerializeField]private Dictionary<int,PlayerChat> playerChatDictionary;
    [SerializeField] private bool doSingleton;
    public static PlayerChatManager instance;

    
    private void Awake() {
        if (instance != null&&doSingleton) {
            Debug.LogError("Wrong");
            Destroy(instance);
        }
        instance = this;
        PlayerListInit();
        DontDestroyOnLoad(this);
    }


    private void PlayerListInit() {
        playerList = new List<PlayerChat>();
        playerChatDictionary = new Dictionary<int, PlayerChat>();
    }

    public void Register(PlayerChat player) {
        playerList.Add(player);
        player.SetPlayerId(playerList.Count);
        playerChatDictionary[player.GetPlayerId()]=player;
        foreach (PlayerChat chat in playerList) {
            chat.RegisterOther(player.GetPlayerId());
        }
        foreach(PlayerChat chat in playerList) {
            player.RegisterOther(chat.GetPlayerId());
        }
    }

    public void SendMessageChat(string tmp) {
        Message message=JsonUtility.FromJson<Message>(tmp);
        if (playerChatDictionary.ContainsKey(message.receivePlayerId)) {
            SendMessageClientRpc(tmp);
            return;
        }
        Debug.Log("The Player:" + message.receivePlayerId + " is not found");
    }

    [ClientRpc]
    private void SendMessageClientRpc(string  message) {
        Message tmpMessage=JsonUtility.FromJson<Message>(message);
        this.playerChatDictionary[tmpMessage.receivePlayerId].GetMessage(message);
    }
}
