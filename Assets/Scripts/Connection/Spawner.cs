using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections;


public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private List<Transform> _recentSpawnPoints = new List<Transform>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
       var spawnPoint = PlayerSpawnPoint();
        if (runner.IsServer)
        {
            runner.Spawn(_playerPrefab, spawnPoint.position, null, player);
        }
    }

    private CharacterInputHandler _characterInputHandler;
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        _characterInputHandler ??= NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if (_characterInputHandler != null)
        {
            input.Set(_characterInputHandler.GetLocalInputs());
        }
    }
    
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        runner.Shutdown();
    }
    
    private Transform PlayerSpawnPoint()
    {
        Transform spawnPoint = null;
        var spawnPoints = FindObjectsOfType<SpawnPoint>();

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found!");
            return null;
        }

        int offset = UnityEngine.Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoint = spawnPoints[(offset + i) % spawnPoints.Length].transform;

            if (!_recentSpawnPoints.Contains(spawnPoint))
                break;
        }

        _recentSpawnPoints.Add(spawnPoint);

        if (_recentSpawnPoints.Count > 2)
        {
            _recentSpawnPoints.RemoveAt(0);
        }

        return spawnPoint;



    }




    //private void SpawnPlayer(PlayerRef playerRef)
    //{
    //    if (PlayerData.TryGet(playerRef, out var playerData) == false)
    //    {
    //        playerData = new PlayerData();
    //        playerData.PlayerRef = playerRef;
    //        playerData.Nickname = playerRef.ToString();

    //        playerData.IsAlive = false;
    //        playerData.IsConnected = false;
    //    }

    //    if (playerData.IsConnected == true)
    //        return;

    //    Debug.LogWarning($"{playerRef} connected.");

    //    playerData.IsConnected = true;
    //    playerData.IsAlive = true;

    //    PlayerData.Set(playerRef, playerData);

    //    var spawnPoint = GetSpawnPoint();
    //    var player = Runner.Spawn(PlayerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

    //    // Set player instance as PlayerObject so we can easily get it from other locations.
    //    Runner.SetPlayerObject(playerRef, player.Object);

    //    RecalculateStatisticPositions();
    //}

    //private void DespawnPlayer(PlayerRef playerRef, Player player)
    //{
    //    if (PlayerData.TryGet(playerRef, out var playerData) == true)
    //    {
    //        if (playerData.IsConnected == true)
    //        {
    //            Debug.LogWarning($"{playerRef} disconnected.");
    //        }

    //        playerData.IsConnected = false;
    //        playerData.IsAlive = false;
    //        PlayerData.Set(playerRef, playerData);
    //    }

    //    Runner.Despawn(player.Object);

    //    RecalculateStatisticPositions();
    //}

    //private IEnumerator RespawnPlayer(PlayerRef playerRef, float delay)
    //{
    //    if (delay > 0f)
    //        yield return new WaitForSecondsRealtime(delay);

    //    if (Runner == null)
    //        yield break;

    //    // Despawn old player object if it exists.
    //    var playerObject = Runner.GetPlayerObject(playerRef);
    //    if (playerObject != null)
    //    {
    //        Runner.Despawn(playerObject);
    //    }

    //    // Don't spawn the player for disconnected clients.
    //    if (PlayerData.TryGet(playerRef, out PlayerData playerData) == false || playerData.IsConnected == false)
    //        yield break;

    //    // Update player data.
    //    playerData.IsAlive = true;
    //    PlayerData.Set(playerRef, playerData);

    //    var spawnPoint = GetSpawnPoint();
    //    var player = Runner.Spawn(PlayerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

    //    // Set player instance as PlayerObject so we can easily get it from other locations.
    //    Runner.SetPlayerObject(playerRef, player.Object);
    //}





    #region Unused callbacks

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

    #endregion
}
