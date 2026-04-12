using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public Image PlayerHpBar;

    public void playerbar(float hp)
    {
        PlayerHpBar.fillAmount = hp/30;
    }
}
