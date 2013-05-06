namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Výèet vlastností jak ukládat log
    /// </summary>
    public enum ConsoleAutoSave
    {
        /// <summary>
        /// Pouze manuální ukládání (default)
        /// </summary>
        ManualOnly,
        /// <summary>
        /// Uložit log pøi každém pøidání textu
        /// </summary>
        OnLineAdd,
        /// <summary>
        /// Uložit log pøi ukonèení programu
        /// </summary>
        OnProgramExit
    }
}