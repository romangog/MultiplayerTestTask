using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour, ICreateRoomInvokable, IJoinRoomInvokable
{
    public event Action<CreateRoomCallArgs> CreateRoomCallEvent;
    public event Action<JoinRoomCallArgs> JoinRoomCallEvent;

    [SerializeField] private SimpleTouchGraphic _createRoomButton;
    [SerializeField] private SimpleTouchGraphic _joinRoomButton;
    [SerializeField] private TMP_InputField _createRoomNameInputField;
    [SerializeField] private TMP_InputField _joinRoomNameInputField;


    private void Start()
    {
        _createRoomButton.PointerDownEvent += OnCreateRoomButtonTapped;
        _joinRoomButton.PointerDownEvent += OnJoinRoomButtonTapped;
    }

    private void OnCreateRoomButtonTapped()
    {
        CreateRoomCallEvent?.Invoke(new CreateRoomCallArgs(_createRoomNameInputField.text));
    }

    private void OnJoinRoomButtonTapped()
    {
        JoinRoomCallEvent?.Invoke(new JoinRoomCallArgs(_joinRoomNameInputField.text));
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

public class CreateRoomCallArgs
{
    public string RoomName;

    public CreateRoomCallArgs(string roomName)
    {
        RoomName = roomName;
    }
}

public class JoinRoomCallArgs
{
    public string RoomName;

    public JoinRoomCallArgs(string roomName)
    {
        RoomName = roomName;
    }
}
