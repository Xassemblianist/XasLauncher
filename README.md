# XasLauncher

**Hafif, detay-odaklı bir Minecraft launcher. C# / WPF.**

<p align="left">
  <img src="https://img.shields.io/badge/platform-Windows-0078D6?style=flat-square&logo=windows&logoColor=white" />
  <img src="https://img.shields.io/badge/dil-C%23%20%2F%20.NET-239120?style=flat-square&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/UI-WPF-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/lisans-LGPL%20v3.0-lightgrey?style=flat-square" />
</p>

---

## Bu nedir

Resmi Mojang launcher'ında olmayan üç şey istiyordum: **özel UI**, **anlık RAM kontrolü**, **Forge desteği tek tıkla**. Üçüne birden sahip bir launcher bulamayınca yazdım.

XasLauncher, [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core) üzerine kurulu, sadeleştirilmiş bir WPF arayüzdür. Vanilla + Forge sürümlerini listeler, kuruyor, başlatıyor; RAM tahsisini slider'la ayarlıyorsun.

## Özellikler

- **Custom WPF UI** &mdash; standart pencere kromu yok, draggable title bar, minimize / close
- **Vanilla + Forge** &mdash; Forge installer entegre
- **RAM slider** &mdash; tahsis edilecek belleği canlı ayarla
- **Mods klasörünü tek tıkla aç** &mdash; mod yönetimi için Explorer'a sıçra
- **Tab tabanlı navigasyon** &mdash; Ana ekran / Ayarlar
- **Async sürüm taraması** &mdash; UI bloklanmaz

## Hızlı başlangıç

### Çalıştırılabilirden (önerilen)

[Releases](../../releases) sayfasından `XasLauncher.exe` indir, çift tıkla. .NET 8 runtime gerekli.

### Kaynaktan

```powershell
# Visual Studio 2022 veya .NET 8 SDK gerekli
git clone https://github.com/Xassemblianist/XasLauncher.git
cd XasLauncher\src
dotnet build -c Release
dotnet run -c Release
```

## Teknik

| | |
|---|---|
| **UI** | WPF (XAML) |
| **Çekirdek** | .NET 8 |
| **Minecraft kütüphanesi** | [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core) |
| **Forge desteği** | CmlLib.Core.Installer.Forge |

## Yol haritası

- [ ] Microsoft hesap girişi (offline modun ötesinde)
- [ ] Skin önizleme
- [ ] Modpack paylaşımı (CurseForge / Modrinth)
- [ ] Otomatik sürüm güncellemesi
- [ ] Tema desteği

## Lisans

LGPL v3.0
