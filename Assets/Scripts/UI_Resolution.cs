using Klak.Ndi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resolution : MonoBehaviour
{
    public InputField front_width, front_height;
    public InputField left_width, left_height;
    public InputField right_width, right_height;
    public InputField bottom_width, bottom_height;
    public Toggle front, left, right, bottom;
    public Toggle ndi;
    public Text ndi_text;
    public Dropdown[] NDI_dropdown;

    private int[] ndi_dropdown_idx = { 0 };

    public void Awake()
    {
        ndi_dropdown_idx = new int[NDI_dropdown.Length];
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

        SetStateNDI(ndi.isOn);
        StartCoroutine(LoadNdiSources(1.0f));
    }
    public void OnClickApply()
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
        Settings.LEFT.resolution = _left;
        Settings.RIGHT.resolution = _right;
        Settings.FLOOR.resolution = _bottom;

        Settings.MAIN.stateScreen = front.isOn;
        Settings.LEFT.stateScreen = left.isOn;
        Settings.RIGHT.stateScreen = right.isOn;
        Settings.FLOOR.stateScreen = bottom.isOn;

        for (int i = 0; i < NDI_dropdown.Length; i++)
        {
            ndi_dropdown_idx[i] = NDI_dropdown[i].value;
        }


        Settings.MAIN.ndiName = NDI_dropdown[0].options[ndi_dropdown_idx[0]].text;
        Settings.LEFT.ndiName = NDI_dropdown[1].options[ndi_dropdown_idx[1]].text;
        Settings.RIGHT.ndiName = NDI_dropdown[2].options[ndi_dropdown_idx[2]].text;
        Settings.FLOOR.ndiName = NDI_dropdown[3].options[ndi_dropdown_idx[3]].text;
    }

    public void NDIToggle()
    {
        Settings.MAIN.stateNDI = ndi.isOn;
        Settings.LEFT.stateNDI = ndi.isOn;
        Settings.RIGHT.stateNDI = ndi.isOn;
        Settings.FLOOR.stateNDI = ndi.isOn;

        SetStateNDI(ndi.isOn);
    }

    public void SetStateNDI(bool state)
    {
        if (state)  ndi_text.text = "NDI ON";
        else        ndi_text.text = "NDI OFF";

        foreach (var dropdown in NDI_dropdown)
        {
            dropdown.interactable = state;
        }
    }
    private IEnumerator LoadNdiSources(float interval)
    {
        while(true)
        {
            List<string> names = NdiFinder.sourceNames.ToList();
            names.Insert(0, "None");
            for (int i = 0; i < NDI_dropdown.Length; i++)
            {
                NDI_dropdown[i].ClearOptions();
                NDI_dropdown[i].AddOptions(names);

                if (NDI_dropdown[i].options.Count <= ndi_dropdown_idx[i])
                {
                    ndi_dropdown_idx[i] = 0;
                }
                NDI_dropdown[i].value = ndi_dropdown_idx[i];

                if (names.Count == 0)
                {
                    NDI_dropdown[i].AddOptions(new List<string> { "NDI 소스 없음" });
                }
            }
            yield return new WaitForSeconds(interval);
        }

    }
}
