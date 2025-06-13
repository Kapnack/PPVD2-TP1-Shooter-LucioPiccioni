using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(InputSystemUIInputModule))]
public class EventSystemManager : MonoBehaviour, IEventSystemManager
{
    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        ServiceProvider.SetService<IEventSystemManager>(this);
    }

    public void SetSelectedObject(GameObject obj)
    {
        if (obj == null) return;

        eventSystem.SetSelectedGameObject(obj);
    }

    public void SetSelectedObject(GridLayoutGroup gridLayoutGroup)
    {
        if (gridLayoutGroup == null) return;

        GameObject obj = gridLayoutGroup.GetComponentsInChildren<Button>().FirstOrDefault(b => b.gameObject.activeInHierarchy).gameObject;

        eventSystem.SetSelectedGameObject(obj);
    }
}
