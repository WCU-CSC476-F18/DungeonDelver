using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover {//a publci declaration of the IFacingMover interface
    //publicly accessible methods and properties on any class that implements this interface. 
    int GetFacing();
    bool moving { get;  }
    float GetSpeed();
    float gridMult { get; }
    Vector2 roomPos { get; set; }
    Vector2 roomNum { get; set; }
    //A default value for its mult parameter.
    Vector2 GetRoomPosOnGrid(float mult = -1);
}
