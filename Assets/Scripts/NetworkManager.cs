using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour
{
    public string player_prefab;
    public Transform[] spawnPoints;

    public void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Transform t_spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);
    }
}
