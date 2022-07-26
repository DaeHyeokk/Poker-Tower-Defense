
public class AtTheGatesOfHellMission : NonRepeatMission
{
    private EnemyCounter _enemyCounter;
    private readonly string _missionCompletionString = "<color=\"white\">지옥의 문 앞에 서다</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    private void Update()
    {
        // 미션을 이미 완수했으면 수행하지 않는다.
        if (isCompleted) return;

        CheckMission();
    }

    public override void CheckMission()
    {
        if (_enemyCounter.roundEnemyCount >= 75)
            GiveReward();
    }
}
