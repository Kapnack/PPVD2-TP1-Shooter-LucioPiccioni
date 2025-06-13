using UnityEngine;
using UnityEngine.UI;

public interface IEventSystemManager
{
    public void SetSelectedObject(GameObject obj);
    public void SetSelectedObject(GridLayoutGroup gridLayoutGroup);
}