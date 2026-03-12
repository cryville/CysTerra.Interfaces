using System;

namespace Cryville.Measure {
	/// <summary>
	/// Provides a set of predefined units.
	/// </summary>
	public static class Units {
		/// <summary>
		/// Dimensionless unit (1).
		/// </summary>
		public static readonly Unit Dimensionless = new(Dimensions.Dimensionless);

		/// <summary>
		/// second (s).
		/// </summary>
		public static readonly Unit Second = new(Dimensions.Time);
		/// <summary>
		/// metre (m).
		/// </summary>
		public static readonly Unit Metre = new(Dimensions.Length);
		/// <summary>
		/// kilogram (kg).
		/// </summary>
		public static readonly Unit Kilogram = new(Dimensions.Mass);
		/// <summary>
		/// gram (g).
		/// </summary>
		public static readonly Unit Gram = new(Dimensions.Mass, 1e-3);
		/// <summary>
		/// ampere (A).
		/// </summary>
		public static readonly Unit Ampere = new(Dimensions.ElectricCurrent);
		/// <summary>
		/// kelvin (K).
		/// </summary>
		public static readonly Unit Kelvin = new(Dimensions.ThermodynamicTemperature);
		/// <summary>
		/// mole (mol).
		/// </summary>
		public static readonly Unit Mole = new(Dimensions.AmountOfSubstance);
		/// <summary>
		/// candela (cd).
		/// </summary>
		public static readonly Unit Candela = new(Dimensions.LuminousIntensity);

		/// <summary>
		/// hertz (Hz).
		/// </summary>
		public static readonly Unit Hertz = new(Dimensions.Frequency);
		/// <summary>
		/// newton (N).
		/// </summary>
		public static readonly Unit Newton = new(Dimensions.Force);
		/// <summary>
		/// pascal (Pa).
		/// </summary>
		public static readonly Unit Pascal = new(Dimensions.Pressure);
		/// <summary>
		/// joule (J).
		/// </summary>
		public static readonly Unit Joule = new(Dimensions.Energy);
		/// <summary>
		/// watt (W).
		/// </summary>
		public static readonly Unit Watt = new(Dimensions.Power);
		/// <summary>
		/// coulomb (C).
		/// </summary>
		public static readonly Unit Coulomb = new(Dimensions.ElectricCharge);
		/// <summary>
		/// volt (V).
		/// </summary>
		public static readonly Unit Volt = new(Dimensions.ElectricPotentialDifference);
		/// <summary>
		/// farad (F).
		/// </summary>
		public static readonly Unit Farad = new(Dimensions.Capacitance);
		/// <summary>
		/// ohm (Ω).
		/// </summary>
		public static readonly Unit Ohm = new(Dimensions.ElectricalResistance);
		/// <summary>
		/// siemens (S).
		/// </summary>
		public static readonly Unit Siemens = new(Dimensions.ElectricalConductance);
		/// <summary>
		/// weber (Wb).
		/// </summary>
		public static readonly Unit Weber = new(Dimensions.MagneticFlux);
		/// <summary>
		/// tesla (T).
		/// </summary>
		public static readonly Unit Tesla = new(Dimensions.MagneticFluxDensity);
		/// <summary>
		/// henry (H).
		/// </summary>
		public static readonly Unit Henry = new(Dimensions.Inductance);
		/// <summary>
		/// degree Celsius (°C).
		/// </summary>
		public static readonly OffsetUnit DegreeCelsius = new(Dimensions.ThermodynamicTemperature, 1, 273.15);
		/// <summary>
		/// lumen (lm).
		/// </summary>
		public static readonly Unit Lumen = new(Dimensions.LuminousIntensity);
		/// <summary>
		/// lux (lx).
		/// </summary>
		public static readonly Unit Lux = new(Dimensions.Illuminance);
		/// <summary>
		/// becquerel (Bq).
		/// </summary>
		public static readonly Unit Becquerel = new(Dimensions.Frequency);
		/// <summary>
		/// gray (Gy).
		/// </summary>
		public static readonly Unit Gray = new(Dimensions.Dose);
		/// <summary>
		/// sievert (Sv).
		/// </summary>
		public static readonly Unit Sievert = new(Dimensions.Dose);
		/// <summary>
		/// katal (kat).
		/// </summary>
		public static readonly Unit Katal = new(Dimensions.CatalyticActivity);

		/// <summary>
		/// minute (min).
		/// </summary>
		public static readonly Unit Minute = new(Dimensions.Time, 60);
		/// <summary>
		/// hour (h).
		/// </summary>
		public static readonly Unit Hour = new(Dimensions.Time, 3600);
		/// <summary>
		/// day (d).
		/// </summary>
		public static readonly Unit Day = new(Dimensions.Time, 86400);
		/// <summary>
		/// astronomical unit (au).
		/// </summary>
		public static readonly Unit AstronomicalUnit = new(Dimensions.Length, 149597870700);
		/// <summary>
		/// degree (°).
		/// </summary>
		public static readonly Unit Degree = new(Dimensions.Dimensionless, Math.PI / 180);
		/// <summary>
		/// arcminute (′).
		/// </summary>
		public static readonly Unit Arcminute = new(Dimensions.Dimensionless, Math.PI / 10800);
		/// <summary>
		/// arcsecond (″).
		/// </summary>
		public static readonly Unit Arcsecond = new(Dimensions.Dimensionless, Math.PI / 648000);
		/// <summary>
		/// hectare (ha).
		/// </summary>
		public static readonly Unit Hectare = new(Dimensions.Area, 10000);
		/// <summary>
		/// litre (l, L).
		/// </summary>
		public static readonly Unit Litre = new(Dimensions.Volume, 0.001);
		/// <summary>
		/// tonne (t).
		/// </summary>
		public static readonly Unit Tonne = new(Dimensions.Mass, 1000);
		/// <summary>
		/// dalton (Da).
		/// </summary>
		public static readonly Unit Dalton = new(Dimensions.Mass, 1.66053906892e-27);
		/// <summary>
		/// electronvolt (eV).
		/// </summary>
		public static readonly Unit Electronvolt = new(Dimensions.Energy, 1.602176634e-19);
		/// <summary>
		/// neper (amplitude quantity) (Np).
		/// </summary>
		public static readonly LogarithmicUnit NeperAmplitude = new(Dimensions.Dimensionless);
		/// <summary>
		/// neper (power quantity) (Np).
		/// </summary>
		public static readonly LogarithmicUnit NeperPower = new(Dimensions.Dimensionless, 0.5);
		/// <summary>
		/// bel (amplitude quantity) (B).
		/// </summary>
		public static readonly LogarithmicUnit BelAmplitude = new(Dimensions.Dimensionless, Math.Log(10));
		/// <summary>
		/// bel (power quantity) (B).
		/// </summary>
		public static readonly LogarithmicUnit BelPower = new(Dimensions.Dimensionless, 0.5 * Math.Log(10));
		/// <summary>
		/// decibel (amplitude quantity) (dB).
		/// </summary>
		public static readonly LogarithmicUnit DecibelAmplitude = (LogarithmicUnit)BelAmplitude.WithPrefix(MetricPrefixes.Deci);
		/// <summary>
		/// decibel (power quantity) (dB).
		/// </summary>
		public static readonly LogarithmicUnit DecibelPower = (LogarithmicUnit)BelPower.WithPrefix(MetricPrefixes.Deci);
	}
}
