﻿namespace GhostSharp.Entities
{
    /// <summary>
    /// Choose which exceptions, if any, to suppress.
    /// The default is None (throw all exceptions).
    /// </summary>
    public enum ExceptionLevel
    {
        /// <summary>
        /// No exceptions thrown. All errors suppressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Exceptions generated by a Ghost response (Tag not found, Post not found) are thrown.
        /// All other exceptions suppressed.
        /// </summary>
        Ghost = 1,

        /// <summary>
        /// All exceptions are thrown, except those generated by a Ghost response (Post not found).
        /// </summary>
        NonGhost = 2,

        /// <summary>
        /// All exceptions thrown. Nothing is suppressed.
        /// </summary>
        All = 3
    }
}
