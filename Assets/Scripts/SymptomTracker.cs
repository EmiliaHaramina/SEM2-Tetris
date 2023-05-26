using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomTracker : MonoBehaviour
{

    [SerializeField]
    List<GameObject> symptomsBySeverity;

    private int severity;
    private int minimumSeverity = 0;
    private int maximumSeverity;

    // Start is called before the first frame update
    public void StartTracking()
    {
        severity = minimumSeverity;
        maximumSeverity = symptomsBySeverity.Count - 1;

        ShowCurrentSeverity();
    }

    private void HideCurrentSeverity()
    {
        symptomsBySeverity[severity].SetActive(false);
    }

    private void ShowCurrentSeverity()
    {
        symptomsBySeverity[severity].SetActive(true);
    }

    public void WorsenSymptoms()
    {
        if (severity == maximumSeverity)
            return;

        HideCurrentSeverity();
        severity++;
        ShowCurrentSeverity();
    }

    public void ImproveSymptoms()
    {
        if (severity == minimumSeverity)
            return;

        HideCurrentSeverity();
        severity--;
        ShowCurrentSeverity();
    }

    public void ResetTracker()
    {
        HideCurrentSeverity();
        severity = minimumSeverity;
        ShowCurrentSeverity();
    }

}
