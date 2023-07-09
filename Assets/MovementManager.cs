using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _teleportationControllers;
    
    [Header("Providers")]
    [SerializeField] private ActionBasedContinuousMoveProvider _continuousMovementController;
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private ActionBasedSnapTurnProvider _snapTurnProvider;
    [SerializeField] private ActionBasedContinuousTurnProvider _continuousTurnProvider;
    
    private bool _isTeleportationEnabled = true;

    public void CheckMovementSystem()
    {
        if (_isTeleportationEnabled)
        {
            EnableContinuousMovement(true);
            EnableContinuousTurn(true);
            EnableTeleportation(false);
            EnableSnapTurn(false);
            _isTeleportationEnabled = false;
        }
        else
        {
            EnableContinuousMovement(false);
            EnableContinuousTurn(false);
            EnableTeleportation(true);
            EnableSnapTurn(true);
            _isTeleportationEnabled = true;
        }
    }

    private void EnableTeleportation(bool value)
    {
        foreach (var controller in _teleportationControllers)
        {
            controller.SetActive(value);
        }
        _teleportationProvider.enabled = value;
    }

    private void EnableContinuousMovement(bool value)
    {
        _continuousMovementController.enabled = value;
    }

    private void EnableSnapTurn(bool value)
    {
        _snapTurnProvider.enabled = value;
    }

    private void EnableContinuousTurn(bool value)
    {
        _continuousTurnProvider.enabled = value;
    }
}
