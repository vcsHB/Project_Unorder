using Project_Unorder.DialogueSystem;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour
{
    public LocalizedStringTable myStringTable;
    [SerializeField] private DialogueData _data;

    public void GetText(string key)
    {
        var table = myStringTable.GetTable();
        var entry = table.GetEntry(key);

        Debug.Log(entry.LocalizedValue);
    }
}