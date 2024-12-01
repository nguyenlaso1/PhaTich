// @sonhg: class: FunctionExtension
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class FunctionExtension
{
	public static void ChangeColorRecursive(this Transform parent, Color color)
	{
		SpriteRenderer[] componentsInChildren = parent.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.color = color;
		}
	}

	public static Color TryParseColorString(this string colorString)
	{
		return Color.white;
	}

	public static void SetAlpha(this Image image, float alpha)
	{
		Color color = new Color(image.color.r, image.color.g, image.color.b, alpha);
		image.color = color;
	}

	public static void SetAlpha(this SpriteRenderer render, float alpha)
	{
		Color color = new Color(render.color.r, render.color.g, render.color.b, alpha);
		render.color = color;
	}

	public static Tween DoAlpha(this CanvasGroup canvas, float endValue, float duration)
	{
		return DOTween.To(() => canvas.alpha, delegate(float alpha)
		{
			canvas.alpha = alpha;
		}, endValue, duration);
	}

	public static Tween DoAlpha(this Image image, float endValue, float duration)
	{
		return DOTween.To(() => image.color.a, delegate(float alpha)
		{
			image.SetAlpha(alpha);
		}, endValue, duration);
	}

	public static Tween DoAlpha(this SpriteRenderer render, float endValue, float duration)
	{
		return DOTween.To(() => render.color.a, delegate(float alpha)
		{
			render.SetAlpha(alpha);
		}, endValue, duration);
	}

	public static Tween DoColor(this Text text, Color endValue, float duration)
	{
		return DOTween.To(() => text.color, delegate(Color color)
		{
			text.color = color;
		}, endValue, duration);
	}

	public static Tween DoColor(this Image image, Color endValue, float duration)
	{
		return DOTween.To(() => image.color, delegate(Color color)
		{
			image.color = color;
		}, endValue, duration);
	}

	public static T GetRandomElement<T>(this List<T> list)
	{
		int index = UnityEngine.Random.Range(0, list.Count);
		if (list.Count > 0)
		{
			return list[index];
		}
		return default(T);
	}

	public static MoveDirection Reverse(this MoveDirection direction)
	{
		switch (direction)
		{
		case MoveDirection.RIGHT:
			return MoveDirection.LEFT;
		case MoveDirection.LEFT:
			return MoveDirection.RIGHT;
		case MoveDirection.DOWN:
			return MoveDirection.UP;
		case MoveDirection.UP:
			return MoveDirection.DOWN;
		default:
			return direction;
		}
	}

	public static Vector3 GetDircetionVector(this MoveDirection direction)
	{
		Vector3 result;
		switch (direction)
		{
		case MoveDirection.RIGHT:
			result = Vector3.right;
			break;
		case MoveDirection.LEFT:
			result = Vector3.left;
			break;
		case MoveDirection.DOWN:
			result = Vector3.down;
			break;
		case MoveDirection.UP:
			result = Vector3.up;
			break;
		default:
			result = Vector3.zero;
			break;
		}
		return result;
	}
}
