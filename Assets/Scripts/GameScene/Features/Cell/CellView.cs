using System;
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
    private Color _cellCurrentColor;
    private Color _cellHighlightColor;

    public void Init()
    {
        _pieceImage.gameObject.SetActive(false);
        _outlineImage.gameObject.SetActive(false);
        _rowIndexText.gameObject.SetActive(false);
        _columnIndexText.gameObject.SetActive(false);
    }
    public void ChangePiece(Sprite pieceSprite)
    {
        if (pieceSprite == null)
        {
            _pieceImage.gameObject.SetActive(false);
        }
        else
        {
            _pieceImage.sprite = pieceSprite;
            _pieceImage.gameObject.SetActive(true);
        }
    }
    public void SetPosition(Vector2 position, float size)
    {
        _rectTransform.sizeDelta = new Vector2(size, size);
        _collider.size = new Vector2(size, size);
        _rectTransform.anchorMin = new Vector2(position.x, position.y);
        _rectTransform.anchorMax = new Vector2(position.x, position.y);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
    public void SetRowIndex(string index, Color indexColor)
    {
        _rowIndexText.gameObject.SetActive(true);
        _rowIndexText.text = index;
        _rowIndexText.color = indexColor;
    }
    public void SetColumnIndex(string index, Color indexColor)
    {
        _columnIndexText.gameObject.SetActive(true);
        _columnIndexText.text = index;
        _columnIndexText.color = indexColor;
    }
    public void SetBaseColor(Color color)
    {
        _cellBaseColor = color;
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        _cellCurrentColor = color;
        _cellImage.color = color;
        _cellHighlightColor = MakeLighterColor(color);
    }
    public void ClearColor()
    {
        SetColor(_cellBaseColor);
    }
    public void SetOutline(Sprite outlineSprite, Color outlineColor)
    {
        _outlineImage.gameObject.SetActive(true);
        _outlineImage.sprite = outlineSprite;
        _outlineImage.color = outlineColor;
    }
    public void ClearOutline()
    {
        _outlineImage.gameObject.SetActive(false);
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
    public void Highlight(bool enterOrExit)
    {
        if(enterOrExit)
            _cellImage.color = _cellHighlightColor;
        else
            _cellImage.color = _cellCurrentColor;
    }
}
