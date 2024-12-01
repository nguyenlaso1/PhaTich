// @sonhg: class: GroupButton
using System;
using UnityEngine;

public class GroupButton : MonoBehaviour
{
	private void Start()
	{
	}

	public void SetButtonParams(int num, int depth, Context.OnDeletegateObject onGroupClick, object objectParam = null)
	{
		this.SetNumber(num);
		this.chipLabel.GetComponent<UILabel>().depth = depth + 1;
		this.uiSprite.depth = depth;
		this._onGroupClick = onGroupClick;
		this._objectParam = objectParam;
	}

	public void SetSprite(string sprite)
	{
		this.uiSprite.spriteName = sprite;
	}

	public void OnClickGroupButton()
	{
		this._onGroupClick(this._objectParam);
	}

	public void SetNumber(int num)
	{
		string text = string.Format("{0:#,##0.##}", num).Replace(',', '.');
		this.chipLabel.GetComponent<UILabel>().text = text;
	}

	private Context.OnDeletegateObject _onGroupClick;

	private object _objectParam;

	public GameObject chipLabel;

	public UISprite uiSprite;

	private bool _isShowPointChar;

	[SerializeField]
	private AnimationCurve easeCurve;

	[SerializeField]
	private float duration = 1f;
}
