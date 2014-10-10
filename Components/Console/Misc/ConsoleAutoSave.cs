namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Výčet vlastností jak ukládat log
    /// </summary>
    public enum ConsoleAutoSave
    {
        /// <summary>
        /// Pouze manuální ukládání (default)
        /// </summary>
        ManualOnly,
        /// <summary>
        /// Uložit log při každém přidání textu
        /// </summary>
        OnLineAdd,
        /// <summary>
        /// Uložit log při ukončení programu
        /// </summary>
        OnProgramExit
    }
}