namespace ArachNGIN.Tracer.MessageFormat
{
    /// <summary>
    ///     Debug message format class
    /// </summary>
    public class DebugMessageFormat : IMessageFormat
    {
        /// <summary>
        ///     Formats trace message like this:
        ///     06.07.2017 19:30:18: Info [Program.Main() -> line:41]: Trace test!
        /// </summary>
        /// <param name="message">Trace message</param>
        /// <returns></returns>
        public string Format(TracerMessage message)
        {
            return
                $"{message.TimeStamp:dd.MM.yyyy HH:mm:ss}: {message.Level} [{message.OriginClass}.{message.OriginFunction}() -> line:{message.OriginLine}]: {message.Message}";
        }
    }
}