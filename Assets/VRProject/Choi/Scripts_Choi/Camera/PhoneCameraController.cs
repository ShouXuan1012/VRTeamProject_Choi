using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class PhoneCameraController : MonoBehaviour
{
    [Header("카메라들")]
    [SerializeField] private Camera selfieCamera;
    [SerializeField] private Camera normalCamera;

    [Header("렌더 텍스처")]
    [SerializeField] private RenderTexture renderTexture;

    [Header("화면")]
    [SerializeField] private RawImage phoneScreenUI;

    [Header("UI 버튼")]
    [SerializeField] private Button switchCameraButton;
    [SerializeField] private Button takePhotoButton;

    private Camera currentCamera;

    void Start()
    {
        SetActiveCamera(selfieCamera); // 시작은 셀피카메라

        switchCameraButton.onClick.AddListener(SwitchCamera);
        takePhotoButton.onClick.AddListener(TakePhoto);
    }

    void SetActiveCamera(Camera cam)
    {
        if (currentCamera != null)
        {
            currentCamera.targetTexture = null;
            currentCamera.gameObject.SetActive(false);
        }

        currentCamera = cam;
        currentCamera.targetTexture = renderTexture;
        currentCamera.gameObject.SetActive(true);

        phoneScreenUI.texture = renderTexture;
    }

    void SwitchCamera()
    {
        if (currentCamera == selfieCamera)
        {
            SetActiveCamera(normalCamera);
            phoneScreenUI.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            SetActiveCamera(selfieCamera);
            phoneScreenUI.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    void TakePhoto()
    {
        RenderTexture.active = renderTexture;
        Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        string filename = $"photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(path, photo.EncodeToPNG());

        Debug.Log($"사진 저장됨: {path}");

        RenderTexture.active = null;
        AchievementManager.Instance.AddProgress(EAchievementType.PhotoMaster, 1);

    }
}
