using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;

public class PhoneCameraController : MonoBehaviourPun
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
        // 내 전용 RenderTexture 생성
        renderTexture = new RenderTexture(512, 512, 16);
        renderTexture.name = $"RenderTexture_{photonView.ViewID}";

        SetActiveCamera(selfieCamera);

        if (photonView.IsMine)
        {
            switchCameraButton.onClick.AddListener(SwitchCamera);
            takePhotoButton.onClick.AddListener(TakePhoto);
        }
        else
        {
            switchCameraButton.gameObject.SetActive(false);
            takePhotoButton.gameObject.SetActive(false);
        }
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
        if (!photonView.IsMine) return; // 내 조작만 가능

        bool isSelfie = (currentCamera == selfieCamera) ? false : true;
        photonView.RPC(nameof(RPC_SwitchCamera), RpcTarget.AllBuffered, isSelfie);
    }

    [PunRPC]
    void RPC_SwitchCamera(bool toSelfie)
    {
        if (toSelfie)
        {
            SetActiveCamera(selfieCamera);
            phoneScreenUI.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            SetActiveCamera(normalCamera);
            phoneScreenUI.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void TakePhoto()
    {
        RenderTexture.active = renderTexture;
        Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        string filename = $"photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";

#if UNITY_ANDROID && !UNITY_EDITOR
    string picturesPath = "/storage/emulated/0/Pictures/MyVRPhotos"; // 갤러리 폴더
    if (!Directory.Exists(picturesPath))
        Directory.CreateDirectory(picturesPath);

    string path = Path.Combine(picturesPath, filename);
#else
        string path = Path.Combine(Application.persistentDataPath, filename);
#endif

        File.WriteAllBytes(path, photo.EncodeToPNG());
        Debug.Log($"사진 저장됨: {path}");

        RenderTexture.active = null;
        AchievementManager.Instance.AddProgress(EAchievementType.PhotoMaster, 1);
    }
}
