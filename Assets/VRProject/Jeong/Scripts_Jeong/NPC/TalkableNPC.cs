using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TalkableNPC : MonoBehaviour
{
    [SerializeField] private string npcID = "NPC_Frog";
    [SerializeField] private float textDelay = 0.05f;

    [Header("UI Elements")]
    [SerializeField] private GameObject speechCanvas;
    [SerializeField] private GameObject speechBalloon;
    [SerializeField] private Button speechButton;
    [SerializeField] private Text speechText;
    [SerializeField] private Text initialText;

    private TextAsset dialogueFile;
    private DialogueData dialogueData;

    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;

    private bool isRead = false;
    private bool isTalking = false;
    private bool isTyping = false;

    private void Start()
    {
        LoadDialogueData();

        if (dialogueData == null || dialogueData.dialogue.Count == 0)
        {
            Debug.LogError($"No dialogue found for {npcID}.");
            return;
        }

        isRead = dialogueData.isRead;

        speechBalloon.SetActive(true);
        speechText.gameObject.SetActive(false);
        initialText.gameObject.SetActive(true);

        speechText.text = "";
        if (isRead) initialText.text = "...";
        else initialText.text = "?";

        speechButton.interactable = false;
        speechButton.onClick.AddListener(OnSpeechButtonClicked);
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            speechCanvas.transform.forward = Camera.main.transform.forward;

            if (isTalking)
            {
                Vector3 dir = Camera.main.transform.position - transform.position;
                dir.y = 0; // Y축 회전 무시
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsLocalPlayer(other))
        {
            speechButton.interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && IsLocalPlayer(other))
        {
            isTalking = false;
            isTyping = false;

            currentDialogueIndex = 0;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            speechBalloon.SetActive(true);
            speechText.gameObject.SetActive(false);
            initialText.gameObject.SetActive(true);

            speechText.text = "";
            if (isRead) initialText.text = "...";
            else initialText.text = "?";

            speechButton.interactable = false;
        }
    }

    private void OnSpeechButtonClicked()
    {
        if (isTyping) return;

        if (currentDialogueIndex < dialogueData.dialogue.Count)
        {
            if (!isTalking)
            {
                isTalking = true;

                speechText.gameObject.SetActive(true);
                initialText.gameObject.SetActive(false);
            }

            ShowDialogue(dialogueData.dialogue[currentDialogueIndex].text);

            currentDialogueIndex++;
        }
        // 대화 끝났을 때
        else
        {
            isTalking = false;
            isRead = true;

            SaveDialogueState(isRead);

            speechBalloon.SetActive(false);
            speechText.gameObject.SetActive(false);
            initialText.gameObject.SetActive(false);

            speechText.text = "";
            initialText.text = "...";

            speechButton.interactable = false;

            currentDialogueIndex = 0;
        }
    }
    private void LoadDialogueData()
    {
        dialogueFile = Resources.Load<TextAsset>($"NPCDialogue/{npcID}");
        if (dialogueFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(dialogueFile.text);
        }
    }
    private void ShowDialogue(string text)
    {
        speechText.text = "";
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        foreach (char letter in text.ToCharArray())
        {
            speechText.text += letter;
            if (letter != ' ') yield return new WaitForSeconds(textDelay);
        }
        isTyping = false;
    }
    private void SaveDialogueState(bool isRead)
    {
        if (dialogueData != null)
        {
            dialogueData.isRead = isRead;

            string json = JsonUtility.ToJson(dialogueData, true);
            System.IO.File.WriteAllText($"{Application.dataPath}/Resources/NPCDialogue/{npcID}.json", json);
        }
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}
