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
        Vector3 offset = new Vector3(width * 2f, 0f, 0f);
        transform.position = transform.position + offset;
    }
}