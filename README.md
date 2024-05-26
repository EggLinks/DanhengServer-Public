# Danheng Server

**__This project is under development! Some game technique cannot run correctly!__**

<p align="center">
<a href="https://visualstudio.com"><img src="https://img.shields.io/badge/Visual%20Studio-000000.svg?style=for-the-badge&logo=visual-studio&logoColor=white" /></a>
<a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-000000.svg?style=for-the-badge&logo=.NET&logoColor=white" /></a>
<a href="https://www.gnu.org/"><img src="https://img.shields.io/badge/GNU-000000.svg?style=for-the-badge&logo=GNU&logoColor=white" /></a>
</p>
<p align="center">
  <a href="https://discord.gg/xRtZsmHBVj"><img src="https://img.shields.io/badge/Discord%20Server-000000.svg?style=for-the-badge&logo=Discord&logoColor=white" /></a>
</p>

[EN](README.md) | [簡中](docs/README_zh-CN.md) | [繁中](docs/README_zh-CN.md) | [JP](docs/README_ja-JP.md)

## 💡 Function

- [√] **Shop**
- [√] **Formation**
- [√] **Gacha** - Custom probability : )
- [√] **Battle** - Some errors are exist among scene skills
- [√] **Scene** - Walking simulator, interaction, correct loading of entities
- [√] **Basic character development** - Some minor bugs that don't significantly affect the experience
- [√] **Quests** - Many quests involving the male Trailblazer (Caelus) have been completed. If you choose the female Trailblazer (Stelle), you may stuck in certain quests and need to be repaired
- [-] **Friends** - Under development
- [-] **Memory of Chaos and Pure Fiction** - Under development
- [-] **Simulated Universe** - Under development

- [ ] **More**  - Coming soon

## 🍗 Use & Installation

### Quick Start

1. Download the executable file from [Action](https://github.com/EggLinks/DanhengServer/actions)
2. Open the downloaded `DanhengServer.zip` and extract it to any folder __*preferably an English path__
   (Optional) Download the `certificate.p12` from the WebServer folder of the source code to enable HTTPS mode, ensuring a more secure traffic
3. Run GameServer.exe
4. Run proxy, start the game, and enjoy!

### Build

Danheng server is built using Dotnet Framework

**Requirement: **

- [.NET](https://dotnet.microsoft.com/)
- [Git](https://git-scm.com/downloads)

##### Windows

```shell
git clone --recurse-submodules https://github.com/EggLinks/DanhengServer.git
cd DanhengServer
dotnet build # compile
```

## ❓ Help

- Support Android system
- Mission 100040119 (cannot complet automatically) (use /mission finish 100040119 to fix it)

## ❕️ Troubleshooting

For solutions to common problems or assistance, please join our Discord server at [https://discord.gg/xRtZsmHBVj](https://discord.gg/xRtZsmHBVj)

## 🙌 Acknowledgements

- Weedwacker - Provides kcp implementation
- [SqlSugar](https://github.com/donet5/SqlSugar) - Provides ORM
- [LunarCore](https://github.com/Melledy/LunarCore) - Some data structures and algorithms
