using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Fusion;



	public struct PlayerDatass : INetworkStruct
	{
		public PlayerRef PlayerRef;
		public int       Kills;
		public int       Deaths;
		public int       LastKillTick;
		//public int       StatisticPosition;
		public bool      IsAlive;
		public bool      IsConnected;
        public string Nickname { get => default; set { } }

    }

    public enum EGameplayStates
	{
		Skirmish = 0,
		Running  = 1,
		Finished = 2,
	}


	public class GameLogic : NetworkBehaviour, IPlayerJoined, IPlayerLeft 
	{
		public GameUI GameUI;
        public EGameplayState State { get; set; }

	    [SerializeField] private NetworkPrefabRef playerprefab;
	//[Networked, Capacity(2)] private NetworkDictionary<PlayerRef, Player> Players => default;

    public NetworkDictionary<PlayerRef, PlayerData> PlayerData { get; }


        private List<Spawner> _spawnedPlayers = new(2);
		private List<PlayerRef> _pendingPlayers = new(2);
		private List<PlayerData> _tempPlayerData = new(2);
		private List<Transform> _recentSpawnPoints = new(1);

		public void PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef)
		{
			if (HasStateAuthority == false)
				return;

			// Update statistics of the killer player.
			if (PlayerData.TryGet(killerPlayerRef, out PlayerData killerData))
			{
				killerData.Kills++;
				PlayerData.Set(killerPlayerRef, killerData);
			}

			// Update statistics of the victim player.
			var playerData = PlayerData.Get(victimPlayerRef);
			playerData.Deaths++;
			playerData.IsAlive = false;
			PlayerData.Set(victimPlayerRef, playerData);

			

			//StartCoroutine(RespawnPlayer(victimPlayerRef, PlayerRespawnTime));

			RecalculateStatisticPositions();
		}

	

		public override void FixedUpdateNetwork()
		{
			if (HasStateAuthority == false)
				return;

			// PlayerManager is a special helper class which iterates over list of active players (NetworkRunner.ActivePlayers) and call spawn/despawn callbacks on demand.
			//PlayerManager.UpdatePlayerConnections(Runner, SpawnPlayer, DespawnPlayer);

			// Start gameplay when there are enough players connected.
			if (State == EGameplayState.Skirmish && PlayerData.Count > 1)
			{
				StartGameplay();
			}

			if (State == EGameplayState.Running)
			{
				//_runningStateTime += Runner.DeltaTime;

				var sessionInfo = Runner.SessionInfo;

				
			}
		}	

		private void StartGameplay()
		{
			// Stop all respawn coroutines.
			StopAllCoroutines();

			State = EGameplayState.Running;

			// Reset player data after skirmish and respawn players.
			foreach (var playerPair in PlayerData)
			{
				var data = playerPair.Value;

				data.Kills = 0;
				data.Deaths = 0;
				data.IsAlive = false;

				PlayerData.Set(data.PlayerRef, data);

				//StartCoroutine(RespawnPlayer(data.PlayerRef, 0f));
			}
		}

		private void StopGameplay()
		{
			RecalculateStatisticPositions();

			State = EGameplayState.Finished;
		}

		private void RecalculateStatisticPositions()
		{
			if (State == EGameplayState.Finished)
				return;

			_tempPlayerData.Clear();

			foreach (var pair in PlayerData)
			{
				_tempPlayerData.Add(pair.Value);
			}

			_tempPlayerData.Sort((a, b) =>
			{
				if (a.Kills != b.Kills)
					return b.Kills.CompareTo(a.Kills);

				return a.LastKillTick.CompareTo(b.LastKillTick);
			});

			for (int i = 0; i < _tempPlayerData.Count; i++)
			{
				var playerData = _tempPlayerData[i];

				PlayerData.Set(playerData.PlayerRef, playerData);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
		private void RPC_PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef)
		{
			string killerNickname = "";
			string victimNickname = "";

			if (PlayerData.TryGet(killerPlayerRef, out PlayerData killerData))
			{
				killerNickname = killerData.Nickname;
			}

			if (PlayerData.TryGet(victimPlayerRef, out PlayerData victimData))
			{
				victimNickname = victimData.Nickname;
			}

			//GameUI.GameplayView.KillFeed.ShowKill(killerNickname, victimNickname);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
		private void RPC_SetPlayerNickname(PlayerRef playerRef, string nickname)
		{
			var playerData = PlayerData.Get(playerRef);
			playerData.Nickname = nickname;
			PlayerData.Set(playerRef, playerData);
		}

    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
		{
            NetworkObject playerObject = Runner.Spawn(playerprefab, Vector3.zero, Quaternion.identity);
			//Players.Add(player, playerObject.GetComponent<Player>());
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        throw new System.NotImplementedException();
    }
}

