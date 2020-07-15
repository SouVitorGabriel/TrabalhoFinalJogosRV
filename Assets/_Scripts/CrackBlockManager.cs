using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackBlockManager : MonoBehaviour
{
    public _CrackBlock[] Rachaveis;

    public void ResetAll()
    {
        foreach(_CrackBlock c in Rachaveis)
        {
            c.Recomeco();
        }
    }
}
