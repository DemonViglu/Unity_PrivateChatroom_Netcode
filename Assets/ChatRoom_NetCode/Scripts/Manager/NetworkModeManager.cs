using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkModeManager : MonoBehaviour
{
    [SerializeField] private Button S;
    [SerializeField] private Button H;
    [SerializeField] private Button C;

    [SerializeField] private GameObject PhonePanel;

    [SerializeField] private Text Ip;
    [SerializeField] private Text Port;

    [SerializeField] private UnityTransport unitytransport;

    private string ip;
    private string port;

    private void Start() {
        S.onClick.AddListener(() =>
        {
            InitConnectionData();
            NetworkManager.Singleton.StartServer();
            Transition();
        });

        H.onClick.AddListener(() =>
        {
            InitConnectionData();
            NetworkManager.Singleton.StartHost();
            Transition();
        });

        C.onClick.AddListener(() =>
        {
            InitConnectionData();
            NetworkManager.Singleton.StartClient();
            Transition();
        });
    }

    private void InitConnectionData() {
        ip = Ip.text;
        port=Port.text;
        if (ip == "") ip = "127.0.0.1";
        if (port == "") port = "7777";
        unitytransport.SetConnectionData(ip,(ushort)int .Parse(port));
        //unitytransport.ConnectionData.Address = ip;
        //unitytransport.ConnectionData.Port = (ushort)int.Parse(port);
    }
    private void Transition() {
        SceneManager.LoadSceneAsync("Example");
        PhonePanel.SetActive(true);
    }
}
