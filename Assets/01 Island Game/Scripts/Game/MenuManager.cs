using System.Globalization;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject IngameUI;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Victory;
    [SerializeField] private GameObject Lose;
    [SerializeField] private GameObject HiddenSymbol;
    [SerializeField] private GameObject SeenSymbol;

    void OnEnable()
    {
        GameManager.OnStateChange += GameManagerOnOnStateChange;
    }
    void OnDisable()
    {
        GameManager.OnStateChange -= GameManagerOnOnStateChange;
    }
    private void GameManagerOnOnStateChange(GameState state)
    {
        Debug.Log($"MenuManager received state change: {state}");

        if (Menu == null)
        {
            Debug.LogError("Menu reference is MISSING on MenuManager!");
            return;
        }
        Menu.SetActive(state == GameState.Menu);

        IngameUI.SetActive(state != GameState.Menu && state != GameState.Victory && state != GameState.Lose);

        HiddenSymbol.SetActive(state == GameState.CollectStealth);

        SeenSymbol.SetActive(state == GameState.CollectSeen);

        Victory.SetActive(state == GameState.Victory);

        Lose.SetActive(state == GameState.Lose);

        if (state == GameState.Menu || state == GameState.Victory || state == GameState.Lose)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
