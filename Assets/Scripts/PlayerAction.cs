using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.PlayerSettings;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody player;
    public Camera mainCamera;
    public GameObject TopRayPosition, BottomRayPosition;

    private float moveSpeed = 7f;
    private float mouseSpeed = 2.0f;
    private Vector3 flySpeed = new Vector3(0, 10, 0);

    private float yRotation = 0f;

    private readonly float runSpeed = 21f;
    private readonly float walkSpeed = 7f;
    private readonly Vector3 runFlySpeed = new Vector3(0, 20, 0);
    private readonly Vector3 walkFlySpeed = new Vector3(0, 10, 0);

    void Start()
    {
        player = GetComponent<Rigidbody>();
        player.transform.localScale = new Vector3(1, Settings.PlayerHeight / 100f, 1);
        player.transform.position = new Vector3(player.transform.position.x, Settings.PlayerHeight / 100f, player.transform.position.z);

        Settings.OnApply += PlayerSetting;
        Settings.OnFOV += () => { mainCamera.fieldOfView = Settings.PlayerFOV; };
    }

    public void Move(float xInput, float zInput)
    {
        Vector3 inputVector = new Vector3(xInput, 0, zInput);
        player.linearVelocity = Vector3.zero;

        if (inputVector.sqrMagnitude < 0.1f) return;

        Vector3 moveDirection = transform.rotation * inputVector;
        Vector3 movement = moveDirection * moveSpeed;
        player.MovePosition(player.position + movement * Time.deltaTime);
    }

    public void CameraMove(float mouseX, float mouseY)
    {
        transform.Rotate(Vector3.up * mouseX * mouseSpeed);
        yRotation -= mouseY * mouseSpeed;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
    }

    public void Fly()
    {
        Vector3 move = flySpeed * Time.fixedDeltaTime;
        if(!CheckHitWall(TopRayPosition.transform.position, transform.up, move.y))
        {
            player.MovePosition(player.position + move);
        }
    }

    public void Fall()
    {
        Vector3 move = flySpeed * Time.fixedDeltaTime;
        if (!CheckHitWall(BottomRayPosition.transform.position, -transform.up, move.y))
        {
            player.MovePosition(player.position - move);
        }
    }

    public void LetsWalk()
    {
        moveSpeed = walkSpeed;
        flySpeed = walkFlySpeed;
    }
    public void LetsRun()
    {
        moveSpeed = runSpeed;
        flySpeed = runFlySpeed;
    }

    private bool CheckHitWall(Vector3 start, Vector3 dist, float magnitude)
    {
        if (Physics.Raycast(start, dist, out RaycastHit hit, magnitude))
        {
            if(hit.collider)
            {
                return true;
            }
        }
        return false;
    }

    private void PlayerSetting()
    {
        player.transform.localScale = new Vector3(1, Settings.PlayerHeight / 100f, 1);
        player.transform.position = new Vector3(player.position.x, Settings.PlayerHeight / 100f, player.transform.position.z);
        mainCamera.fieldOfView = Settings.PlayerFOV;
    }
}
