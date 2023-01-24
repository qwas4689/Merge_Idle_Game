using TMPro;

public interface ITextUpdate
{
    /// <summary>
    /// 텍스트 업데이트
    /// </summary>
    /// <param name="text">바뀔 텍스트</param>
    /// <param name="constStr">바꿀 텍스트</param>
    /// <param name="num">숫자</param>
    public void UpdateText(TextMeshProUGUI text, string constStr, int? num);
}
