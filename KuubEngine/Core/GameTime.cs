// <copyright file="GameTime.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace KuubEngine.Core
{
    /// <summary>
    /// Used to keep track of changes in time between updates
    /// </summary>
    public class GameTime
    {
        /// <summary>
        /// Gets or sets the total time since game launched
        /// </summary>
        public TimeSpan Total { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time since the last frame
        /// </summary>
        public TimeSpan Elapsed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTime"/> class.
        /// </summary>
        public GameTime()
        {
            Total = TimeSpan.Zero;
            Elapsed = TimeSpan.Zero;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{{ Total: {0}, Elapsed: {1} }}", Total, Elapsed);
        }
    }
}