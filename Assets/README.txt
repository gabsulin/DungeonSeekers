# DungeonSeekers

> 🔐 Tato hra obsahuje prvky ochrany proti neoprávněnému kopírování a reverznímu inženýrství.

## 🔒 Ochranné prvky

- ✅ Licenční ověření při spuštění (HWID + klíč)
- ✅ Server-side kontrola přes Python backend
- ✅ Obfuskace kódu pomocí ConfuserEx (`Assembly-CSharp.dll`)

## ⚠️ Poznámky pro vývojáře

- Při **buildování hry** je potřeba následně obfuskovat hlavní DLL:
  - `DungeonSeekers_Data/Managed/Assembly-CSharp.dll`
  - Použij nástroj **ConfuserEx** s přiloženým `.crproj` profilem (viz níže)
- Pokud se build po obfuskaci rozbije, vrať zálohovanou DLL nebo rebuildni v Unity.
- Licenční validace je povinná — pokud selže, hra se ukončí (`Application.Quit()`).

## 🧰 Nástroje

- Unity [verze XX.XX]
- ConfuserEx (`Confuser.CLI.exe`, viz složka `Tools/Obfuscator`)
- PythonAnywhere API (`/api/validate`)

## ⚖️ Právní upozornění

> Tento software je chráněn autorským právem. Jakýkoli pokus o:
> - odstranění licenčního systému,
> - reverzní inženýrství obfuskovaných DLL,
> - nebo šíření buildů bez oprávnění,
> může být považován za porušení licenčních podmínek.