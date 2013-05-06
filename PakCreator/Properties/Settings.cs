namespace PakCreator.Properties {
    
    
    // Tato tøída Vám umožòuje zpracovávat specifické události v tøídì nastavení:
    //  Událost SettingChanging je vyvolána pøed zmìnou hodnoty nastavení.
    //  Událost PropertyChanged  je vyvolána po zmìnì hodnoty nastavení.
    //  Událost SettingsLoaded je vyvolána po naètení hodnot nastavení.
    //  Událost SettingsSaving je vyvolána pøed uložením hodnot nastavení.
    internal sealed partial class Settings {
        
        public Settings() {
            // // Pro pøidávání obslužných rutin událostí urèených pro ukládání a zmìnu nastavení odkomentujte prosím níže uvedené øádky:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Kód pro zpracování události SettingChangingEvent pøidejte zde.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Kód pro zpracování události SettingsSaving pøidejte zde.
        }
    }
}
