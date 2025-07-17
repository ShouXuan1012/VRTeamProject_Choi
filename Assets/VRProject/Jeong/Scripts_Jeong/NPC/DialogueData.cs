using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public string id;
    public List<DialogueLine> dialogue;
    public bool isRead;
}
[System.Serializable]
public class DialogueLine
{
    public string text;
}