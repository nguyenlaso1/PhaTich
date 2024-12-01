// @sonhg: class: PlaceBombCommand
using System;

public class PlaceBombCommand : BaseInputCommand
{
	public override void Execute(BaseCharactersController actor)
	{
		if (actor is IPlaceBomb)
		{
			(actor as IPlaceBomb).PlaceBomb();
		}
	}
}
