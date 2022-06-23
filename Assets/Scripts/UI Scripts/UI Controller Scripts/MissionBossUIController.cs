using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionBossUIController : MonoBehaviour
{
    [SerializeField]
    private Button[] _missionBossButtons;
    [SerializeField]
    private Slider[] _missionBossCooltimeSliders;
    [SerializeField]
    private TextMeshProUGUI[] _missionBossCooltimeTexts;

    private readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    public void SetMissionBossCooltimeSlider(int index, float maxValue)
    {
        _missionBossCooltimeSliders[index].maxValue = maxValue;
        _missionBossCooltimeSliders[index].value = maxValue;
        _missionBossCooltimeSliders[index].gameObject.SetActive(false);
    }

    public void StartMissionBossCooltime(int bossLevel, float cooltime)
    {
        StartCoroutine(MissionBossCooltimeCoroutine(bossLevel, cooltime));
    }

    private IEnumerator MissionBossCooltimeCoroutine(int bossLevel, float cooltime)
    {
        // 미션 보스를 소환하는 버튼의 상호작용 기능 해제
        _missionBossButtons[bossLevel].interactable = false;

        float remainCooltime = cooltime;

        _missionBossCooltimeTexts[bossLevel].text = cooltime.ToString();
        _missionBossCooltimeSliders[bossLevel].value = _missionBossCooltimeSliders[bossLevel].maxValue;
        _missionBossCooltimeSliders[bossLevel].gameObject.SetActive(true);

        while (remainCooltime > 0)
        {
            float oneSecond = 1f;
            while(oneSecond > 0)
            {
                yield return _waitForFixedUpdate;
                oneSecond -= Time.fixedDeltaTime;
            }

            remainCooltime -= 1f;
            _missionBossCooltimeTexts[bossLevel].text = remainCooltime.ToString();
            _missionBossCooltimeSliders[bossLevel].value--;
        }

        _missionBossCooltimeSliders[bossLevel].gameObject.SetActive(false);

        // 미션 보스를 소환하는 버튼의 상호작용 기능 활성화
        _missionBossButtons[bossLevel].interactable = true;
    }
}
