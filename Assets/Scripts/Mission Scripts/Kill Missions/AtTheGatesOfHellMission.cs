
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

    protected override void Update()
    {
        // 미션이 종료된 상태라면 건너뛴다.
        if (isEnd) return;

        if (_enemyCounter.roundEnemyCount >= 75)
            CompleteMission();
    }
}
