using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStars : MonoBehaviour
{
    public void Ach1()
    {
        GameManager.Instance.SetStars("add_01", 3);
    }

    public void Ach2()
    {
    //    GameManager.Instance.AddQuestProgress("daily_math_01", 1);
    }

    public void Ach3()
    {
        GameManager.Instance.AddXP(100);
    }
}
