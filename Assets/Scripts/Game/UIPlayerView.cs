using TMPro;
using UnityEngine;


	public class UIPlayerView : MonoBehaviour
	{
		public UIHealth Health;
		public UIBall Ball;
	    public TextMeshProUGUI Nickname;

    public void UpdatePlayer(LifeHandler life, PlayerData playerData, Ball ball)
	{

	   Health.UpdateHealth(life); 

	   Ball.UpdateBallOwner(ball);

       Nickname.text = playerData.Nickname;

    }
}

