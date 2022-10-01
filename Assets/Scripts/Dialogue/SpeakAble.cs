using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeakAble : MonoBehaviour
{
    [SerializeField] DialogueObject _dialogue;
    [SerializeField] GameObject _hintText;
    [SerializeField] TextMeshProUGUI _dialogueText;

    private bool _canShowDialogue = false;

    private void Update()
    {
        if (_canShowDialogue)
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;
            if (!_dialogue.HasNextLine())
            {
                CloseDiaglogue();
                return;
            }
            _dialogueText.text = _dialogue.GetNextLine();
            ShowDialogue();
        }
        else
        {
            CloseDiaglogue();
        }

    }

    private void ShowDialogue()
    {
        _dialogueText.transform.parent.gameObject.SetActive(true);
        _dialogueText.gameObject.SetActive(true);
    }

    private void CloseDiaglogue()
    {
        _dialogue.ResetLineNumber();
        _dialogueText.text = "";
        _dialogueText.transform.parent.gameObject.SetActive(false);
        _dialogueText.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _hintText.SetActive(true);
            _canShowDialogue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _hintText.SetActive(false);
            _canShowDialogue = false;
        }
    }
}
