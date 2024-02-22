using UnityEngine;
using System.Collections;

/// <summary>
/// Tuple.
/// </summary>
public class Tuple<T1, T2> {
	
	public static readonly char SEPARATOR = '|';
	
	private T1 first;
    public T1 First {
    	get { return first; }
		set { first = value; }
    }

    private T2 second;
    public T2 Second {
    	get { return second; }
		set { second = value; }
    } 

	/// <summary>
	/// Initializes a new instance of the <see cref="Tuple`2"/> class.
	/// </summary>
	/// <param name="f">F.</param>
	/// <param name="s">S.</param>
    public Tuple(T1 f, T2 s) {
    	first = f;
    	second = s;
    }
	
	/// <summary>
	/// Create the specified first and second.
	/// </summary>
	/// <param name='first'>
	/// First.
	/// </param>
	/// <param name='second'>
	/// Second.
	/// </param>
	/// <typeparam name='Ta'>
	/// The 1st type parameter.
	/// </typeparam>
	/// <typeparam name='Tb'>
	/// The 2nd type parameter.
	/// </typeparam>
	public static Tuple<Ta, Tb> Create<Ta, Tb>(Ta first, Tb second) {
		return new Tuple<Ta, Tb>(first, second);
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Tuple`2"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents the current <see cref="Tuple`2"/>.
	/// </returns>
	public override string ToString () {
		return string.Format ("{0}{1}{2}", First, SEPARATOR, Second);
	}
	
	/// <summary>
	/// Serves as a hash function for a <see cref="Tuple`2"/> object.
	/// </summary>
	/// <returns>
	/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
	/// </returns>
	public override int GetHashCode () {
		return first.GetHashCode() ^ second.GetHashCode();
	}
}

/// <summary>
/// Tuple.
/// </summary>
public class Tuple<T1, T2, T3> : Tuple<T1, T2> {

	private T3 third;
	public T3 Third {
		get { return this.third; }
		set { this.third = value; }
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Tuple`3"/> class.
	/// </summary>
	/// <param name="f">F.</param>
	/// <param name="s">S.</param>
	/// <param name="t">T.</param>
	public Tuple(T1 f, T2 s, T3 t) : base(f, s) {
		this.third = t;
	}

	/// <summary>
	/// The first.
	/// </summary>
	public static Tuple<Ta, Tb, Tc> Creat<Ta, Tb, Tc>(Ta one, Tb two, Tc three) {
		return new Tuple<Ta, Tb, Tc>(one, two, three);
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Tuple`3"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Tuple`3"/>.</returns>
	public override string ToString() {
		return string.Format("{0}{1}{2}", base.ToString(), SEPARATOR, this.third);
	}

	/// <summary>
	/// Serves as a hash function for a <see cref="Tuple`3"/> object.
	/// </summary>
	/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
	public override int GetHashCode() {
		return base.GetHashCode() ^ this.third.GetHashCode();
	}
}