using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static VideoManager;

public class VideoManager : MonoBehaviour
{
    public VideoScreens screens;

    void Start()
    {
        Settings.OnVideoApply += SetVideo;
        Settings.OnApply += SetSetting;
        UIManager.OnPlay += PlayVideo;
        UIManager.OnStop += PauseVideo;

        InitScreens();
        SetVideo();
        SetSetting();
    }

    private void InitScreens()
    {
        InitScreen(ref screens.main);
        InitScreen(ref screens.left);
        InitScreen(ref screens.right);
        InitScreen(ref screens.floor);
    }
    private void InitScreen(ref VideoScreen vs)
    {
        vs.videoPlayer = vs.screen.GetComponent<VideoPlayer>();
        vs.meshRenderer = vs.screen.GetComponent<MeshRenderer>();
        vs.boxCollider = vs.screen.GetComponent<BoxCollider>();
        vs.videoPlayer.prepareCompleted += OnPrepareCompleted;
        vs.videoPlayer.playOnAwake = false;
        vs.videoPlayer.waitForFirstFrame = true;
        vs.videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
    }

    private void SetVideo()
    {
        SetVideoFunc(ref screens.main, Settings.MAIN);
        SetVideoFunc(ref screens.left, Settings.LEFT);
        SetVideoFunc(ref screens.right, Settings.RIGHT);
        SetVideoFunc(ref screens.floor, Settings.FLOOR);
    }
    private void SetSetting()
    {
        SetSettingFunc(ref screens.main, Settings.MAIN);
        SetSettingFunc(ref screens.left, Settings.LEFT);
        SetSettingFunc(ref screens.right, Settings.RIGHT);
        SetSettingFunc(ref screens.floor, Settings.FLOOR);
        SetPositionFunc();
    }

    private void SetVideoFunc(ref VideoScreen videoScreen, ScreenSetting setting)
    {
        if (videoScreen.videoPlayer.isPlaying)
        {
            videoScreen.videoPlayer.Pause();
        }

        if (setting.videoURL != "")
        {
            videoScreen.videoPlayer.url = setting.videoURL;
            videoScreen.videoPlayer.Prepare();
        }
    }

    private void SetSettingFunc(ref VideoScreen videoScreen, ScreenSetting setting)
    {
        videoScreen.resolution = setting.resolution;
        videoScreen.screen.transform.localScale = new Vector3(videoScreen.resolution.width / 1000f, 1, videoScreen.resolution.height / 1000f);
        videoScreen.meshRenderer.enabled = setting.state;
        videoScreen.boxCollider.enabled = setting.state;
        if (setting.state)  videoScreen.videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        else                videoScreen.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    }

    private void SetPositionFunc()
    {
        float leftX = screens.main.screen.transform.localScale.x * -5;
        float leftY = 5f;
        float leftZ = screens.main.screen.transform.position.z + screens.left.screen.transform.localScale.x * -5;

        float rightX = screens.main.screen.transform.localScale.x * 5;
        float rightY = 5f;
        float rightZ = screens.main.screen.transform.position.z + screens.right.screen.transform.localScale.x * -5;

        float floorX = screens.main.screen.transform.position.x;
        float floorY = 0.001f;
        float floorZ = screens.main.screen.transform.position.z + screens.floor.screen.transform.localScale.z * -5;

        screens.left.screen.transform.position = new Vector3(leftX, leftY, leftZ);
        screens.right.screen.transform.position = new Vector3(rightX, rightY, rightZ);
        screens.floor.screen.transform.position = new Vector3(floorX, floorY, floorZ);
    }

    private void PlayVideo()
    {
        screens.main.videoPlayer.Play();
        screens.left.videoPlayer.Play();
        screens.right.videoPlayer.Play();
        screens.floor.videoPlayer.Play();
    }

    private void PauseVideo()
    {
        screens.main.videoPlayer.Pause();
        screens.left.videoPlayer.Pause();
        screens.right.videoPlayer.Pause();
        screens.floor.videoPlayer.Pause();
    }

    private int i = 0;
    private void OnPrepareCompleted(VideoPlayer vp)
    {
        //StartCoroutine(CaptureFirstFrame(vp));
        i++;
        if (i == 4)
        {
            PlayVideo();
            i = 0;
        }
    }

    [System.Serializable]
    public struct VideoScreens
    {
        public VideoScreen main, left, right, floor;
    };

    [System.Serializable]
    public struct VideoScreen
    {
        public GameObject screen;
        [HideInInspector] public Resolution resolution;
        [HideInInspector] public VideoPlayer videoPlayer;
        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public BoxCollider boxCollider;
    };

    [System.Serializable]
    public struct Clips
    {
        public VideoClip main, left, right, floor;
    };
}

public class VideoThumbnailUtility : MonoBehaviour
{
    // --- 이 함수만 호출하면 됩니다 ---
    /// <summary>
    /// 비디오 클립의 중간 지점 썸네일을 RawImage에 표시합니다.
    /// </summary>
    /// <param name="videoClip">썸네일을 가져올 비디오 클립</param>
    /// <param name="targetImage">썸네일을 표시할 RawImage</param>
    /// <param name="owner">코루틴을 실행할 MonoBehaviour (보통 this)</param>
    public static void GenerateThumbnail(string videoUrl, RawImage targetImage, MonoBehaviour owner)
    {
        owner.StartCoroutine(CreateThumbnailCoroutine(videoUrl, targetImage));
    }

    private static IEnumerator CreateThumbnailCoroutine(string videoSource, RawImage targetImage)
    {
        GameObject tempPlayerObject = new GameObject("TempVideoPlayer");
        VideoPlayer videoPlayer = tempPlayerObject.AddComponent<VideoPlayer>();

        RenderTexture renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.url = videoSource;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        bool seekDone = false;
        videoPlayer.seekCompleted += (source) =>
        {
            seekDone = true;
        };

        videoPlayer.Pause();

        videoPlayer.time = videoPlayer.length / 2;

        yield return new WaitUntil(() => seekDone);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        Texture2D thumbnail = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        thumbnail.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        thumbnail.Apply();
        RenderTexture.active = null;

        targetImage.texture = thumbnail;

        renderTexture.Release();
        Destroy(tempPlayerObject);
    }
}