using Cryville.EEW.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a worker that gets events from a source.
	/// </summary>
	public interface ISourceWorker : IDisposable, INamedComponent {
		/// <summary>
		/// Raised for each event received.
		/// </summary>
		event Handler<object?>? Received;
		/// <summary>
		/// Raised when the worker reports itself working normally.
		/// </summary>
		event Handler<Heartbeat>? Heartbeat;
		/// <summary>
		/// Raised when an error occurs in the worker.
		/// </summary>
		event Handler<Exception>? ErrorEmitted;
		/// <summary>
		/// Runs the worker.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		Task RunAsync(CancellationToken cancellationToken);
	}
	/// <summary>
	/// Represents a worker that gets events from a source.
	/// </summary>
	/// <typeparam name="T">The type of the events.</typeparam>
	public interface ISourceWorker<out T> : ISourceWorker where T : class {
		/// <summary>
		/// Raised for each event received.
		/// </summary>
		new event Handler<T?>? Received;

		event Handler<object?>? ISourceWorker.Received {
			add { Received += value; }
			remove { Received -= value; }
		}
	}
}
