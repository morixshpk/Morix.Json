using System;

namespace Morix.Json
{
    /// <summary>
    /// The exception that is thrown when a JSON message cannot be parsed.
    /// </summary>
    /// <remarks>
    /// This exception is only intended to be thrown by LightJson.
    /// </remarks>
    public sealed class JsonParseException : Exception
    {
        /// <summary>
        /// Gets the text position where the error occurred.
        /// </summary>
        public TextPosition Position { get; private set; }

        /// <summary>
        /// Gets the type of error that caused the exception to be thrown.
        /// </summary>
        public JsonErrorType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of JsonParseException.
        /// </summary>
        public JsonParseException(TextPosition position)
            : base(GetDefaultMessage(JsonErrorType.Unknown, position)) { }

        /// <summary>
        /// Initializes a new instance of JsonParseException with the given error type and position.
        /// </summary>
        /// <param name="errorType">The error type that describes the cause of the error.</param>
        /// <param name="position">The position in the text where the error occurred.</param>
        public JsonParseException(JsonErrorType type, TextPosition position)
            : this(GetDefaultMessage(type, position), type, position) { }

        /// <summary>
        /// Initializes a new instance of JsonParseException with the given message, error type, and position.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="type">The error type that describes the cause of the error.</param>
        /// <param name="position">The position in the text where the error occurred.</param>
        public JsonParseException(string message, JsonErrorType type, TextPosition position)
            : base(message)
        {
            this.Type = type;
            this.Position = position;
        }

        private static string GetDefaultMessage(JsonErrorType type, TextPosition textPosition)
        {
            string message = "An error occurred while parsing the JSON message.";
            if (type == JsonErrorType.IncompleteMessage)
            {
                message = "The string ended before a value could be parsed.";
            }
            else if (type == JsonErrorType.InvalidOrUnexpectedCharacter)
            {
                return "The parser encountered an invalid or unexpected character.";
            }
            else if (type == JsonErrorType.DuplicateObjectKeys)
            {
                message = "The parser encountered a JsonObject with duplicate keys.";
            }
            
            return message + " Line " + textPosition.Line + ", Position " + textPosition.Position + ".";
        }

        /// <summary>
        /// Enumerates the types of errors that can occur when parsing a JSON message.
        /// </summary>
        public enum JsonErrorType : byte
        {
            /// <summary>
            /// Indicates that the cause of the error is unknown.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Indicates that the text ended before the message could be parsed.
            /// </summary>
            IncompleteMessage,

            /// <summary>
            /// Indicates that a JsonObject contains more than one key with the same name.
            /// </summary>
            DuplicateObjectKeys,

            /// <summary>
            /// Indicates that the parser encountered and invalid or unexpected character.
            /// </summary>
            InvalidOrUnexpectedCharacter,
        }
    }
}
