////////////////////////////////////////////////////////////
// File: MLInputManager.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: The Machine Learning Agent's action outcomes
//////////////////////////////////////////////////////////// 

using Unity.MLAgents;
using UnityEngine;

public class MLInputManager : Agent, IAgentable
{
    #region Interface Contracts

    void IAgentable.Reinforce(bool negative)
    {
        throw new System.NotImplementedException();
    }

    void IAgentable.SetTeam(int newTeam)
    {
        throw new System.NotImplementedException();
    }

    void IAgentable.SetTurn(bool isMyTurn)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Variables

    [SerializeField]
    private CharacterActions actions;

    #endregion

    #region Unity Methods

    void Start()
    {
	
    }
 
    void Update()
    {
	
    }

    #endregion

    #region Override Methods


    #endregion

    #region Private Methods

    

    #endregion
}
