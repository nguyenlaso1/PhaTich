// @plugin: class: ExceptionHitBuilder
using System;

public class ExceptionHitBuilder : HitBuilder<ExceptionHitBuilder>
{
	public string GetExceptionDescription()
	{
		return this.exceptionDescription;
	}

	public ExceptionHitBuilder SetExceptionDescription(string exceptionDescription)
	{
		if (exceptionDescription != null)
		{
			this.exceptionDescription = exceptionDescription;
		}
		return this;
	}

	public bool IsFatal()
	{
		return this.fatal;
	}

	public ExceptionHitBuilder SetFatal(bool fatal)
	{
		this.fatal = fatal;
		return this;
	}

	public override ExceptionHitBuilder GetThis()
	{
		return this;
	}

	public override ExceptionHitBuilder Validate()
	{
		return this;
	}

	private string exceptionDescription = string.Empty;

	private bool fatal;
}
