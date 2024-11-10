using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private BoardSettings _boardSettings;
    [SerializeField] private BoardStyleSettings _boardStyleSettings;
    [SerializeField] private BoardContoller _boardController;

    private void Start()
    {
        _boardController.Init(_boardSettings, _boardStyleSettings);
    }
}
