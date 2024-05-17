using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject createAndJoinGameObject;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            createAndJoinGameObject.SetActive(true);
            gameObject.SetActive(false);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
