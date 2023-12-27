﻿namespace Anubis.LC.ExtraDays.Models.Enums
{
    /// <summary>
    /// Player permission levels
    /// </summary>
    public enum AllowedCaller
    {
        /// <summary>
        /// Nil permission level, deny everyone.
        /// </summary>
        None = -1,

        /// <summary>
        /// Default permission level, all players have access
        /// </summary>
        Player = 0,

        /// <summary>
        /// Only the lobby host has access
        /// </summary>
        /// <remarks>
        /// Beyond access control, this permission level can be useful to restrict commands that can only execute on the host system
        /// </remarks>
        Host = 1
    }
}
