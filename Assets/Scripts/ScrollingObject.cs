using UnityEngine;

// ���� ������Ʈ�� ��� �������� �����̴� ��ũ��Ʈ
public class ScrollingObject : MonoBehaviour
{

    private void Update()
    {
        // ���� ������Ʈ�� �������� ���� �ӵ��� ���� �̵��ϴ� ó��
        transform.Translate(Vector3.left * Time.deltaTime * 0.5f);
    }
}