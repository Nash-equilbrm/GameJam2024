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
    OnBtnClick,
    IgnoreTouch,
    StopIgnoreTouch,


    // MULTIPLAYER
    OnConnectToServer,
    OnConnectToServerSuccess,
    OnCreateRoom,
    OnJoinRoom,
    OnJoinRoomSuccess,
    OnJoinRoomFailed,
    OnPlayerEnter,
    SetPlayerID,
    TimeUp,
    OnPlayerLeft,


    // GAMEPLAY
    StartGamePlay,
    EndGamePlay,
    WonGame,
    LostGame,
    BackToMenu,
    OnTimeChanged,
    OnMapGenerate,
    OnHeightChanged,
    CameraShake,

    //PLAYER
    PlayerJump,
    PlayerHitDart,
    PlayerHitGround,
    PlayerFlip,
    DartHitDart,
    PlayerUseSkill,
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