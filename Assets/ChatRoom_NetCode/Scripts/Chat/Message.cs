using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{

    public int sendPlayerId;
    public int receivePlayerId;
    public string message;
    public Message(int sendPlayerId, int receivePlayerId,string message = "") {
        this.message = message;
        this.sendPlayerId = sendPlayerId;
        this.receivePlayerId = receivePlayerId;
    }
}
