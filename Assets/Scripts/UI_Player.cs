using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    public InputField playerHeight;
    public InputField playerFOV;
    public Slider playerFOVSlider;
    public void Start()
    {
        Settings.OnApply += Apply;
    }
    public void OnEnable()
    {
        playerHeight.text = Settings.PlayerHeight.ToString();
        playerFOVSlider.value = Settings.PlayerFOV;
        playerFOV.text = playerFOVSlider.value.ToString();
    }
    public void Apply()
    {
        int.TryParse(playerHeight.text, out int _playerHeight);
        Settings.SetPlayerHeight(_playerHeight);
        Settings.SetPlayerFOV((int)playerFOVSlider.value);
    }

    public void SliderApply()
    {
        playerFOV.text = playerFOVSlider.value.ToString();
        Settings.SetPlayerFOV((int)playerFOVSlider.value);
        Settings.FOVApply();
    }
}
