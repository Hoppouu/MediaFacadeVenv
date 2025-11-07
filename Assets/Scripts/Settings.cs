using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public static class Settings
{
    public static ScreenSetting MAIN { get; private set; }
    public static ScreenSetting LEFT { get; private set; }
    public static ScreenSetting RIGHT { get; private set; }
    public static ScreenSetting FLOOR { get; private set; }

    public static int PlayerHeight { get; private set; } = 170;
    public static int PlayerFOV { get; private set; } = 60;
    public static event Action OnVideoApply, OnApply;
    public static event Action OnFOV;

    static Settings()
    {
        MAIN = CreateScreenSetting(3738, 1000);
        LEFT = CreateScreenSetting(2348, 1000);
        RIGHT = CreateScreenSetting(2348, 1000);
        FLOOR = CreateScreenSetting(3738, 2348);
    }

    private static ScreenSetting CreateScreenSetting(int width_, int height_)
    {
        ScreenSetting temp = new ScreenSetting();
        temp.resolution = new Resolution() { width = width_, height = height_ };
        temp.videoURL = "";
        temp.stateScreen = true;

        return temp;
    }

    public static void SetPlayerHeight(int height) { PlayerHeight = height; }
    public static void SetPlayerFOV(int fov) { PlayerFOV = fov; }
    public static void ApplyChanges() { OnApply.Invoke(); }
    public static void ApplyVideos() { OnVideoApply.Invoke(); }
    public static void FOVApply() { OnFOV.Invoke(); }

}
public class ScreenSetting
{
    public Resolution resolution;
    public string videoURL;
    public bool stateScreen;
    public bool stateNDI;
    public string ndiName;
}