using UnityEngine;
using UnityEngine.UI;

public class RainbowButton : MonoBehaviour
{
    private Button buttonComponent;

    void Start()
    {
        // Text�R���|�[�l���g���擾
        buttonComponent = GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogError("�C���[�W���R���|�[�l���g����Ă���I�u�W�F�N�g��������܂���ł���");
        }
    }

    void Update()
    {
        if (buttonComponent != null)
        {
            // �F����F�ɕω�������
            buttonComponent.image.color = GetRainbowColor(Time.time);
        }
    }

    Color GetRainbowColor(float time)
    {
        // ���ԂɊ�Â��ĐF���v�Z
        float r = Mathf.Sin(time * 2f) * 0.5f + 0.5f;
        float g = Mathf.Sin(time * 2f + 2f * Mathf.PI / 3f) * 0.5f + 0.5f;
        float b = Mathf.Sin(time * 2f + 4f * Mathf.PI / 3f) * 0.5f + 0.5f;
        return new Color(r, g, b);
    }
}