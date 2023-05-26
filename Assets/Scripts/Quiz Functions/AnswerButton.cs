using System;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [Header("Local References")]
    public bool Correct;
    
    [SerializeField] private Button _button;
    [SerializeField] private Text _text;
    
    [Header("Settings")]
    [SerializeField] private Color _correctColor;
    [SerializeField] private Color _invalidColor;

    public static Action<bool> OnAnswerClicked;
    public static Action OnNextQuestion;

    private Color _defaultColor = new Color(71, 71, 71);

    private void OnEnable()
    {
        _button.onClick.AddListener(AnswerButtonClicked);
        AnswerController.OnAnswerValidation += ValidateAnswer;
        QuizController.OnResetAnswerButtons += ResetButton;
        TimeController.OnDisableButtons += ToggleButtonInteractability;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(AnswerButtonClicked);
        AnswerController.OnAnswerValidation -= ValidateAnswer;
        QuizController.OnResetAnswerButtons -= ResetButton;
        TimeController.OnDisableButtons -= ToggleButtonInteractability;
    }

    private void ToggleButtonInteractability(bool toggleValue)
    {
        _button.interactable = toggleValue;
    }

    private void AnswerButtonClicked()
    {
        OnAnswerClicked?.Invoke(Correct);
        OnNextQuestion?.Invoke();
    }

    private void ValidateAnswer()
    {
        PaintButtonBackground();
        ToggleButtonInteractability(false);
    }

    private void ResetButton()
    {
        ToggleButtonInteractability(true);
        _button.image.color = _defaultColor;
    }

    private void PaintButtonBackground()
    {
        _button.image.color = (Correct ? _correctColor : _invalidColor);
    }
}
