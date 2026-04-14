using UnityEngine;
using Unity.Netcode;

public class NetworkProjectile : NetworkBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifeTime = 3f;
   
    public override void OnNetworkSpawn()
    {
        // Only the server should despawn network objects 
        if (IsServer)
            Invoke(nameof(Despawn), lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only the server should decide when the projectile disappears
        if (!IsServer) return;

        var health = collision.collider.GetComponentInParent<NetworkHealth>();
        if (health != null)
        {
            health.TakeDamageServerRpc(damage);
        }
            Despawn();
    }

    private void Despawn()
    {
        // Despawn removes it across the network
        if(NetworkObject && NetworkObject.IsSpawned)
            NetworkObject.Despawn();
    }
}
