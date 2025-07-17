using UnityEngine;
using System.IO;
using System.Collections;

public class ScreenshotWithLogo : MonoBehaviour
{
    public Camera vrCamera; // MainCamera 또는 CenterEyeAnchor
    public string logoResourcePath = "logo"; // Resources/logo.png

    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();

        int width = 1920;
        int height = 1080;

        // 스크린샷 생성
        RenderTexture rt = new RenderTexture(width, height, 24);
        vrCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        vrCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();
        vrCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 로고 불러오기
        Texture2D logo = Resources.Load<Texture2D>(logoResourcePath);
        if (logo != null)
        {
            // 로고 합성 위치 계산 (우측 하단)
            int logoWidth = logo.width;
            int logoHeight = logo.height;
            int xStart = width - logoWidth - 10;
            int yStart = 10;

            // 픽셀 복사
            Color[] logoPixels = logo.GetPixels();
            screenshot.SetPixels(xStart, yStart, logoWidth, logoHeight, logoPixels);
            screenshot.Apply();
        }
        else
        {
            Debug.LogWarning("로고 이미지를 Resources/" + logoResourcePath + ".png 에 넣어주세요.");
        }

        // 저장
        byte[] bytes = screenshot.EncodeToPNG();
        string fileName = "VRScreenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = "/sdcard/Pictures/" + fileName;
        File.WriteAllBytes(filePath, bytes);

        // 갤러리에 반영
        RefreshAndroidGallery(filePath);

        Debug.Log("스크린샷 저장 완료: " + filePath);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void RefreshAndroidGallery(string path)
    {
        using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            mediaScanner.CallStatic("scanFile", activity, new string[] { path }, null, null);
        }
    }
#else
    private void RefreshAndroidGallery(string path)
    {
        Debug.Log("Android 환경이 아니므로 미디어 갤러리 스캔은 생략됩니다.");
    }
#endif
}
