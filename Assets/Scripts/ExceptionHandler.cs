// @sonhg: class: ExceptionHandler
using System;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
	public bool logBug;

	private StreamWriter _writer;

	private int _exceptionCount;
}
