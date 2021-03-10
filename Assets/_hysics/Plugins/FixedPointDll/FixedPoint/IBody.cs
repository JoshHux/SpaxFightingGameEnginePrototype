using System;

namespace Spax
{
	public interface IBody
	{
		bool FPDisabled
		{
			get;
			set;
		}

		string Checkum();
	}
}
