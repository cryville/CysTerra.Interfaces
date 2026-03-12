using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Represents a component collector.
	/// </summary>
	public interface IComponentCollector : INamedComponent {
		/// <summary>
		/// Whether the collector collects singleton components automatically.
		/// </summary>
		bool IsAutomatic { get; }

		/// <summary>
		/// Raised when the list of builders is changed.
		/// </summary>
		event EventHandler? BuildersChanged;
		/// <summary>
		/// Collected builders.
		/// </summary>
		IEnumerable<IBuilder> Builders { get; }

		/// <summary>
		/// Raised when the list of components is changed.
		/// </summary>
		event NotifyCollectionChangedEventHandler? ComponentsChange;
		/// <summary>
		/// Collected configured components.
		/// </summary>
		IList Components { get; }
	}
	/// <summary>
	/// Represents a component collector.
	/// </summary>
	public abstract class ComponentCollector<T> : IComponentCollector {
		/// <inheritdoc />
		public abstract string? GetName([NotNull] ref CultureInfo? culture);

		/// <inheritdoc />
		public abstract bool IsAutomatic { get; }

		/// <inheritdoc />
		public event EventHandler? BuildersChanged;

		IEnumerable<IBuilder<T>> m_builders = [];
		/// <summary>
		/// Collected builders.
		/// </summary>
		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<IBuilder<T>> Builders {
			get => m_builders;
			private set {
				m_builders = value;
				BuildersChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		IEnumerable<IBuilder> IComponentCollector.Builders => Builders;

		/// <inheritdoc />
		public event NotifyCollectionChangedEventHandler? ComponentsChange;

		readonly ObservableCollection<T> m_components = [];
		/// <summary>
		/// Collected configured components.
		/// </summary>
		public IReadOnlyList<T> Components => m_components;
		IList IComponentCollector.Components => m_components;

		/// <summary>
		/// Creates an instance of the <see cref="ComponentCollector{T}" /> class.
		/// </summary>
		protected ComponentCollector() {
			m_components.CollectionChanged += OnComponentsChange;
		}
		/// <summary>
		/// Called when the list of components is changed.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">Information about the event.</param>
		protected virtual void OnComponentsChange(object? sender, NotifyCollectionChangedEventArgs e) {
			ComponentsChange?.Invoke(this, e);
		}
	}
}
