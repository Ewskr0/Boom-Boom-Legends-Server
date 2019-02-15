// These classes must be the same on client / server.
// Package sent by the server

namespace BBL.Utils.Packages
{
    public enum ServerPackages
    {
        WelcomeMsg = 1,
        AccountData,
        AlertMsg,
        GamePosition,
        GameDropBomb,
        GameFound,
        GameStarted
    }

    // Package received from the client
    public enum ClientPackages
    {
        Login = 1,
        Register,
        ThankYou,
        GamePosition,
        GameDropBomb,
        GameFindLobby,
        GameConnectToLobby
    }
}