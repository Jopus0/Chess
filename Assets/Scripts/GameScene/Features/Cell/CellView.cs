using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private Image _cellImage;
    [SerializeField] private Image _pieceImage;
    [SerializeField] private Image _outlineImage;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _columnIndexText;
    [SerializeField] private TextMeshProUGUI _rowIndexText;
    [SerializeField] private float _highlightFactor;

    private Color _cellBaseColor;
    private Color _cellHighlightColor;

    private Color _moveOutlineColor;
    private Color _attackOutlineColor;

    private Sprite _moveOutlineSprite;
    private Sprite _attackOutlineSprite;

    public void Init()
    {
        _pieceImage.gameObject.SetActive(false);
        _outlineImage.gameObject.SetActive(false);
    }
    public void SetPosition(Vector2 position, float size)
    {
        _rectTransform.sizeDelta = new Vector2(size, size);
        _collider.size = new Vector2(size, size);
        _rectTransform.anchorMin = new Vector2(position.x, position.y);
        _rectTransform.anchorMax = new Vector2(position.x, position.y);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
    public void SetIndices(string columnIndex, string rowIndex, Color indexColor)
    {
        ChangeIndex(_columnIndexText, columnIndex, indexColor);
        ChangeIndex(_rowIndexText, rowIndex, indexColor);
    }
    private void ChangeIndex(TextMeshProUGUI indexText, string index, Color indexColor)
    {
        if (index == null)
        {
            indexText.gameObject.SetActive(false);
            return;
        }

        indexText.text = index;
        indexText.color = indexColor;
    }
    public void SetColor(Color color)
    {
        _cellImage.color = color;

        _cellBaseColor = color;
        _cellHighlightColor = MakeLighterColor(color);
    }
    public void ChangePiece(Sprite pieceSprite)
    {
        if (pieceSprite == null)
        {
            _pieceImage.gameObject.SetActive(false);
        }
        else
        {
            _pieceImage.gameObject.SetActive(true);
            _pieceImage.sprite = pieceSprite;
        }
    }
    public void ChangeOutline(bool showOrClose, bool moveOrAttack)
    {
        if (!showOrClose)
        {
            _outlineImage.gameObject.SetActive(false);
        }
        else
        {
            _outlineImage.gameObject.SetActive(true);
            if (moveOrAttack)
            {
                _outlineImage.sprite = _moveOutlineSprite;
                _outlineImage.color = _moveOutlineColor;
            }
            else
            {
                _outlineImage.sprite = _attackOutlineSprite;
                _outlineImage.color = _attackOutlineColor;
            }
        }
    }
    public void SetOutline(Sprite moveOutline, Sprite attackOutline)
    {
        _moveOutlineSprite = moveOutline;
        _attackOutlineSprite = attackOutline;
    }
    public void SetOutlineColor(Color moveOutlineColor, Color attackOutlineColor)
    {
        _moveOutlineColor = moveOutlineColor;
        _attackOutlineColor = attackOutlineColor;
    }
    private Color MakeLighterColor(Color color)
    {
        return new Color(
            Mathf.Clamp01(color.r * _highlightFactor),
            Mathf.Clamp01(color.g * _highlightFactor),
            Mathf.Clamp01(color.b * _highlightFactor),
            color.a
        );
    }

    public void OnHighlight(bool isHighlighted)
    {
        if(isHighlighted)
        {
            _cellImage.color = _cellHighlightColor;
        }
        else
        {
            _cellImage.color = _cellBaseColor;
        }
    }
}
