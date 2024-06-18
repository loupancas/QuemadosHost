using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIBall : MonoBehaviour
{
	    public Image BallIcon;

	    private Ball _ball;

        public TextMeshProUGUI Name;
    public void UpdateBallOwner(Ball ball)
	{
        if (_ball == null)
            return;


        // Modify UI text only when value changed.  
        //if (ball._BallOwned == _ball._BallOwned)
        //    return;
        //BallIcon.sprite = ball.Icon;
        //Name.text = ball.Name;
        //_ball._BallOwned = ball._BallOwned;




    }

    public void SetBall(Ball ball)
    {
        if (ball == null)
            return;
        _ball = ball;

        BallIcon.sprite = ball.Icon;
        Name.text = ball.Name;
    }
}