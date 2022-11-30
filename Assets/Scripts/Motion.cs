using UnityEngine;
using Photon.Pun;

public class Motion : MonoBehaviourPunCallbacks
{
    public float speed;
    public float sprintModifier;
    public Camera normalCam;
    public GameObject cameraParent;
    public Transform weaponParent;
    private int current_Health;
    public int max_Health;

    private NetworkManager manager;
    private Transform ui_HealthBar;
    private Rigidbody rb;
    private Vector3 TargetWeaponBobPosition;
    private Vector3 weaponParentOrigin;

    private float baseFOV;
    private float sprintFOVModifier = 1.5f;
    private float movementCounter;
    private float idleCounter;

    private void Start ()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        current_Health = max_Health;
        cameraParent.SetActive(photonView.IsMine);

        if(!photonView.IsMine)
        {
            gameObject.layer = 11;
        }

        if(Camera.main) Camera.main.enabled = false; 
        baseFOV = normalCam.fieldOfView;
        rb = GetComponent<Rigidbody>();
        weaponParentOrigin = weaponParent.localPosition;
        if(photonView.IsMine)
        {
            ui_HealthBar = GameObject.Find("HUD/Health/Bar").transform;
            RefreshHealthBar();
        }    
    }


    void FixedUpdate ()
    {
        if(!photonView.IsMine) return;
        //axies
        float hMove = Input.GetAxisRaw("Horizontal");
        float vMove = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isSprinting = sprint && vMove > 0 ;
        Vector3 direction = new Vector3(hMove, 0, vMove);
        direction.Normalize();

        float adjustedSpeed = speed;


        if(isSprinting)
        {
            adjustedSpeed *= sprintModifier;
        }

        Vector3 TargetVelocity = transform.TransformDirection(direction) * adjustedSpeed * Time.fixedDeltaTime;
        TargetVelocity.y = rb.velocity.y;
        rb.velocity = TargetVelocity;

        if(isSprinting)
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        }
        else
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }

        if(hMove == 0 && vMove == 0)
        {
            HeadBob(idleCounter, 0.0285f, 0.0285f);
            idleCounter += Time.deltaTime * 2f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, TargetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if(!isSprinting)
        {
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 4f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, TargetWeaponBobPosition, Time.deltaTime * 6f);
        }
        else 
        {
            HeadBob(movementCounter, 0.1f, 0.07f);
            movementCounter += Time.deltaTime * 5f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, TargetWeaponBobPosition, Time.deltaTime * 10f);
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(10);
        }
    }

    void HeadBob (float z, float xIntensity, float yIntensity)
    {
        TargetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity, Mathf.Sin(z * 2)* yIntensity, 0);
    }

    void RefreshHealthBar ()
    {
        float Health_ratio = (float)current_Health/(float)max_Health;
        ui_HealthBar.localScale = new Vector3(Health_ratio, 1, 1);
    }

    public void TakeDamage(int p_damage)
    {
        if(photonView.IsMine)
        {
            current_Health -= p_damage;
            RefreshHealthBar();
            if(current_Health <= 0)
            {
                manager.Spawn();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
