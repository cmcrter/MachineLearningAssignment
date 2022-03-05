////////////////////////////////////////////////////////////
// File: TTTMLAgent.cs
// Author: Charles Carter
// Date Created: 07/02/22   
// Last Edited By: Charles Carter
// Date Last Edited: 07/02/22
// Brief: A parent interface to be able to use different types of ML
//////////////////////////////////////////////////////////// 

namespace ML.TTT
{
    public interface iAgent
    {
        public void SetTeam(Tile_State newTeam);
        public void SetTurn(bool isMyTurn);
        public void Reinforce(Tile_State endState);
    }
}
