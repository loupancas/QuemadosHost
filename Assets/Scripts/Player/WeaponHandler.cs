using Fusion;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _ballPrefab;
    [SerializeField] private Transform _firingPositionTransform;
    [SerializeField] private ParticleSystem _shootingParticles;

    [Networked]
    NetworkBool _spawnedBall { get; set; }
    
    private ChangeDetector _changeDetector;
    
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(_spawnedBall):
                    RemoteParticles();
                    break;
            }
        }
    }

    public void Fire()
    {
        Runner.Spawn(_ballPrefab, _firingPositionTransform.position, transform.rotation);
        _spawnedBall = !_spawnedBall;
    }

    void RemoteParticles()
    {
        _shootingParticles.Play();
    }
}