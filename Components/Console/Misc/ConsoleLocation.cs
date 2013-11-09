namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// V��et pou�it� pro um�st�n� konzole na obrazovku
    /// </summary>
    public enum ConsoleLocation
    {
        /// <summary>
        /// Lev� horn� roh
        /// </summary>
        TopLeft,
        /// <summary>
        /// Prav� horn� roh
        /// </summary>
        TopRight,
        /// <summary>
        /// Spodn� lev� roh
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Spodn� prav� roh
        /// </summary>
        BottomRight,
        /// <summary>
        /// Prost�edek obrazovky
        /// </summary>
        ScreenCenter,
        /// <summary>
        /// N�kde jinde. Nastav� se na hodnoty uveden�
        /// v property Location
        /// </summary>
        SomeWhereElse
    }
}