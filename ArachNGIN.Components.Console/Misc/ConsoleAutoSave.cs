namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// V��et vlastnost� jak ukl�dat log
    /// </summary>
    public enum ConsoleAutoSave
    {
        /// <summary>
        /// Pouze manu�ln� ukl�d�n� (default)
        /// </summary>
        ManualOnly,
        /// <summary>
        /// Ulo�it log p�i ka�d�m p�id�n� textu
        /// </summary>
        OnLineAdd,
        /// <summary>
        /// Ulo�it log p�i ukon�en� programu
        /// </summary>
        OnProgramExit
    }
}