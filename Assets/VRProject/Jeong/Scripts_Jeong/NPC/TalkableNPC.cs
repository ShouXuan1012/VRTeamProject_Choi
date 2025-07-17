using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TalkableNPC : MonoBehaviour
{
    public string npcID = "NPC_Frog";
    public float textDelay = 0.05f;

    [SerializeField] private GameObject speechCanvas;
    [SerializeField] private GameObject speechBalloon;
    [SerializeField] private Button speechButton;
    [SerializeField] private Text speechText;

    private TextAsset dialogueFile;
    private DialogueData dialogueData;
    private int currentDialogueIndex = 0;

    private bool isRead = false;
    private bool isTalking = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    private void Start()
    {
        speechBalloon.SetActive(true);
        speechButton.interactable = false;
        speechText.text = "?";

        speechButton.onClick.AddListener(OnSpeechButtonClicked);
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            speechCanvas.transform.forward = Camera.main.transform.forward;

            if (isTalking)
            {
                Vector3 directionToPlayer = Camera.main.transform.position - transform.position;
                directionToPlayer.y = 0; // Y축 회전 무시
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView view = other.GetComponent<PhotonView>();
            if (view == null || !view.IsMine) return;
            speechButton.interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView view = other.GetComponent<PhotonView>();
            if (view == null || !view.IsMine) return;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            speechBalloon.SetActive(true);
            speechButton.interactable = false;

            if (!isRead)
            {
                speechText.text = "?";
            }
            else
            {
                speechText.text = "...";
            }

            currentDialogueIndex = 0;

            isTalking = false;
            isTyping = false;
        }
    }

    private void OnSpeechButtonClicked()
    {
        if (dialogueData == null || dialogueData.dialogue.Count == 0)
        {
            LoadDialogueData();
        }

        if (isTyping)
        {
            return; // 타이핑 중엔 클릭 무시
        }

        if (currentDialogueIndex < dialogueData.dialogue.Count)
        {
            if (!isTalking)
            {
                isTalking = true;
            }

            ShowDialogue(dialogueData.dialogue[currentDialogueIndex].text);
            currentDialogueIndex++;
        }
        else
        {
            isTalking = false;

            speechBalloon.SetActive(false);
            speechButton.interactable = false;
            speechText.text = "...";

            currentDialogueIndex = 0;

            isRead = true;
            SaveDialogueState(isRead);
        }
    }
    private void LoadDialogueData()
    {
        dialogueFile = Resources.Load<TextAsset>($"NPCDialogue/{npcID}");
        if (dialogueFile != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(dialogueFile.text);
        }
        else
        {
            Debug.LogError($"Dialogue file for {npcID} not found!");
        }
    }
    private void ShowDialogue(string text)
    {
        speechText.text = "";
        typingCoroutine = StartCoroutine(TypeText(text));
    }
    private IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            isTyping = true;
            speechText.text += letter;
            yield return new WaitForSeconds(textDelay);
        }
        isTyping = false;
    }
    public void SaveDialogueState(bool isRead)
    {
        if (dialogueData != null)
        {
            dialogueData.isRead = isRead;
            string json = JsonUtility.ToJson(dialogueData, true);
            System.IO.File.WriteAllText(Application.dataPath + $"/Resources/NPCDialogue/{npcID}.json", json);
        }
    }
}
