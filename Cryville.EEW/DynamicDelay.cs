using Cryville.Common.Compat;
using System;

namespace Cryville.EEW {
	/// <summary>
	/// A helper class that produces dynamic delay values.
	/// </summary>
	public class DynamicDelay {
		/// <summary>
		/// The current phase.
		/// </summary>
		public double CurrentPhase { get; private set; }

		readonly double _baseDelay;
		readonly double _maxDelay;
		readonly double _delayMultiplier;

		/// <summary>
		/// Creates an instance of the <see cref="DynamicDelay" /> class.
		/// </summary>
		/// <param name="baseDelay">The minimum delay.</param>
		/// <param name="maxDelay">The maximum delay.</param>
		/// <param name="delayMultiplier">The multiplier between adjacent delay values.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="baseDelay" /> is negative or zero. -or- <paramref name="maxDelay" /> is negative or zero. -or- <paramref name="delayMultiplier" /> is less than or equal to 1.</exception>
		public DynamicDelay(double baseDelay = 1, double maxDelay = 1440, double delayMultiplier = 3.4641) {
			ThrowHelper.ThrowIfLessThanOrEqual(baseDelay, 0);
			ThrowHelper.ThrowIfLessThanOrEqual(maxDelay, 0);
			ThrowHelper.ThrowIfLessThanOrEqual(delayMultiplier, 1);

			_baseDelay = baseDelay;
			_maxDelay = maxDelay;
			_delayMultiplier = delayMultiplier;
		}

		/// <summary>
		/// Increments the current phase by the specified amount.
		/// </summary>
		/// <param name="amount">The amount of phase to increment.</param>
		/// <returns>The next delay value.</returns>
		public double IncrementPhase(double amount = 1) {
			double nextPhase = CurrentPhase + amount;
			double delay = _baseDelay;
			for (double cd = _baseDelay; cd <= _maxDelay; cd *= _delayMultiplier) {
				if ((int)(nextPhase / cd) > (int)(CurrentPhase / cd)) {
					delay = cd * _delayMultiplier;
				}
			}
			CurrentPhase = nextPhase;
			if (delay > _maxDelay)
				return _maxDelay;
			return delay;
		}
	}
}
