////////////////////////////////////////////////////////////
// File: IAgentable.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: An interface for different types of machine learning agents
//////////////////////////////////////////////////////////// 

using UnityEngine;

public interface IAgentable
{
    public void SetTeam(int newTeam);
    public void SetTurn(bool isMyTurn);
    public void Reinforce(int state);
}
