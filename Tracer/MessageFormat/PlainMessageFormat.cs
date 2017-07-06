namespace ArachNGIN.Tracer.MessageFormat
{
    /// <summary>
    ///     Plain message format class
    /// </summary>
    public class PlainMessageFormat : IMessageFormat
    {
        /// <summary>
        ///     Formats message to include just a message and nothing else
        /// </summary>
        /// <param name="message">Trace message</param>
        /// <returns></returns>
        public string Format(TracerMessage message)
        {
            return message.Message;
        }
    }
}