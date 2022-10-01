using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue", fileName ="DialogueObject", order = 0)]
public class DialogueObject : ScriptableObject
{
    [SerializeField] List<string> _dialogues = new List<string>();

    private int nextLine = 0;

    public string GetNextLine()
    {
        return _dialogues[nextLine++];
    }

    public void ResetLineNumber()
    {
        nextLine = 0;
    }

    public bool HasNextLine()
    {
        return ! (nextLine == _dialogues.Count);
    }
}

