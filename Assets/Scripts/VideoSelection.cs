using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VideoSelection : MonoBehaviour
{
    public RawImage frontImage, leftImage, rightImage, floorImage;
    public void SetFrontVideo()
    {
        string filePath = EditorUtility.OpenFilePanel("Select a file", "", "mp4");
        Settings.MAIN.videoURL = filePath;
        VideoThumbnailUtility.GenerateThumbnail(filePath, frontImage, this);
    }
    public void SetLeftVideo()
    {
        string filePath = EditorUtility.OpenFilePanel("Select a file", "", "mp4");
        Settings.LEFT.videoURL = filePath;
        VideoThumbnailUtility.GenerateThumbnail(filePath, leftImage, this);
    }
    public void SetRightVideo()
    {
        string filePath = EditorUtility.OpenFilePanel("Select a file", "", "mp4");
        Settings.RIGHT.videoURL = filePath;
        VideoThumbnailUtility.GenerateThumbnail(filePath, rightImage, this);
    }
    public void SetFloorVideo()
    {
        string filePath = EditorUtility.OpenFilePanel("Select a file", "", "mp4");
        Settings.FLOOR.videoURL = filePath;
        VideoThumbnailUtility.GenerateThumbnail(filePath, floorImage, this);
    }
}
