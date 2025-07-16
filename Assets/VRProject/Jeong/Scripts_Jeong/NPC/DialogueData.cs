using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public string id;
    public List<DialogueLine> dialogue;
}
[System.Serializable]
public class DialogueLine
{
    public string text;
}