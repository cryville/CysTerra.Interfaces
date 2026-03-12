namespace Cryville.Measure {
	/// <summary>
	/// Provides a set of predefined dimensions.
	/// </summary>
	public static class Dimensions {
		/// <summary>
		/// Dimension one.
		/// </summary>
		public static readonly Dimension Dimensionless;

		/// <summary>
		/// Time (T).
		/// </summary>
		public static readonly Dimension Time = new() { Time = 1 };
		/// <summary>
		/// Length (L).
		/// </summary>
		public static readonly Dimension Length = new() { Length = 1 };
		/// <summary>
		/// Mass (M).
		/// </summary>
		public static readonly Dimension Mass = new() { Mass = 1 };
		/// <summary>
		/// Electric current (I).
		/// </summary>
		public static readonly Dimension ElectricCurrent = new() { ElectricCurrent = 1 };
		/// <summary>
		/// Thermodynamic temperature (Θ).
		/// </summary>
		public static readonly Dimension ThermodynamicTemperature = new() { ThermodynamicTemperature = 1 };
		/// <summary>
		/// Amount of substance (N).
		/// </summary>
		public static readonly Dimension AmountOfSubstance = new() { AmountOfSubstance = 1 };
		/// <summary>
		/// Luminous intensity (J).
		/// </summary>
		public static readonly Dimension LuminousIntensity = new() { LuminousIntensity = 1 };

		/// <summary>
		/// Frequency (1/T).
		/// </summary>
		public static readonly Dimension Frequency = new() { Time = -1 };
		/// <summary>
		/// Force (M·L/T^2).
		/// </summary>
		public static readonly Dimension Force = new() { Time = -2, Length = 1, Mass = 1 };
		/// <summary>
		/// Pressure (M/L/T^2).
		/// </summary>
		public static readonly Dimension Pressure = new() { Time = -2, Length = -1, Mass = 1 };
		/// <summary>
		/// Energy (M·L^2/T^2).
		/// </summary>
		public static readonly Dimension Energy = new() { Time = -2, Length = 2, Mass = 1 };
		/// <summary>
		/// Power (M·L^2/T^3).
		/// </summary>
		public static readonly Dimension Power = new() { Time = -3, Length = 2, Mass = 1 };
		/// <summary>
		/// Electric charge (T·I).
		/// </summary>
		public static readonly Dimension ElectricCharge = new() { Time = 1, ElectricCurrent = 1 };
		/// <summary>
		/// Electric potential difference (M·L^2/T^3/I).
		/// </summary>
		public static readonly Dimension ElectricPotentialDifference = new() { Time = -3, Length = 2, Mass = 1, ElectricCurrent = -1 };
		/// <summary>
		/// Capacitance (T^4·I^2/M/L^2).
		/// </summary>
		public static readonly Dimension Capacitance = new() { Time = 4, Length = -2, Mass = -1, ElectricCurrent = 2 };
		/// <summary>
		/// Electrical resistance (M·L^2/T^3/I^2).
		/// </summary>
		public static readonly Dimension ElectricalResistance = new() { Time = -3, Length = 2, Mass = 1, ElectricCurrent = -2 };
		/// <summary>
		/// Electrical conductance (T^3·I^2/M/L^2).
		/// </summary>
		public static readonly Dimension ElectricalConductance = new() { Time = 3, Length = -2, Mass = -1, ElectricCurrent = 2 };
		/// <summary>
		/// Magnetic flux (M·L^2/T^2/I).
		/// </summary>
		public static readonly Dimension MagneticFlux = new() { Time = -2, Length = 2, Mass = 1, ElectricCurrent = -1 };
		/// <summary>
		/// Magnetic flux density (M/T^2/I).
		/// </summary>
		public static readonly Dimension MagneticFluxDensity = new() { Time = -2, Mass = 1, ElectricCurrent = -1 };
		/// <summary>
		/// Inductance (M·L^2/T^2/I^2).
		/// </summary>
		public static readonly Dimension Inductance = new() { Time = -2, Length = 2, Mass = 1, ElectricCurrent = -2 };
		/// <summary>
		/// Illuminance (J/L^2).
		/// </summary>
		public static readonly Dimension Illuminance = new() { Length = -2, LuminousIntensity = 1 };
		/// <summary>
		/// Dose (L^2/T^2).
		/// </summary>
		public static readonly Dimension Dose = new() { Time = -2, Length = 2 };
		/// <summary>
		/// Catalytic activity (N/T).
		/// </summary>
		public static readonly Dimension CatalyticActivity = new() { Time = -1, AmountOfSubstance = 1 };

		/// <summary>
		/// Area (L^2).
		/// </summary>
		public static readonly Dimension Area = new() { Length = 2 };
		/// <summary>
		/// Volume (L^3).
		/// </summary>
		public static readonly Dimension Volume = new() { Length = 3 };
	}
}
