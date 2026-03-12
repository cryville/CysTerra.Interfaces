using Cryville.Common.Compat;
using System;

namespace Cryville.EEW {
	/// <summary>
	/// A helper class that produces progressive delay.
	/// </summary>
	public class ProgressiveDelay {
		double _nextDelay;
		/// <summary>
		/// The delay to next tick.
		/// </summary>
		public double CurrentDelay { get; private set; }

		readonly double _baseDelay;
		readonly double _maxDelay;
		readonly double _delayMultiplier;

		/// <summary>
		/// Creates an instance of the <see cref="ProgressiveDelay" /> class.
		/// </summary>
		/// <param name="baseDelay">The minimum delay.</param>
		/// <param name="maxDelay">The maximum delay.</param>
		/// <param name="delayMultiplier">The multiplier between adjacent delay values.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="baseDelay" /> is negative or zero. -or- <paramref name="maxDelay" /> is negative or zero. -or- <paramref name="delayMultiplier" /> is less than or equal to 1.</exception>
		public ProgressiveDelay(double baseDelay = 1, double maxDelay = 1440, double delayMultiplier = 3.4641) {
			ThrowHelper.ThrowIfLessThanOrEqual(baseDelay, 0);
			ThrowHelper.ThrowIfLessThanOrEqual(maxDelay, 0);
			ThrowHelper.ThrowIfLessThanOrEqual(delayMultiplier, 1);

			_baseDelay = baseDelay;
			_maxDelay = maxDelay;
			_delayMultiplier = delayMultiplier;

			Reset();
		}

		/// <summary>
		/// Decrements the current delay and ticks if the delay has run over.
		/// </summary>
		/// <param name="amount">The amount of delay to decrement.</param>
		/// <returns>Whether to tick.</returns>
		public bool Step(double amount = 1) {
			CurrentDelay -= amount;
			if (CurrentDelay > 0)
				return false;
			CurrentDelay += _nextDelay;
			_nextDelay *= _delayMultiplier;
			if (_nextDelay > _maxDelay)
				_nextDelay = _maxDelay;
			return true;
		}

		/// <summary>
		/// Resets to the base delay.
		/// </summary>
		public void Reset() {
			CurrentDelay = _baseDelay;
			_nextDelay = _baseDelay * _delayMultiplier;
		}
	}
}
