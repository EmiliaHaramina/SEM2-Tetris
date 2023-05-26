using UnityEngine;
using UnityEngine.UI;

public class EnableQuizButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _quizScreen;

    // Update is called once per frame
    void Update()
    {
        if (StateManager.gameOver)
        {
            _button.onClick.AddListener(EnableQuizScreen);
            GetComponent<EnableQuizButton>().enabled = false;
        }
    }

    private void EnableQuizScreen()
    {
        _quizScreen.SetActive(true);
    }
}
