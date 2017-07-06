namespace ArachNGIN.Tracer.MessageFormat
{
    /// <summary>
    ///     Interface for message format
    /// </summary>
    public interface IMessageFormat
    {
        /// <summary>
        ///     Formats a message for logging
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns></returns>
        string Format(TracerMessage message);
    }
}