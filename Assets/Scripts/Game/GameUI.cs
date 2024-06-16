using Fusion;
using UnityEngine;

using UnityEngine.SceneManagement;


	public class GameUI : MonoBehaviour
	{
		public Gameplay       Gameplay;
		[HideInInspector]
		public NetworkRunner  Runner;

		public UIPlayerView   PlayerView;
		public UIGameplayView GameplayView;
		public UIGameOverView GameOverView;
		public GameObject     DisconnectedView;

		public void OnRunnerShutdown(NetworkRunner runner, ShutdownReason reason)
		{
			if (GameOverView.gameObject.activeSelf)
				return; // Regular shutdown - GameOver already active


			DisconnectedView.SetActive(true);
		}

	

		private void Awake()
		{
			PlayerView.gameObject.SetActive(false);
			DisconnectedView.SetActive(false);


			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		private void Update()
		{
			if (Application.isBatchMode == true)
				return;

			if (Gameplay.Object == null || Gameplay.Object.IsValid == false)
				return;

			Runner = Gameplay.Runner;

			//var keyboard = Keyboard.current;
			bool gameplayActive = Gameplay.State < EGameplayState.Finished;


			

			GameplayView.gameObject.SetActive(gameplayActive);
			GameOverView.gameObject.SetActive(gameplayActive == false);

			var playerObject = Runner.GetPlayerObject(Runner.LocalPlayer);
			if (playerObject != null)
			{
				//var player = playerObject.GetComponent<Player>();
				var playerData = Gameplay.PlayerData.Get(Runner.LocalPlayer);

				//PlayerView.UpdatePlayer(player, playerData);
				PlayerView.gameObject.SetActive(gameplayActive);
			}
			else
			{
				PlayerView.gameObject.SetActive(false);
			}
		}
	}

