using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionIgnoreMouse : MonoBehaviour
{
    private EventSystem _eventSystem;
    private Selectable _latestSelected;

    private void Update()
    {
        _eventSystem = EventSystem.current;

        if (EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>())
        {
            _latestSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        }

        if (!_eventSystem)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _eventSystem.SetSelectedGameObject(_latestSelected.gameObject);
        }
    }
}
