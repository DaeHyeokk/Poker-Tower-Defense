
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

    private void Update()
    {
        // �̼��� �̹� �ϼ������� �������� �ʴ´�.
        if (isCompleted) return;

        CheckMission();
    }

    public override void CheckMission()
    {
        if (_enemyCounter.roundEnemyCount >= 75)
            GiveReward();
    }
}
