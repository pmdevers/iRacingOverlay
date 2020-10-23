namespace iRacingSDK
{
	/// <summary>
	/// Search replay tape for events
	/// </summary>
	public enum ReplaySearchMode
	{
		ToStart = 0,
		ToEnd,
		PrevSession,
		NextSession,
		PrevLap,
		NextLap,
		PrevFrame,
		NextFrame,
		PrevIncident,

		/// <summary>
		/// Camera select car and move to 4 seconds before incident
		/// </summary>
		NextIncident
	};
}
