using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Text _heading;
    [SerializeField] private float _totalTime = 30f;

    public static Action OnTimerEnded;
    public static Action<bool> OnDisableButtons;

    private float _currentTime;
    private bool _isReady = false;

    private void OnEnable()
    {
        QuizController.OnStartTimer += SetTimer;
        QuizController.OnQuizEnded += ResetHeading;
        AnswerButton.OnAnswerClicked += PauseTimer;
    }

    private void OnDisable()
    {
        QuizController.OnStartTimer -= SetTimer;
        QuizController.OnQuizEnded -= ResetHeading;
        AnswerButton.OnAnswerClicked -= PauseTimer;
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (_isReady)
        {
            if (_currentTime > 0f)
            {
                StartTimer();
            }
            else
            {
                _heading.text = "Vrijeme isteklo!";
                _isReady = false;
                OnTimerEnded?.Invoke();
                OnDisableButtons?.Invoke(false);
                ResetTimer();
            }
        }
    }

    private void StartTimer()
    {
        _currentTime -= Time.deltaTime;
        _heading.text = "Preostalo: " + _currentTime.ToString("F2");
    }

    private void SetTimer()
    {
        ToggleTimer(true);
        ResetTimer();
    }

    private void ResetTimer()
    {
        _currentTime = _totalTime;
    }

    private void ToggleTimer(bool toggleValue)
    {
        _isReady = toggleValue;
    }

    private void PauseTimer(bool value)
    {
        if (value)
        {
            ToggleTimer(!value);
        }
        else
        {
            ToggleTimer(value);
        }
    }

    private void ResetHeading()
    {
        _heading.text = "Kviz";
    }
}
