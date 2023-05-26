using UnityEngine;

public class ButtonColliderToggle : MonoBehaviour
{
    [SerializeField] bool _buttonStatus;

    private void OnEnable()
    {
        PhysicsButton.onButtonPressed += ToggleCollider;
    }

    private void OnDisable()
    {
        PhysicsButton.onButtonPressed -= ToggleCollider;
    }

    private void ToggleCollider(bool status)
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (status)
        {
            boxCollider.enabled = !_buttonStatus;
        }
        else
        {
            boxCollider.enabled = _buttonStatus;
        }
    }
}
