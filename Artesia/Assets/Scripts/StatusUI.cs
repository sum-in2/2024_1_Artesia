using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    public Image HPbar;
    private PlayerStat playerStat;

    public TextMeshProUGUI HPText;

    private void Start()
    {
        playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if(playerStat != null){
            SetHPBar();
            SetHPText();
        }
    }

    private void SetHPBar()
    {
        float fillAmount = (float)playerStat.NowHp / playerStat.Hp;

        HPbar.fillAmount = Mathf.Clamp01(fillAmount);
    }

    private void SetHPText()
    {
        HPText.text = $"HP {playerStat.NowHp}/{playerStat.Hp}";
    }
}