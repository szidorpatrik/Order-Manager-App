using System;

namespace DatabaseLibrary.Models;

public class Order {
	public DateTime    DateCompleted { get; set; }
	public DateTime    DateCanceled  { get; set; }
	public DateTime    DateCreated   { get; set; }
	public DateTime    DateEnd       { get; set; }
	public DateTime    DateStart     { get; set; }
	public int         Id            { get; set; }
	public OrderItem[] OrderItems    { get; set; } = [];
	public int?        OrderNumber   { get; set; } = 0;
	public bool        IsCompleted   { get; set; }
	public bool        IsCanceled    { get; set; }
	public bool        IsPending     => !IsCompleted && !IsCanceled;

	/// <summary>
	/// Gets the duration of the order based on its status.
	/// </summary>
	/// <value>
	/// A <see cref="TimeSpan"/> representing the duration of the order:
	/// <list type="bullet">
	///     <item>If the order is canceled, returns <see cref="TimeSpan.MinValue"/>.</item>
	///     <item>If the order is completed, returns the time elapsed between <see cref="DateStart"/> and <see cref="DateCompleted"/>.</item>
	///     <item>If the order is pending, returns the time elapsed between <see cref="DateStart"/> and the current time (<see cref="DateTime.Now"/>).</item>
	/// </list>
	/// </value>
	/// <remarks>
	/// The duration is calculated dynamically by calling the <see cref="GetDuration"/> method.
	/// Ensure that <see cref="DateStart"/> is properly initialized before accessing this property.
	/// </remarks>
	public TimeSpan Duration => GetDuration();

	public override string ToString() => $"#{OrderNumber}";

	/// <summary>
	/// Returns the duration of the order based on its status.
	/// </summary>
	/// <returns>
	/// A <see cref="TimeSpan"/> representing the duration:
	/// <list type="bullet">
	///     <item>If the order is canceled, returns <see cref="TimeSpan.MinValue"/>.</item>
	///     <item>If the order is completed, returns the difference between <see cref="DateCompleted"/> and <see cref="DateStart"/>.</item>
	///     <item>If the order is pending, returns the difference between the current time (<see cref="DateTime.Now"/>) and <see cref="DateStart"/>.</item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method assumes that the <see cref="DateStart"/> property is properly initialized.
	/// If <see cref="DateStart"/> is not set, the calculation may yield unexpected results.
	/// </remarks>
	private TimeSpan GetDuration() {
		if (IsCanceled)
			return TimeSpan.MinValue;
		if (IsCompleted)
			return DateCompleted - DateStart;
		return DateTime.Now - DateStart;
	}
}
