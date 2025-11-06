using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerAction playerAction;
    bool stop = false;
    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        Cursor.lockState = CursorLockMode.Locked;
        UIManager.OnPlay += setPlay;
        UIManager.OnStop += setStop;

    }
    private void setPlay()
    {
        stop = false;
    }

    private void setStop()
    {
        stop = true;
    }
    private void Update()
    {
        if(stop)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        playerAction.CameraMove(mouseX, mouseY);

        KeyboardEvent();
    }

    void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        playerAction.Move(xInput, zInput);

        FixedKeyboardEvent();
    }

    private void FixedKeyboardEvent()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerAction.Fly();
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            playerAction.Fall();
        }
    }
    private void KeyboardEvent()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAction.LetsRun();
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerAction.LetsWalk();
        }
    }
}
