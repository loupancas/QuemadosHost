using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerView : MonoBehaviour
{
		public UIHealth Health;
		
	    public TextMeshProUGUI Nickname;

	     private Transform _local;
        
    [SerializeField] Image _lifeBarImage;


    public void Init(NetworkPlayer local)
    { 
	  _local = local.gameObject.transform;
	  Nickname=GetComponentInChildren<TextMeshProUGUI>();
    }

	public void UpdateNickName(string playerNickname)
    {
        Nickname.text = playerNickname;

    }

	public void UpdateHealth(float life)
    {
        _lifeBarImage.fillAmount = life;
    }

 //   public void UpdatePlayer(LifeHandler life, PlayerData playerData, Ball ball)
	//{

	//   Health.UpdateHealth(life); 

	//   //Ball.UpdateBallOwner(ball);

 //      Nickname.text = playerData.Nickname;

 //   }
}

