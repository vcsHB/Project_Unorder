using Project_Unorder.DialogueSystem;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private LocalizedStringTable _characterNameTable;
    [SerializeField] private DialogueData _data;

    private void Awake()
    {

    }
}