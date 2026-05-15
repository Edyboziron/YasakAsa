using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public string name; // Konuþan kiþinin adý (Örn: Anlatýcý)
    public Sprite characterIcon; // Konuþan kiþinin resmi (YENÝ)
    [TextArea(3, 10)]
    public string text; // Söylediði söz
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Diyalog Kimliði")]
    public string dialogueID;

    [Header("Konuþma Satýrlarý")]
    public List<DialogueLine> lines;
}