using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour
{
    public static Action OnAnswerValidation;

    private static int _score = 0;

    private void OnEnable()
    {
        AnswerButton.OnAnswerClicked += CheckAnswer;
        QuizController.OnShuffleAnswerButtons += ShuffleAnswerButtons;
    }

    private void OnDisable()
    {
        AnswerButton.OnAnswerClicked -= CheckAnswer;
        QuizController.OnShuffleAnswerButtons -= ShuffleAnswerButtons;
    }

    public static int GetScore()
    {
        return _score;
    }

    public static void ResetScore()
    {
        _score = 0;
    }

    private void CheckAnswer(bool valid)
    {
        if (valid)
        {
            _score++;
        }
        OnAnswerValidation?.Invoke();
    }

    private void ShuffleAnswerButtons(GameObject quizPage)
    {
        GameObject answerButtonsParent = quizPage.transform.Find("AnswerBtns").gameObject;
        int[] newButtonOrder = QuestionController.GenerateRandomOrder(answerButtonsParent.transform.childCount);
        List<bool> validity = new List<bool>();
        List<string> answerTexts = new List<string>();
        foreach (Transform child in answerButtonsParent.transform)
        {
            var text = child.GetComponentInChildren<Text>();
            answerTexts.Add(text.text);

            var answerButton = child.GetComponent<AnswerButton>();
            validity.Add(answerButton.Correct);
        }

        for (int i = 0; i < newButtonOrder.Length; i++)
        {
            Transform child = answerButtonsParent.transform.GetChild(i);
            var text = child.GetComponentInChildren<Text>();
            text.text = answerTexts[newButtonOrder[i]];

            var answerButton = child.GetComponent<AnswerButton>();
            answerButton.Correct = validity[newButtonOrder[i]];
        }
    }
}
