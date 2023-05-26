using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    [SerializeField] private Button _shuffleButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private GameObject _landingPage;
    [SerializeField] private GameObject _finalPage;
    [SerializeField] private List<GameObject> _quizPages;

    public static Action OnQuizStarted;
    public static Action OnResetAnswerButtons;
    public static Action OnStartTimer;
    public static Action OnQuizEnded;
    public static Action<GameObject> OnShuffleAnswerButtons;
    public static Action<GameObject> OnDelegateAnswerButtons;

    private int[] _questionOrder;
    private int _currentPageNo = 0;

    private void OnEnable()
    {
        _shuffleButton?.onClick.AddListener(SetNewQuestionOrder);
        AnswerButton.OnNextQuestion += EnableNextQuestion;
        TimeController.OnTimerEnded += EnableNextQuestion;
    }

    private void OnDisable()
    {
        _shuffleButton?.onClick.RemoveListener(SetNewQuestionOrder);
        AnswerButton.OnNextQuestion -= EnableNextQuestion;
        TimeController.OnTimerEnded -= EnableNextQuestion;
    }

    public void RestartQuiz()
    {
        _currentPageNo = 0;
        ShowNextQuestion(_finalPage, _landingPage);
        AnswerController.ResetScore();

        foreach (GameObject page in _quizPages)
        {
            GameObject nextButtonObject = page.transform.Find("NextBtn").gameObject;
            Button nextButton = nextButtonObject.GetComponent<Button>();

            nextButtonObject.SetActive(false);
            nextButton.onClick.RemoveListener(NextQuestion);
        }
    }

    private void SetNewQuestionOrder()
    {
        _questionOrder = QuestionController.GenerateRandomOrder(_quizPages.Count);
        OnQuizStarted?.Invoke();
        ShowNextQuestion(_landingPage, _quizPages[_questionOrder[_currentPageNo]]);
        PrepareAnswers(_quizPages[_questionOrder[_currentPageNo]]);
    }

    private void ShowNextQuestion(GameObject currentPage, GameObject nextPage)
    {
        currentPage.SetActive(false);
        nextPage.SetActive(true);
    }

    private void EnableNextQuestion()
    {
        GameObject currentPage = _quizPages[_questionOrder[_currentPageNo]];
        GameObject nextButtonObject = currentPage.transform.Find("NextBtn").gameObject;

        Button nextButton = nextButtonObject.GetComponent<Button>();

        nextButtonObject.SetActive(true);
        nextButton.onClick.AddListener(NextQuestion);
    }

    private void NextQuestion()
    {
        GameObject currentPage = _quizPages[_questionOrder[_currentPageNo]];

        if (_currentPageNo < _quizPages.Count - 1)
        {
            GameObject nextPage = _quizPages[_questionOrder[_currentPageNo + 1]];
            ShowNextQuestion(currentPage, nextPage);
            PrepareAnswers(nextPage);
            _currentPageNo++;
        }
        else
        {
            ShowNextQuestion(currentPage, _finalPage);
            QuizEnding();
        }
    }

    private void QuizEnding()
    {
        OnQuizEnded?.Invoke();

        GameObject descriptionObject = _finalPage.transform.Find("Description").gameObject;
        var description = descriptionObject.GetComponent<TextMeshPro>();
        
        description.text = string.Format("Rezultat: {0}/{1}", AnswerController.GetScore(), _quizPages.Count);
        description.fontSize = 300;
    }

    private void PrepareAnswers(GameObject nextPage)
    {
        OnShuffleAnswerButtons?.Invoke(nextPage);
        OnResetAnswerButtons?.Invoke();
        OnStartTimer?.Invoke();
    }
}
