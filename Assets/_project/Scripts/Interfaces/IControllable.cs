////////////////////////////////////////////////////////////
// File: IControllable.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: An interface for anything that can be controlled within the game
//////////////////////////////////////////////////////////// 

public interface IControllable
{
    public void UseEquip();
    public void ThrowEquip();
    public void MoveLeft();
    public void MoveRight();
    public void MoveForward();
    public void MoveBackward();
    public void TurnLeft();
    public void TurnRight();
}
