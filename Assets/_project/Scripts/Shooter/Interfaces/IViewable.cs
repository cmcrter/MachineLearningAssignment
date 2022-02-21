////////////////////////////////////////////////////////////
// File: IViewable.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: This is something important that can be viewed by a player (which was hidden previously)
//////////////////////////////////////////////////////////// 

using UnityEngine;

public interface IViewable
{
    //Should return what it is, to be able to put 
    public int GetID();

    bool isSeen
    {
        get;
    }
}
