using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FindMatchUI : MonoBehaviour
{
    public const string DEFAULT_QUEUE = "unrank-match";

    private CreateTicketResponse createTicketResponse;
    private float pollTicketTimer;
    private float pollTicketTimerMax = 1.1f;

    //[SerializeField] private Transform lookingForMatchTransform;

    [SerializeField] private Button findMatchButton;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private bool isFindingMatch = false;

    private void Awake()
    {
        findMatchButton.onClick.AddListener(() =>
        {
            //lookingForMatchTransform.gameObject.SetActive(false);

            if (!isFindingMatch)
            {
                FindMatch();
                isFindingMatch = true;
            }

            else
            {
                isFindingMatch = false;
            }
        });

        backButton.onClick.AddListener(() =>
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private async void FindMatch()
    {
        Debug.Log("FindMatch");

        //lookingForMatchTransform.gameObject.SetActive(true);

        createTicketResponse = await MatchmakerService.Instance.CreateTicketAsync(new List<Unity.Services.Matchmaker.Models.Player>
        {
            new Unity.Services.Matchmaker.Models.Player(AuthenticationService.Instance.PlayerId,
            new MatchmakingPlayerData
            {
                Skill = 100,
            })
        }, new CreateTicketOptions { QueueName = DEFAULT_QUEUE });

        pollTicketTimer = pollTicketTimerMax;
    }

    [Serializable]
    public class MatchmakingPlayerData
    {
        public int Skill;
    }

    private void Update()
    {
        if (createTicketResponse != null)
        {
            pollTicketTimer -= Time.deltaTime;
            if (pollTicketTimer <= 0f)
            {
                pollTicketTimer = pollTicketTimerMax;

                PollMatchmakerTicket();
            }
        }
    }

    private async void PollMatchmakerTicket()
    {
        Debug.Log("PollMatchmakerTicket");
        TicketStatusResponse ticketStatusResponse = await MatchmakerService.Instance.GetTicketAsync(createTicketResponse.Id);

        if (ticketStatusResponse == null)
        {
            Debug.Log("Null means no updates to this ticket, keep waiting");
            return;
        }

        if (ticketStatusResponse.Type == typeof(MultiplayAssignment))
        {
            MultiplayAssignment multiplayAssignment = ticketStatusResponse.Value as MultiplayAssignment;

            Debug.Log("multiplayAssignment.Status " + multiplayAssignment.Status);
            switch (multiplayAssignment.Status)
            {
                case MultiplayAssignment.StatusOptions.Found:
                    createTicketResponse = null;

                    Debug.Log(multiplayAssignment.Ip + " " + multiplayAssignment.Port);

                    string ip4Address = multiplayAssignment.Ip;
                    ushort port = (ushort)multiplayAssignment.Port;
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip4Address, port);

                    MatchGameMultiplayer.Instance.StartClient();
                    break;
                case MultiplayAssignment.StatusOptions.InProgress:
                    break;
                case MultiplayAssignment.StatusOptions.Failed:
                    createTicketResponse = null;
                    Debug.Log("Failed to create Multiplay Server!");
                    //lookingForMatchTransform.gameObject.SetActive(false);
                    break;
                case MultiplayAssignment.StatusOptions.Timeout:
                    createTicketResponse = null;
                    Debug.Log("Multiplay Timeout!");
                    //lookingForMatchTransform.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
