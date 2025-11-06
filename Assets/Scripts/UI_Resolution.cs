using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resolution : MonoBehaviour
{
    public InputField front_width, front_height;
    public InputField left_width, left_height;
    public InputField right_width, right_height;
    public InputField bottom_width, bottom_height;
    public Toggle front, left, right, bottom;
    public void Start()
    {
        Settings.OnApply += Apply;
    }
    public void OnEnable()
    {
        front_width.text = Settings.MAIN.resolution.width.ToString();
        front_height.text = Settings.MAIN.resolution.height.ToString();

        left_width.text = Settings.LEFT.resolution.width.ToString();
        left_height.text = Settings.LEFT.resolution.height.ToString();
        
        right_width.text = Settings.RIGHT.resolution.width.ToString();
        right_height.text = Settings.RIGHT.resolution.height.ToString();
        
        bottom_width.text = Settings.FLOOR.resolution.width.ToString();
        bottom_height.text = Settings.FLOOR.resolution.height.ToString();
    }
    public void Apply()
    {
        int.TryParse(front_width.text, out int _frontWidth);
        int.TryParse(front_height.text, out int _frontHeight);

        int.TryParse(left_width.text, out int _leftWidth);
        int.TryParse(left_height.text, out int _leftHeight);

        int.TryParse(right_width.text, out int _rightWidth);
        int.TryParse(right_height.text, out int _rightHeight);

        int.TryParse(bottom_width.text, out int _bottomWidth);
        int.TryParse(bottom_height.text, out int _bottomHeight);

        Resolution _front = new Resolution() { width = _frontWidth, height = _frontHeight };
        Resolution _left = new Resolution() { width = _leftWidth, height = _leftHeight };
        Resolution _right = new Resolution() { width = _rightWidth, height = _rightHeight };
        Resolution _bottom = new Resolution() { width = _bottomWidth, height = _bottomHeight };

        Settings.MAIN.resolution = _front;
        Settings.LEFT.resolution = _right;
        Settings.RIGHT.resolution = _right;
        Settings.FLOOR.resolution = _bottom;

        Settings.MAIN.state = front.isOn;
        Settings.LEFT.state = left.isOn;
        Settings.RIGHT.state = right.isOn;
        Settings.FLOOR.state = bottom.isOn;
    }
}
