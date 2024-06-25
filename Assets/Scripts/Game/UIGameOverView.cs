using TMPro;
using UnityEngine;



	public class UIGameOverView : MonoBehaviour
	{
		public GameObject      Victory;
		public GameObject      Defeat;

        LifeHandler _lifeHandler;

        private void Update()
		{		

			// Unlock cursor.
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;      

   //        if (bool localPlayerIsWinner = false)
		 //  {
			//Defeat.SetActive(localPlayerIsWinner == true);


   //        }
		 //  else
   //        {
   //         Victory.SetActive(localPlayerIsWinner == true);
   //        }

        }


    }

