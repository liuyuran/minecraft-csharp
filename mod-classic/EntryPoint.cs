using Base.Manager;
using Base.Mods;

namespace mod_classic; 

public class EntryPoint: IModEntryPoint {
    public void OnEnable() {
        LogManager.Instance.Debug("Loaded mod_classic");
    }

    public void OnDisable() {
        LogManager.Instance.Debug("Unloaded mod_classic");
    }
}