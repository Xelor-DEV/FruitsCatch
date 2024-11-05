using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Windows;
    public void ActivateWindow(int index)
    {
        Windows[index].SetActive(true);
    }
    public void DeactivateWindow(int index)
    {
        Windows[index].SetActive(false);
    }
}
