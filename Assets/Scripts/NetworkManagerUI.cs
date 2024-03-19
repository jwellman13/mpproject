using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private GameObject gameLaunchUI;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() => 
        { 
            NetworkManager.Singleton.StartServer(); 
        });

        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            gameLaunchUI.SetActive(false);
        });

        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            gameLaunchUI.SetActive(false);
        });
    }
}
