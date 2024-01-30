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
    OnPopupShow,

    // MULTIPLAYER
    OnConnectToServer,
    OnConnectToServerSuccess,
    OnCreateRoom,
    OnJoinRoom,
    OnJoinRoomSuccess,
    OnPlayerEnter,
    SetPlayerID,
    TimeUp,

    // GAMEPLAY
    StartGamePlay,
    EndGamePlay,
    WonGame,
    LostGame,
    BackToMenu,
    OnTimeChanged,
    OnMapGenerate,
    OnHeightChanged,

    //PLAYER
    PlayerJump,
    PlayerHitDart,
    PlayerHitGround,
    PlayerFlip,
    DartHitDart,
    StartSummonSkill,
    SkillActive,


    // AUDIO
    StopAudio,
    OnMusicVolumeChanged,
    OnSFXVolumeChanged,

}


public enum LanguageId
{
    VIE,
    ENG
}