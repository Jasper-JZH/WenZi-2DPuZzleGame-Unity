using System;

namespace Lighting2D.Editor
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class EditorButtonAttribute : Attribute
	{
		public string Label { get; private set; }

		public EditorButtonAttribute(string label = "")
		{
			Label = label;
		}
	}
}
