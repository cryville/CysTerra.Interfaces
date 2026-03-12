using System;
using System.Collections.Generic;

namespace Cryville.EEW.Report {
	/// <summary>
	/// A utility list that caches the states of the report units.
	/// </summary>
	public class ReportUnitStateList {
		/// <summary>
		/// The maximum time for the report units to stay alive in the list.
		/// </summary>
		public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(10);

		readonly Dictionary<string, int[]> _states = [];
		readonly Queue<(string, DateTime)> _timestamps = [];

		/// <summary>
		/// Pushes the states of a report unit into the list.
		/// </summary>
		/// <param name="id">The ID of the report unit.</param>
		/// <param name="state">The states.</param>
		/// <returns>The result of the push.</returns>
		/// <exception cref="ArgumentException">The length of <paramref name="state" /> mismatches with the length of the states pushed last time.</exception>
		public PushResult Push(string id, ReadOnlySpan<int> state) {
			lock (_states) {
				while (_timestamps.TryPeek(out var ts) && DateTime.UtcNow - ts.Item2 >= Timeout) {
					_timestamps.Dequeue();
					_states.Remove(ts.Item1);
				}
				if (!_states.TryGetValue(id, out var state0)) {
					_states.Add(id, [.. state]);
					_timestamps.Enqueue((id, DateTime.UtcNow));
					return new(true, true, state);
				}
				if (state.Length != state0.Length) throw new ArgumentException("State length mismatch");
				bool newMaxStateFlag = false;
				for (int i = 0; i < state.Length; i++) {
					int s0 = state0[i], s1 = state[i];
					if (s1 > s0) {
						newMaxStateFlag = true;
						state0[i] = s1;
					}
				}
				return new(newMaxStateFlag, false, state0);
			}
		}

		/// <summary>
		/// Manually remove a report unit from the list.
		/// </summary>
		/// <param name="id">The ID of the report unit.</param>
		/// <returns>Whether the report unit is removed successfully.</returns>
		public bool Invalidate(string id) { lock (_states) return _states.Remove(id); }
	}

	/// <summary>
	/// The result of a push action to an instance of the <see cref="ReportUnitStateList" /> class.
	/// </summary>
	public readonly ref struct PushResult {
		/// <summary>
		/// Whether the push updates the maximum states.
		/// </summary>
		public bool HasNewMaxState { get; }
		/// <summary>
		/// Whether the push adds a new report unit into the list.
		/// </summary>
		public bool IsNewId { get; }
		/// <summary>
		/// The current maximum states.
		/// </summary>
		public ReadOnlySpan<int> MaxState { get; }

		internal PushResult(bool hasNewMaxState, bool isNewId, ReadOnlySpan<int> maxState) {
			HasNewMaxState = hasNewMaxState;
			IsNewId = isNewId;
			MaxState = maxState;
		}
	}
}
