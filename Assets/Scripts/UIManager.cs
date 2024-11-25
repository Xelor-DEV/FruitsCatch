using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Windows;
    [SerializeField] private GameObject[] buttons;

    private void Start()
    {
        DeactivateAllButtons();
    }
    private void OnEnable()
    {
        DBConn.OnLoginSuccess += ActivateAllButtons;
        DBConn.OnLogout += DeactivateAllButtons;
    }
    private void OnDisable()
    {
        DBConn.OnLoginSuccess -= ActivateAllButtons;
        DBConn.OnLogout -= DeactivateAllButtons;
    }

    public void ActivateWindow(int index)
    {
        Windows[index].SetActive(true);
    }

    public void DeactivateWindow(int index)
    {
        Windows[index].SetActive(false);
    }

    public void ActivateAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
        }
    }
    public void DeactivateAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }
}
