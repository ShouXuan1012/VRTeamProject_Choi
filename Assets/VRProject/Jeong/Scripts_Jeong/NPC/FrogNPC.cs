using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
처음엔 NPC 캐릭터 위에 물음표 말풍선 띄우기
가까이 다가가면 말풍선 클릭할 수 있게 활성화
클릭하면 소개 말풍선 띄우기, NPC 캐릭터가 플레이어를 바라보게 만들기
말풍선 클릭하면 다음 대화로 넘어가기
텍스트는 한번에 보여주는 게 아니라 한 글자씩 보여주기
멀어지면 다시 물음표 말풍선 띄우기
다시 가까이 다가가면 처음 대화 상태로 말풍선 띄우기
끝까지 읽으면 말풍선 닫고 '...' 말풍선 띄우기
 */
public class FrogNPC : MonoBehaviour
{
    public string npcID = "NPC_Frog";
    public float textDelay = 0.05f;

    [SerializeField] private GameObject speechCanvas;
    [SerializeField] private GameObject speechBalloon;
    [SerializeField] private Button speechButton;
    [SerializeField] private Text speechText;

    private int currentDialogueIndex = 0;

    private bool isTalking = false;


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
                transform.forward = -Camera.main.transform.forward;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechButton.interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBalloon.SetActive(true);
            speechButton.interactable = false;
            speechText.text = "?";

            currentDialogueIndex = 0;
        }
    }

    private void OnSpeechButtonClicked()
    {
        TextAsset dialogueFile = Resources.Load<TextAsset>($"NPCDialogue/{npcID}");
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(dialogueFile.text);

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
        }
    }
    private void ShowDialogue(string text)
    {
        speechText.text = "";
        StartCoroutine(TypeText(text));
    }
    private IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(textDelay);
        }
    }
}
