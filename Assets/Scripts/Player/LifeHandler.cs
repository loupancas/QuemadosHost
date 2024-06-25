using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LifeHandler : NetworkBehaviour
{
    [SerializeField] private GameObject _playerVisual;
    [Networked, OnChangedRender(nameof(OnLifeChanged))]
    private byte _currentLife { get; set; }
    Ball Ball;
    public const byte MAX_LIFE = 10;
    private const byte MAX_DEADS = 5;

    private byte _currentDeads = 0;

    //UIHealth _uiHealth;

    [Networked] 
    NetworkBool IsDead { get; set; }
    
    private ChangeDetector _changeDetector;
    
    public event Action<bool> OnDeadChange = delegate {  };
    public event Action<float> OnLifeUpdate = delegate { };
    public event Action OnRespawn = delegate {  };
    public event Action OnDespawn = delegate {  };

    private NickNameBarLifeItem _myItemUI;


    //void Awake()
    //{
    //    _currentLife = MAX_LIFE;
    //}

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        _currentLife = MAX_LIFE;
    }

    public void GetMyUI(NickNameBarLifeItem item) => _myItemUI = item;


    //Al morir, si es mi primera vez, revivir a los 2 segundos
    //Si es mi segunda vez, desconectar al jugador
    public void TakeDamage(byte dmg)
    {
        if (dmg > _currentLife) dmg = _currentLife;

        _currentLife -= dmg;

        if (_currentLife == 0)
        {
            _currentDeads++;
            if (_currentDeads == MAX_DEADS)
            {
                DisconnectPlayer();
            }
            else
            {
                StartCoroutine(Server_RespawnCooldown());
                IsDead = true;
            }
        }

        //_uiHealth.UpdateHealth(this);
    }

    //public void Health(byte currentLife)
    //{
    //    _currentLife = currentLife;
    //    //_uiHealth.UpdateHealth(this);

    //}

    IEnumerator Server_RespawnCooldown()
    {
        yield return new WaitForSeconds(2f);

        Server_Revive();
    }

    void Server_Revive()
    {
        OnRespawn();
        IsDead = false;
        _currentLife = MAX_LIFE;
        //_uiHealth.UpdateHealth(this);

    }

    void DisconnectPlayer()
    {   
        if (!Object.HasInputAuthority)
            Runner.Disconnect(Object.InputAuthority);
        
        Runner.Despawn(Object);
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(IsDead):
                    OnDeadChanged();
                    break;
            }
        }
    }
    
    void OnDeadChanged()
    {
        if (IsDead)
        {
            Remote_Dead();
        }
        else
        {
            Remote_Respawn();
        }

        OnDeadChange(IsDead);
    }
    
    void Remote_Dead()
    {
        _playerVisual.SetActive(false);
    }

    void Remote_Respawn()
    {
        _playerVisual.SetActive(true);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawn();
    }

    void OnLifeChanged()
    {
        _myItemUI.UpdateLifeBar(_currentLife / MAX_LIFE);
    }
}
