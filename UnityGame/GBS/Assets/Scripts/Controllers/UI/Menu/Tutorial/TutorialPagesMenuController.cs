using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NOTE : This could be reused in the scoreboard or somewhere else to make multi-page menus.
public class TutorialPagesMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonNext;

    [SerializeField] private GameObject[] pages;

    private int currentPage;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.currentPage = 0;
        UpdateDisplayedTutorialPage();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneTutorial();
    }

    public void Button_Next()
    {
        this.currentPage++;
        UpdateDisplayedTutorialPage();
    }

    public void Button_Previous()
    {
        this.currentPage--;
        UpdateDisplayedTutorialPage();
    }

    #endregion

    #region PrivateMethods

    private void HideTutorialPages()
    {
        foreach (var page in this.pages)
            page.SetActive(false);
    }

    private void DisplayTutorialPage(int idx)
    {
        HideTutorialPages();
        this.pages[idx].SetActive(true);
    }

    private void UpdateDisplayedTutorialPage()
    {
        DisplayTutorialPage(this.currentPage);

        this.buttonBack.gameObject.SetActive(this.currentPage > 0);
        this.buttonNext.gameObject.SetActive(this.currentPage < this.pages.Length - 1);

    }

    #endregion
}
