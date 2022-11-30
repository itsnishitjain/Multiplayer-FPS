using UnityEngine;
using Photon.Pun;

public class look : MonoBehaviourPunCallbacks
{
    public static bool cursorLocked = true;
    public Transform Player;
    public Transform cams;
    public Transform Weapon;

    public float xSensitivity;
    public float ySensitivity;
    public float maxAngle;
    private Quaternion camCenter;

    void Start ()
    {
        camCenter = cams.localRotation;
    }

    void Update()
    {
        if(!photonView.IsMine) return;
        SetY();
        SetX();
        UpdateCurserLocked();
    }

    void SetY ()
    {
        float input = Input.GetAxis("Mouse Y") * ySensitivity;
        Quaternion Rotate = Quaternion.AngleAxis(input, -Vector3.right);
        Quaternion delta = cams.localRotation * Rotate;

        if (Quaternion.Angle(camCenter,delta) < maxAngle)
        {
            cams.localRotation = delta;
        }
        Weapon.rotation = cams.rotation;
    }

    void SetX ()
    {
        float input = Input.GetAxis("Mouse X") * xSensitivity;
        Quaternion Rotate = Quaternion.AngleAxis(input, Vector3.up);
        Quaternion delta = Player.localRotation * Rotate;        
        Player.localRotation = delta;
        
    }

    void UpdateCurserLocked ()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;
            }
        }
    }
}
