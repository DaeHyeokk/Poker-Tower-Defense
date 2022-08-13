
public class AtTheGatesOfHellMission : NonRepeatMission
{
    private EnemyCounter _enemyCounter;
    private readonly string _missionCompletionString = "<color=\"white\">������ �� �տ� ����</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    protected override void Update()
    {
        // �̼��� ����� ���¶�� �ǳʶڴ�.
        if (isEnd) return;

        if (_enemyCounter.roundEnemyCount >= 75)
            CompleteMission();
    }
}
