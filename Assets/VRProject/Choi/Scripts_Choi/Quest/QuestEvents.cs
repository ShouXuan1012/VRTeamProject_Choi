using System;
using System.Collections.Generic;

public static class QuestEvents
{
    private static Dictionary<EQuestType, Action> questEvents = new();

    public static void Subscribe(EQuestType type, Action callback)
    {
        if (questEvents.ContainsKey(type))
            questEvents[type] += callback;
        else
            questEvents[type] = callback;
    }

    public static void Unsubscribe(EQuestType type, Action callback)
    {
        if (questEvents.ContainsKey(type))
            questEvents[type] -= callback;
    }

    public static void Invoke(EQuestType type)
    {
        questEvents[type]?.Invoke();
    }
}
