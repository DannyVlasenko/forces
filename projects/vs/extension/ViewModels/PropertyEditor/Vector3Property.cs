using System.ComponentModel;
using System;
using System.Numerics;

namespace Forces.ViewModels.PropertyEditor
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Serializable]
	public struct Vector3Property
	{
		private Func<Vector3> _getter;
		private Action<Vector3> _setter;

		public Vector3Property(Func<Vector3> getter, Action<Vector3> setter)
		{
			_getter = getter;
			_setter = setter;
		}

		public float X
		{
			get => _getter().X;
			set
			{
				var old = _getter();
				old.X = value;
				_setter(old);
			}
		}
		public float Y
		{
			get => _getter().Y;
			set
			{
				var old = _getter();
				old.Y = value;
				_setter(old);
			}
		}
		public float Z
		{
			get => _getter().Z;
			set
			{
				var old = _getter();
				old.Z = value;
				_setter(old);
			}
		}

		public override string ToString()
		{
			return $"X:{X};Y:{Y};Z:{Z}";
		}
	}
}