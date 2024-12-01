// @plugin: class: Field
using System;

public class Field
{
	public Field(string parameter)
	{
		this.parameter = parameter;
	}

	public override string ToString()
	{
		return this.parameter;
	}

	private readonly string parameter;
}
