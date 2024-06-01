using Fusion;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _firingPositionTransform;
    [SerializeField] private ParticleSystem _shootingParticles;

    [Networked]
    NetworkBool _spawnedBullet { get; set; }
    
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
                case nameof(_spawnedBullet):
                    RemoteParticles();
                    break;
            }
        }
    }

    public void Fire()
    {
        Runner.Spawn(_bulletPrefab, _firingPositionTransform.position, transform.rotation);
        _spawnedBullet = !_spawnedBullet;
    }

    void RemoteParticles()
    {
        _shootingParticles.Play();
    }
}