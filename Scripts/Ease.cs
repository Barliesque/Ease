using System;
using UnityEngine;

namespace Barliesque.Easing
{
	//--------------------------------------------------------------------------
	// A collection of easing methods for use in
	// procedural animations.  Several ways of accessing
	// the different styles are provided to enable
	// selection via Unity's inspector, or a variety
	// of direct call approaches.
	//
	// Author: David Barlia, based (mostly) upon the 
	// easing functions developed by Robert Penner.
	//
	// More Info:  http://robertpenner.com/easing/
	//--------------------------------------------------------------------------

	//TODO  Add easy conversion from EaseSpec to AnimationCurve, or static generation of AnimationCurve

	public enum EaseStyle : int
	{
		Linear = 0,
		Sinus,
		Quad,
		Cubic,
		Quart,
		Quint,
		Expo,
		Circ,
		Back,
		Bounce,
		Elastic,
		Yoyo,
		Spike
	}

	public enum EaseType : int
	{
		In,
		Out,
		InOut
	}

	[Serializable]
	public struct EaseSpec
	{
		public EaseStyle Style;
		public EaseType Type;

		public EaseSpec(EaseStyle style, EaseType type)
		{
			this.Style = style;
			this.Type = type;
		}

		public float Call(float t) => Ease.Call(this, t);
		public float Call(float from, float to, float t) => Ease.Call(this, from, to, t);
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class FlipCurveAttribute : Attribute { }

	//----------------------------------------------------------------

	static public class Ease
	{
		public delegate float EaseFunc(float from, float to, float t);

		public delegate float EaseFunc01(float t);

		delegate float EaseByType(EaseType type, float from, float to, float t);

		static EaseByType[] EaseCall = new EaseByType[]
		{
			Linear.ByType,
			Sinus.ByType,
			Quad.ByType,
			Cubic.ByType,
			Quart.ByType,
			Quint.ByType,
			Expo.ByType,
			Circ.ByType,
			Back.ByType,
			Bounce.ByType,
			Elastic.ByType,
			Yoyo.ByType,
			Spike.ByType
		};

		delegate float EaseByType01(EaseType type, float t);

		static EaseByType01[] EaseCall01 = new EaseByType01[]
		{
			Linear.ByType,
			Sinus.ByType,
			Quad.ByType,
			Cubic.ByType,
			Quart.ByType,
			Quint.ByType,
			Expo.ByType,
			Circ.ByType,
			Back.ByType,
			Bounce.ByType,
			Elastic.ByType,
			Yoyo.ByType,
			Spike.ByType
		};

		//TODO  Add a Remap() function:  (EaseType/Style, from, to, current) --where t is calculated as a linear position between to and from, before the ease recalculates the value -- or maybe just use Mathf.InverseLerp to find t

		//----------------------------------------------------------------

		static public float Call(EaseStyle style, EaseType type, float from, float to, float t)
		{
			return EaseCall[(int)style](type, from, to, t);
		}

		static public float Call(EaseStyle style, EaseType type, float t)
		{
			return EaseCall01[(int)style](type, t);
		}

		static public float Call(EaseSpec spec, float t)
		{
			return Call(spec.Style, spec.Type, t);
		}

		static public float Call(EaseSpec spec, float from, float to, float t)
		{
			return Call(spec.Style, spec.Type, from, to, t);
		}

		//----------------------------------------------------------------

		static public class Linear
		{
			static public float In(float t)
			{
				return t;
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * t + from;
			}

			static public float Out(float t)
			{
				return t;
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * t + from;
			}

			static public float InOut(float t)
			{
				return t;
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * t + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				return (to - from) * t + from;
			}

			static public float ByType(EaseType type, float t)
			{
				return t;
			}
		}

		//----------------------------------------------------------------

		static public class Cubic
		{
			static public float In(float t)
			{
				return t * t * t;
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * t * t * t + from;
			}

			static public float Out(float t)
			{
				--t;
				return (t * t * t + 1f);
			}

			static public float Out(float from, float to, float t)
			{
				--t;
				return (to - from) * (t * t * t + 1f) + from;
			}

			static public float InOut(float t)
			{
				t *= 2f;
				if (t < 1)
					return (0.5f * t * t * t);
				t -= 2;
				return 0.5f * (t * t * t + 2f);
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(from, to, t);
					case EaseType.Out: return Out(from, to, t);
					case EaseType.InOut: return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(t);
					case EaseType.Out: return Out(t);
					case EaseType.InOut: return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Quad
		{
			static public float In(float t)
			{
				return t * t;
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * (t * t) + from;
			}

			static public float Out(float t)
			{
				return t * (2.0f - t);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				t *= 2.0f;
				if (t < 1.0f)
					return t * t * 0.5f;
				t -= 1.0f;
				return 1f + 0.5f * (t * (2.0f - t) - 1.0f);
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Quint
		{
			static public float In(float t)
			{
				return t * t * t * t * t;
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * (t * t * t * t * t) + from;
			}

			static public float Out(float t)
			{
				--t;
				return t * t * t * t * t + 1.0f;
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				t *= 2.0f;
				if (t < 1.0f)
					return 0.5f * t * t * t * t * t;
				t -= 2.0f;
				return 0.5f * (t * t * t * t * t + 2.0f);
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Bounce
		{
			const float F1_275 = 1f / 2.75f;

			static public float In(float t)
			{
				return 1.0f - Out(1.0f - t);
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * Out(1.0f - t) + from;
			}

			static public float Out(float t)
			{
				if (t < (F1_275))
					return (7.5625f * t * t);
				else if (t < (2f * F1_275))
					return (7.5625f * (t -= (1.5f * F1_275)) * t + 0.75f);
				else if (t < (2.5 / 2.75))
					return (7.5625f * (t -= (2.25f * F1_275)) * t + 0.9375f);
				else
					return (7.5625f * (t -= (2.625f * F1_275)) * t + 0.984375f);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				if (t < 0.5f)
					return In(t * 2f) * 0.5f;
				else
					return Out(t * 2f - 1f) * 0.5f + 0.5f;
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Elastic
		{
			const float AMPLITUDE = 0f;
			const float PERIOD = 0.3f;
			const float PERIOD1_5 = PERIOD * 1.5f;
			const float PERIOD0_375 = PERIOD * 0.375f;
			const float PERIOD0_25 = PERIOD * 0.25f;
			const float TWO_PI = Mathf.PI * 2.0f;

			static public float In(float t)
			{
				if (t == 0.0f)
					return 0.0f;
				if (t == 1.0f)
					return 1.0f;
				--t;
				return -(Mathf.Pow(2.0f, 10.0f * t) * Mathf.Sin((t - PERIOD0_25) * TWO_PI / PERIOD));
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				return (Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t - PERIOD0_25) * TWO_PI / PERIOD) + 1.0f);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				if (t == 0) return 0f;
				if (t == 1) return 1f;

				if (t < 0.5f)
				{
					return In(t + t) * 0.5f;
				}

				return Out(t + t - 1f) * 0.5f + 0.5f;
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Back
		{
			const float OVERSHOOT = 1.70158f;

			static public float In(float t)
			{
				return t * t * ((OVERSHOOT + 1.0f) * t - OVERSHOOT);
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				t -= 1f;
				return (t * t * ((OVERSHOOT + 1.0f) * t + OVERSHOOT) + 1.0f);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				t *= 2.0f;
				var s = OVERSHOOT * 1.525f;
				if (t < 1.0f)
					return 0.5f * (t * t * ((s + 1.0f) * t - s));
				t -= 2.0f;
				return 0.5f * (t * t * ((s + 1.0f) * t + s) + 2.0f);
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Sinus
		{
			const float HALF_PI = Mathf.PI * 0.5f;

			static public float In(float t)
			{
				return 1.0f - Mathf.Cos(t * HALF_PI);
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				return Mathf.Sin(t * HALF_PI);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				return 0.5f * (1.0f - Mathf.Cos(Mathf.PI * t));
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Circ
		{
			static public float In(float t)
			{
				return 1f - Mathf.Sqrt(1f - t * t);
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				t = 1f - t;
				return Mathf.Sqrt(1f - t * t);
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				t *= 2f;
				if (t < 1f)
				{
					return (1f - Mathf.Sqrt(1f - t * t)) * 0.5f;
				}
				else
				{
					return (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) * 0.5f;
				}
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//.....................................................................

		static public class Expo
		{
			static public float In(float t)
			{
				return (t == 0f) ? 0f : Mathf.Pow(2f, 10f * (t - 1f));
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				return (t == 1f) ? 1f : -Mathf.Pow(2f, -10 * t) + 1f;
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				t *= 2f;
				if (t < 1f)
				{
					return (t == 0) ? 0f : (0.5f * Mathf.Pow(2f, 10f * (t - 1f)));
				}
				else
				{
					return (t == 2) ? 1f : (0.5f * (2f - Mathf.Pow(2f, -10f * (--t))));
				}
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//----------------------------------------------------------------

		static public class Quart
		{
			static public float In(float t)
			{
				return t * t * t * t;
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * t * t * t * t + from;
			}

			static public float Out(float t)
			{
				--t;
				return 2f - (t * t * t * t + 1f);
			}

			static public float Out(float from, float to, float t)
			{
				--t;
				return (from - to) * (t * t * t * t + 1f) + from;
			}

			static public float InOut(float t)
			{
				t *= 2f;
				if (t < 1)
					return (0.5f * t * t * t * t);
				t -= 2;
				return 2f - 0.5f * (t * t * t * t + 2f);
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(from, to, t);
					case EaseType.Out: return Out(from, to, t);
					case EaseType.InOut: return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(t);
					case EaseType.Out: return Out(t);
					case EaseType.InOut: return InOut(t);
				}

				return 0f;
			}
		}

		//----------------------------------------------------------------

		static public class Yoyo
		{
			const float _offsetLong = 17f / 24f;
			const float _offsetShort = 1f - _offsetLong;

			const float _invLong = 1f / _offsetLong;
			const float _invShort = 1f / _offsetShort;

			static public float In(float t)
			{
				if (t < _offsetLong)
				{
					return (1f - Mathf.Cos(t * _invLong * Mathf.PI)) * 0.5f;
				}
				else
				{
					return Mathf.Cos((t - _offsetLong) * _invShort * Mathf.PI * 0.5f);
				}
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				if (t < _offsetShort)
				{
					return Mathf.Sin(t * _invShort * Mathf.PI * 0.5f);
				}
				else
				{
					return (1f + Mathf.Cos((t - _offsetShort) * _invLong * Mathf.PI)) * 0.5f;
				}
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				return (1f - Mathf.Cos(t * Mathf.PI * 2f)) * 0.5f;
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(from, to, t);
					case EaseType.Out:
						return Out(from, to, t);
					case EaseType.InOut:
						return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In:
						return In(t);
					case EaseType.Out:
						return Out(t);
					case EaseType.InOut:
						return InOut(t);
				}

				return 0f;
			}
		}

		//----------------------------------------------------------------

		static public class Spike
		{
			static public float In(float t)
			{
				if (t < 0.5f)
				{
					t *= 2f;
					return t * t * t;
				}
				else
				{
					t = t * 2f - 1f;
					return 1f - t * t * t;
				}
			}

			static public float In(float from, float to, float t)
			{
				return (to - from) * In(t) + from;
			}

			static public float Out(float t)
			{
				if (t < 0.5f)
				{
					t = 1f - t * 2f;
					return 1f - t * t * t;
				}
				else
				{
					t = 1f - (t * 2f - 1f);
					return t * t * t;
				}
			}

			static public float Out(float from, float to, float t)
			{
				return (to - from) * Out(t) + from;
			}

			static public float InOut(float t)
			{
				if (t < 0.5f)
				{
					t *= 2f;
					return t * t * t;
				}
				else
				{
					t = 1f - (t * 2f - 1f);
					return t * t * t;
				}
			}

			static public float InOut(float from, float to, float t)
			{
				return (to - from) * InOut(t) + from;
			}

			static public float ByType(EaseType type, float from, float to, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(from, to, t);
					case EaseType.Out: return Out(from, to, t);
					case EaseType.InOut: return InOut(from, to, t);
				}

				return from;
			}

			static public float ByType(EaseType type, float t)
			{
				switch (type)
				{
					case EaseType.In: return In(t);
					case EaseType.Out: return Out(t);
					case EaseType.InOut: return InOut(t);
				}

				return 0f;
			}
		}


		//.....................................................................
	}
}