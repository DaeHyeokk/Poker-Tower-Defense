using UnityEngine;
using UnityEngine.UI;

public class MissionTower : MonoBehaviour
{
    private enum SelectTowerType { ���þ���, ž, �����, �����, Ʈ����, ��Ʈ����Ʈ, ����ƾ, �÷���, Ǯ�Ͽ콺, ��ī�ε�, ��Ʈ����Ʈ�÷��� }
    private enum SelectTowerColor { ���þ���, ����, �ʷ�, �Ķ� }

    [SerializeField]
    private HorizontalLayoutGroup _levelLayout;
    [SerializeField]
    private SelectTowerType _selectTowerType;
    [SerializeField]
    private SelectTowerColor _selectTowerColor;
    [SerializeField]
    private int _selectTowerLevel;

    private TowerBuilder _towerBuilder;
    private Image _towerImage;
    private Image[] _levelImages;

    private int _towerIndex;
    private int _level;
    private TowerColor.ColorType _colorType;

    
    private void Awake()
    {
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _towerImage = GetComponent<Image>();
        _levelImages = _levelLayout.GetComponentsInChildren<Image>(true);

        Setup();
    }
    

    // ó�� �����Ǹ� Ÿ���� ����, ����, ����Ÿ���� �������� �����Ѵ�.
    public void Setup()
    {
        if (_selectTowerType == SelectTowerType.���þ���)
            _towerIndex = Random.Range(0, Tower.towerTypeNames.Length); // 0�̻� Ÿ��Ÿ���� ���� �̸��� �� �߿��� �������� ����.
        else
            _towerIndex = (int)_selectTowerType - 1;

        if (_selectTowerLevel == 0)
            _level = Random.Range(0, 4); // 0�̻� 4�̸��� �� �߿��� �������� ����.
        else
            _level = _selectTowerLevel - 1;

        if (_selectTowerColor == SelectTowerColor.���þ���)
            _colorType = (TowerColor.ColorType)Random.Range((int)TowerColor.ColorType.Red, (int)TowerColor.ColorType.Blue + 1); // Red, Green, Blue �߿��� �������� ����.
        else
            _colorType = (TowerColor.ColorType)_selectTowerColor; // Red, Green, Blue �߿��� �������� ����.

        SetTowerRenderer();
        SetTowerLevelImage();
    }
    private void SetTowerRenderer()
    {
        _towerImage.sprite = _towerBuilder.towerSprites[_towerIndex];

        switch (_colorType)
        {
            case TowerColor.ColorType.Red:
                _towerImage.color = Color.red;
                break;

            case TowerColor.ColorType.Green:
                _towerImage.color = Color.green;
                break;

            case TowerColor.ColorType.Blue:
                _towerImage.color = Color.blue;
                break;

            default:
                _towerImage.color = Color.white;
                break;
        }
    }

    private void SetTowerLevelImage()
    {
        for (int i = 0; i < _level; i++)
            _levelImages[i].gameObject.SetActive(true);
    }

    public bool IsCompareTower(Tower tower)
    {
        if (_towerIndex == tower.towerIndex && _level == tower.level && _colorType == tower.towerColor.colorType)
            return true;
        else
            return false;
    }

    public bool IsCompareTower(MissionTower missionTower)
    {
        if (_towerIndex == missionTower._towerIndex && _level == missionTower._level && _colorType == missionTower._colorType)
            return true;
        else
            return false;
    }
}
