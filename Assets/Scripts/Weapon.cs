using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviourPunCallbacks
{
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;
    public GameObject MuzzleFlash;
    public Transform FirePoint;
    public Transform FirePoint_aimed;
   
    private int CurrentIndex;
    private GameObject currentWeapon;

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) return;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            photonView.RPC("Equip", RpcTarget.All, 0);
        }
        
        if(currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));
            if(Input.GetMouseButtonDown(0))
            {
                photonView.RPC("Shoot", RpcTarget.All);
            }
        } 
    }

    [PunRPC]
    void Equip (int Index)
    {
        if(currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        CurrentIndex = Index;

        GameObject newWeapon = Instantiate (loadout[Index].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = newWeapon;
    }

    void Aim (bool isAiming)
    {
        if(Input.GetMouseButtonDown(0) && isAiming)
        {
            GameObject weaponMuzzle = Instantiate(MuzzleFlash, new Vector3(FirePoint_aimed.transform.position.x, FirePoint_aimed.transform.position. y, FirePoint_aimed.transform.position.z), Quaternion.identity);    
            Destroy(weaponMuzzle, 1f);               
        }
        if(Input.GetMouseButtonDown(0) && !isAiming)
        {   
            GameObject weaponMuzzle_aimed = Instantiate(MuzzleFlash, new Vector3(FirePoint.transform.position.x, FirePoint.transform.position. y, FirePoint.transform.position.z), Quaternion.identity);
            Destroy(weaponMuzzle_aimed, 1f);
            
        }
        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform Aimed = currentWeapon.transform.Find("States/Aimed");
        Transform NotAimed = currentWeapon.transform.Find("States/NotAimed");
        
        if(isAiming)
        {
            anchor.position = Vector3.Lerp(anchor.position, Aimed.position, Time.deltaTime * loadout[CurrentIndex].aimSpeed);
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, NotAimed.position,Time.deltaTime * loadout[CurrentIndex].aimSpeed);            
        }
    }
    [PunRPC]
    void Shoot ()
    {
        Transform Spawn = transform.Find("Cameras/Camera");

        Vector3 bloomPos = Spawn.position + Spawn.forward * 1000f;
        bloomPos += Random.Range(-loadout[CurrentIndex].weaponBloom, loadout[CurrentIndex].weaponBloom) * Spawn.up;
        bloomPos += Random.Range(-loadout[CurrentIndex].weaponBloom, loadout[CurrentIndex].weaponBloom) * Spawn.right;
        bloomPos -= Spawn.position;
        bloomPos.Normalize();       
        RaycastHit Hit = new RaycastHit();
        if(Physics.Raycast(Spawn.position, bloomPos, out Hit, 1000f, canBeShot))
        {
            GameObject newHole = Instantiate(bulletHolePrefab, Hit.point + Hit.normal * 0.001f, Quaternion.identity) as GameObject;
            newHole.transform.LookAt(Hit.point + Hit.normal);
            Destroy(newHole, 5f);

            if(photonView.IsMine)
            {
                if(Hit.collider.gameObject.layer == 11)
                {
                    Hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[CurrentIndex].damage);
                }
            }
        }
    }

    [PunRPC]
    private void TakeDamage(int p_damage)
    {
        GetComponent<Motion>().TakeDamage(p_damage);
    }
}
