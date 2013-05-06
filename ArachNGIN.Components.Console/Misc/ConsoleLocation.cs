namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Výèet použitý pro umístìní konzole na obrazovku
    /// </summary>
    public enum ConsoleLocation
    {
        /// <summary>
        /// Levý horní roh
        /// </summary>
        TopLeft,
        /// <summary>
        /// Pravý horní roh
        /// </summary>
        TopRight,
        /// <summary>
        /// Spodní levý roh
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Spodní pravý roh
        /// </summary>
        BottomRight,
        /// <summary>
        /// Prostøedek obrazovky
        /// </summary>
        ScreenCenter,
        /// <summary>
        /// Nìkde jinde. Nastaví se na hodnoty uvedené
        /// v property Location
        /// </summary>
        SomeWhereElse
    }
}