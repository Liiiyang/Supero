using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerData 
{
    public int stage;

    public playerData(GameManager gm)
    {
        stage = gm.stage;
    }
    
}
