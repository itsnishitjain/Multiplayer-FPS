using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string Name;
    public int damage;
    public float firerate;
    public float weaponBloom;
    public float Recoil;
    public float KickBack;
    public float aimSpeed;
    public GameObject prefab;
}
