public enum UIType
{
    Unknow,
    Screen,
    Popup,
    Notify,
    Overlap
}

public enum GameFlowState
{
    MainMenu,
    Lobby,
    Gameplay
}

public enum EventID
{
    // GUI
    OpenMainMenu,
    OnLanguageChange,
    OnGameLoading,

    // MULTIPLAYER
    OnConnectToServer,
    OnConnectToServerSuccess,
    OnCreateRoom,
    OnJoinRoom,
    OnJoinRoomSuccess,
    OnPlayerEnter,
    SetPlayerID,


    // GAMEPLAY
    StartGamePlay,
    EndGamePlay,
    WonGame,
    LostGame,
    BackToMenu,


    // AUDIO
    StopAudio
}


public enum LanguageId
{
    VIE,
    ENG
}