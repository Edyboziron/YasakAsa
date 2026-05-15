using UnityEngine;
using UnityEngine.UI; // Image bileþeni için
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class TypewriterManager : MonoBehaviour
{
    [Header("UI Baðlantýlarý")]
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI nameDisplay;
    public Image characterPortrait; // Karakter resmi (YENÝ)
    public GameObject dialoguePanel;
    public GameObject clickButton;

    [Header("Ses ve Hýz")]
    public AudioSource audioSource;
    public AudioClip speakingSound;
    [Range(0.01f, 0.1f)] public float typingSpeed = 0.04f;
    [Range(1, 5)] public int audioFrequency = 2;

    private Queue<DialogueLine> linesQueue;
    private bool isTyping = false;
    private string currentFullSentence;
    private Action onDialogueFinished;

    void Start()
    {
        linesQueue = new Queue<DialogueLine>();
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (clickButton != null) clickButton.SetActive(false);
    }

    public void StartDialogue(DialogueData data, Action onFinished = null)
    {
        if (data == null) return;

        onDialogueFinished = onFinished;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (clickButton != null) clickButton.SetActive(true);

        linesQueue.Clear();
        foreach (DialogueLine line in data.lines) linesQueue.Enqueue(line);

        DisplayNextSentence();
    }

    public void OnClickScreen()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textDisplay.text = currentFullSentence;
            textDisplay.maxVisibleCharacters = currentFullSentence.Length;
            isTyping = false;
        }
        else DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = linesQueue.Dequeue();

        // 1. Ýsmi ve RESMÝ Güncelle (YENÝ)
        if (nameDisplay != null) nameDisplay.text = currentLine.name;
        if (characterPortrait != null && currentLine.characterIcon != null)
            characterPortrait.sprite = currentLine.characterIcon;

        currentFullSentence = currentLine.text;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textDisplay.text = sentence;
        textDisplay.maxVisibleCharacters = 0;

        int counter = 0;
        while (textDisplay.maxVisibleCharacters < sentence.Length)
        {
            textDisplay.maxVisibleCharacters++;
            if (counter % audioFrequency == 0 && audioSource != null && speakingSound != null)
            {
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(speakingSound);
            }
            counter++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (clickButton != null) clickButton.SetActive(false);
        if (onDialogueFinished != null) onDialogueFinished.Invoke();
    }
}