namespace Morix.Json
{
    /// <summary>
    /// Enumerates the types of Json values.
    /// </summary>
    public enum JsonType : byte
    {
        /// <summary>
        /// A null value.
        /// </summary>
        Null = 0,

        /// <summary>
        /// A boolean value.
        /// </summary>
        Boolean,

        /// <summary>
        /// A number value.
        /// </summary>
        Number,

        /// <summary>
        /// A string value.
        /// </summary>
        String,

        /// <summary>
        /// An array value.
        /// </summary>
        Array,

        /// <summary>
        /// An object value.
        /// </summary>
        Object,
    }
}