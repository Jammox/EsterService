namespace EsterService.Service
{
	/// <summary>
	/// Background process controller.
	/// </summary>
	public interface IProcessController
	{
		/// <summary>
		/// Start process.
		/// </summary>
		/// <param name="procName">Process name.</param>
		/// <param name="procArgs">Process arguments.</param>
		void Start(string procName, string procArgs);

		/// <summary>
		/// Start process.
		/// </summary>
		/// <param name="procName">Process name.</param>
		void Start(string procName);

		/// <summary>
		/// Stop process.
		/// </summary>
		void Stop();
	}
}
