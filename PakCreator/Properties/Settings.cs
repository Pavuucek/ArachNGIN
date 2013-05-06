namespace PakCreator.Properties {
    
    
    // Tato třída Vám umožňuje zpracovávat specifické události v třídě nastavení:
    //  Událost SettingChanging je vyvolána před změnou hodnoty nastavení.
    //  Událost PropertyChanged  je vyvolána po změně hodnoty nastavení.
    //  Událost SettingsLoaded je vyvolána po načtení hodnot nastavení.
    //  Událost SettingsSaving je vyvolána před uložením hodnot nastavení.
    internal sealed partial class Settings {
        
        public Settings() {
            // // Pro přidávání obslužných rutin událostí určených pro ukládání a změnu nastavení odkomentujte prosím níže uvedené řádky:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Kód pro zpracování události SettingChangingEvent přidejte zde.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Kód pro zpracování události SettingsSaving přidejte zde.
        }
    }
}
