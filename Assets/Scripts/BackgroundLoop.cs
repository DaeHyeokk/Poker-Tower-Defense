using UnityEngine;

// ���� ������ �̵��� ����� ������ ������ ���ġ�ϴ� ��ũ��Ʈ
public class BackgroundLoop : MonoBehaviour
{
    [SerializeField]
    private float width; // ����� ���� ����

    private void Update()
    {
        // ���� ��ġ�� �������� �������� width �̻� �̵������� ��ġ�� ����
        if (transform.position.x <= -width)
            Reposition();
    }

    // ��ġ�� �����ϴ� �޼���
    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}