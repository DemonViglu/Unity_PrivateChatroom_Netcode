using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using CustomInspector;
using System;

[Serializable]
public class PlayerChat : NetworkBehaviour
{
    [SerializeField] private int PlayerId;

    private PhoneUIManager phoneUIManager;

    private void Start() {
        DontDestroyOnLoad(this);
    }

    private void Update() {
        if(!IsOwner) return;


    }
    
    public override void OnNetworkSpawn() {
        phoneUIManager = ChatUIManager.Instance.PhoneUIManager;
        RegisterMyself();
        if (IsOwner) {
            phoneUIManager.SetOwnerPlayerID(PlayerId);
            phoneUIManager.sendOutEvent += MySendMessage;
        }
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
    }


    [HorizontalLine("Debug")]

    [Button(nameof(SendMessgaeDebug),true)]
    [SerializeField]private int receiveId;
    [SerializeField]private string messsage;
    public void SendMessgaeDebug(int a) {
        Message message=(new Message(this.PlayerId, receiveId, messsage));
        if (!IsOwner) {
            return;
        }
        string tmp = JsonUtility.ToJson(message);
        SendMessageServerRpc(tmp);
    }

    public void MySendMessage(string str) {
        Message message = new Message(this.PlayerId, phoneUIManager.currentPlayerId, str);
        if (!IsOwner) {
            return;
        }
        string tmp= JsonUtility.ToJson(message);
        SendMessageServerRpc(tmp);
    }

    private bool hasRigister = false;
    private void RegisterMyself() {
        if (hasRigister||PlayerChatManager.instance == null) {
            return;
        }
        hasRigister=true;
        PlayerChatManager.instance.Register(this);
    }

    public void RegisterOther(int id) {
        //if (id == this.PlayerId) {
        //    return;
        //}
        RefreshPhone(id, "");
    }

    
    public void SetPlayerId(int id) {
        this.PlayerId=id;
    }

    public int GetPlayerId() {
        return this.PlayerId;
    }

    [ServerRpc]
    public void SendMessageServerRpc(string message) {
        PlayerChatManager.instance.SendMessageChat(message);
    }

    public void GetMessage(string tmp) {
        //only the owner can get the message;
        if (!IsOwner) {
            return;
        }
        Message message=JsonUtility.FromJson<Message>(tmp);
        RefreshPhone(message.sendPlayerId, message.message);

        Debug.Log("Receive a message from:" +message.sendPlayerId+", and the content is:"+message.message);
    }

    private void RefreshPhone(int playerId,string message) {
        if(!IsOwner) {
            return;
        }
        phoneUIManager.AddMessage(playerId,"Other: "+message);
    }

}
