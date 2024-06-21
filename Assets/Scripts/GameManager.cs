using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance=null;
    byte[] connectionToken;
    public Vector2 CameraViewRotation = Vector2.zero;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //if (connectionToken == null)
        //{
        //    connectionToken = ConnectionTokenUtils.NewToken();

        //}
    }

    public void SetConnectionToken(byte[] connectiontoken)
    {
        this.connectionToken = connectiontoken;
    }

    public byte[] GetConnectionToken()
    {
        return connectionToken;
    }


}
