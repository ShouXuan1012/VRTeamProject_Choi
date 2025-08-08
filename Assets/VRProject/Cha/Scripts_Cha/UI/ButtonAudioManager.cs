using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEditor;

public class ButtonSound : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GlobalUIButtonSoundManager.Instance != null)
            {
                GlobalUIButtonSoundManager.Instance.PlayClickSound();
            }
        });
    }
}

public class GlobalUIButtonSoundManager : MonoBehaviour
{
    public static GlobalUIButtonSoundManager Instance;

    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (clickSound == null)
        {
            clickSound = Resources.Load<AudioClip>("VRProject/Cha/Sounds_Cha/UI/BlopSound");
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}

#if UNITY_EDITOR
public class ButtonSoundAutoAttach
{
    [MenuItem("Tools/Attach ButtonSound to All Buttons in Scene")]
    public static void AttachButtonSound()
    {
        Button[] buttons = Object.FindObjectsOfType<Button>(true);
        int count = 0;

        foreach (Button btn in buttons)
        {
            if (btn.GetComponent<ButtonSound>() == null)
            {
                Undo.AddComponent<ButtonSound>(btn.gameObject);
                count++;
            }
        }

        Debug.Log($"ButtonSound 스크립트가 {count}개의 버튼에 추가되었습니다.");
    }
}
#endif
