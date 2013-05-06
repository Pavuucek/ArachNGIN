namespace PakCreator.Properties {
    
    
    // Tato t��da V�m umo��uje zpracov�vat specifick� ud�losti v t��d� nastaven�:
    //  Ud�lost SettingChanging je vyvol�na p�ed zm�nou hodnoty nastaven�.
    //  Ud�lost PropertyChanged  je vyvol�na po zm�n� hodnoty nastaven�.
    //  Ud�lost SettingsLoaded je vyvol�na po na�ten� hodnot nastaven�.
    //  Ud�lost SettingsSaving je vyvol�na p�ed ulo�en�m hodnot nastaven�.
    internal sealed partial class Settings {
        
        public Settings() {
            // // Pro p�id�v�n� obslu�n�ch rutin ud�lost� ur�en�ch pro ukl�d�n� a zm�nu nastaven� odkomentujte pros�m n�e uveden� ��dky:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // K�d pro zpracov�n� ud�losti SettingChangingEvent p�idejte zde.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // K�d pro zpracov�n� ud�losti SettingsSaving p�idejte zde.
        }
    }
}
