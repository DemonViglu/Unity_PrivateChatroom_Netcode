using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    [SerializeField] private Button PhoneButton;
    [SerializeField] private GameObject phonePanel;
    public PhoneUIManager PhoneUIManager;
    public static ChatUIManager Instance;
    private void Awake() {
        if (Instance != null) {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }



    private void Start() {
        InitialButton();
    }

    private void InitialButton() {
        PhoneButton.onClick.AddListener(() =>
        {
            phonePanel.SetActive(!phonePanel.gameObject.activeSelf);
        });
    }
}
