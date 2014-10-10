namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Výčet použitý pro umístění konzole na obrazovku
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
        /// Prostředek obrazovky
        /// </summary>
        ScreenCenter,
        /// <summary>
        /// Někde jinde. Nastaví se na hodnoty uvedené
        /// v property Location
        /// </summary>
        SomeWhereElse
    }
}