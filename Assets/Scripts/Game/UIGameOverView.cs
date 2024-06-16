using TMPro;
using UnityEngine;



	public class UIGameOverView : MonoBehaviour
	{
		public TextMeshProUGUI Winner;
		public GameObject      Victory;
		public GameObject      Defeat;

		private GameUI _gameUI;
		private EGameplayState _lastState;

		

		private void Awake()
		{
			_gameUI = GetComponentInParent<GameUI>();
		}

		private void Update()
		{
			if (_gameUI.Runner == null)
				return;

			// Unlock cursor.
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			if (_gameUI.Gameplay.Object == null || _gameUI.Gameplay.Object.IsValid == false)
				return;

			if (_lastState == _gameUI.Gameplay.State)
				return;


			_lastState = _gameUI.Gameplay.State;

			bool localPlayerIsWinner = false;
			Winner.text = string.Empty;

			foreach (var playerPair in _gameUI.Gameplay.PlayerData)
			{
				//if (playerPair.Value.StatisticPosition != 1)
				//	continue;

				Winner.text = $"Winner is {playerPair.Value.Nickname}";
				localPlayerIsWinner = playerPair.Key == _gameUI.Runner.LocalPlayer;
			}

			Victory.SetActive(localPlayerIsWinner);
			Defeat.SetActive(localPlayerIsWinner == false);
		}
	}

