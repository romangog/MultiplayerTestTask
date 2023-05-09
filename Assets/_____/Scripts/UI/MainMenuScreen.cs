using System;
using TMPro;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour, ICreateRoomInvokable, IJoinRoomInvokable, IConnectionInteractionBlockable, IConnectionInfoDisplayable
{
    public event Action<CreateRoomCallArgs> CreateRoomCallEvent;
    public event Action<JoinRoomCallArgs> JoinRoomCallEvent;
    public event Action CancelConnectionCallEvent;

    [SerializeField] private SimpleTouchGraphic _createRoomButton;
    [SerializeField] private SimpleTouchGraphic _joinRoomButton;
    [SerializeField] private TMP_InputField _createRoomNameInputField;
    [SerializeField] private TMP_InputField _joinRoomNameInputField;
    [SerializeField] private SimpleTouchGraphic _cancelConnectionButton;
    [SerializeField] private GameObject _connectionView;
    [SerializeField] private TMP_Text _connectionInfoLabel;


    private void Start()
    {
        _createRoomButton.PointerDownEvent += OnCreateRoomButtonTapped;
        _joinRoomButton.PointerDownEvent += OnJoinRoomButtonTapped;
        _cancelConnectionButton.PointerDownEvent += OnConnectionCancelButtonTapped;
    }


    private void OnCreateRoomButtonTapped()
    {
        CreateRoomCallEvent?.Invoke(new CreateRoomCallArgs(_createRoomNameInputField.text));
    }

    private void OnJoinRoomButtonTapped()
    {
        JoinRoomCallEvent?.Invoke(new JoinRoomCallArgs(_joinRoomNameInputField.text));
    }

    public void BlockInteraction()
    {
        _connectionView.SetActive(true);
    }

    private void OnConnectionCancelButtonTapped()
    {
        CancelConnectionCallEvent?.Invoke();
        _connectionView.SetActive(false);
    }

    public void DisplayConnectionInfo(string info)
    {
        _connectionInfoLabel.text = info;
    }
}

public interface ICreateRoomInvokable
{
    public event Action<CreateRoomCallArgs> CreateRoomCallEvent;
}

public interface IJoinRoomInvokable
{
    public event Action<JoinRoomCallArgs> JoinRoomCallEvent;
}

public interface IConnectionInteractionBlockable
{
    void BlockInteraction();
    event Action CancelConnectionCallEvent;
}

public interface IConnectionInfoDisplayable
{
    void DisplayConnectionInfo(string info);
}

public class CreateRoomCallArgs
{
    public string RoomName;

    public CreateRoomCallArgs(string roomName)
    {
        if (roomName == "")
            RoomName = "Room1";
        else
            RoomName = roomName;
    }
}

public class JoinRoomCallArgs
{
    public string RoomName;

    public JoinRoomCallArgs(string roomName)
    {
        if (roomName == "")
            RoomName = "Room1";
        else
            RoomName = roomName;
    }
}
