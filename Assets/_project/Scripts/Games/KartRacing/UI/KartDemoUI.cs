////////////////////////////////////////////////////////////
// File: KartDemoUI.cs
// Author: Charles Carter
// Date Created: 14/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 14/05/22
// Brief: A script to manage the UI for the Kart Racing Demo Scene
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KartDemoUI : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private KartDemoManager demoManager;

    //Neither of these should include the reused timer
    [SerializeField]
    private GameObject bettingPanel;
    [SerializeField]
    private GameObject raceEndPanel;

    private List<int> bettingAmounts = new List<int>();

    [SerializeField]
    private int StartingAmount = 500;
    private int CurrentAmount = 500;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    [SerializeField]
    private List<TextMeshProUGUI> buttonBetTexts = new List<TextMeshProUGUI>();

    #endregion

    #region Unity Methods

    void Awake()
    {
        for(int i = 0; i < demoManager.drivers.Count; ++i)
        {
            bettingAmounts.Add(0);
        }
    }

    #endregion

    #region Public Methods

    //The order of cars that crossed the finish line
    public void CarWon(List<int> indexes)
    {
        for(int i = 0; i < indexes.Count; ++i)
        {

        }

        ResetBets();
    }

    //Not keeping bets from a previous round
    public void ResetBets()
    {
        for(int i = 0; i < bettingAmounts.Count; ++i)
        {
            bettingAmounts[i] = 0;
        }
    }

    public void CloseBettingPanel()
    {
        bettingPanel.SetActive(false);
    }

    public void OpenBettingPanel()
    {
        currencyText.text = CurrentAmount.ToString();

        bettingPanel.SetActive(true);
    }

    public void CloseRaceEndPanel()
    {
        raceEndPanel.SetActive(false);
    }

    public void OpenRaceEndPanel()
    {
        raceEndPanel.SetActive(true);
    }

    public void PlaceBettingAmount(int index)
    {
        if(CurrentAmount - 50 < 0)
        {
            return;
        }

        CurrentAmount -= 50;
        bettingAmounts[index] += 50;

        UpdateBetTexts();
    }

    public void RemoveBettingAmount(int index)
    {
        if(bettingAmounts[index] > 0 )
        {
            return;
        }

        CurrentAmount += 50;
        bettingAmounts[index] -= 50;

        UpdateBetTexts();
    }

    #endregion

    #region Private Methods

    private void UpdateBetTexts()
    {
        currencyText.text = CurrentAmount.ToString();

        for(int i = 0; i < buttonBetTexts.Count; ++i)
        {
            buttonBetTexts[i].text = bettingAmounts[i].ToString();
        }
    }

    #endregion
}
