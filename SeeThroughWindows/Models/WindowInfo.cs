namespace SeeThroughWindows.Models
{
    /// <summary>
    /// Stores information about a window that has been modified by the application
    /// </summary>
    public class WindowInfo
    {
        /// <summary>
        /// Original window style before modification
        /// </summary>
        public uint Style { get; set; }

        /// <summary>
        /// Original alpha value before modification
        /// </summary>
        public short OriginalAlpha { get; set; }

        /// <summary>
        /// Current alpha value
        /// </summary>
        public short CurrentAlpha { get; set; }

        /// <summary>
        /// Initializes a new instance of WindowInfo
        /// </summary>
        /// <param name="style">Original window style</param>
        /// <param name="alpha">Original alpha value</param>
        public WindowInfo(uint style, short alpha)
        {
            Style = style;
            CurrentAlpha = alpha;
            OriginalAlpha = alpha;
        }
    }
}
