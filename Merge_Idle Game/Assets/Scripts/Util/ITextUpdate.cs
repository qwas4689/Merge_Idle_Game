using TMPro;

public interface ITextUpdate
{
    /// <summary>
    /// �ؽ�Ʈ ������Ʈ
    /// </summary>
    /// <param name="text">�ٲ� �ؽ�Ʈ</param>
    /// <param name="constStr">�ٲ� �ؽ�Ʈ</param>
    /// <param name="num">����</param>
    public void UpdateText(TextMeshProUGUI text, string constStr, int? num);
}
