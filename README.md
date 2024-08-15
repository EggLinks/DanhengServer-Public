# Danheng Server

**__This project is under development! Some game technique might not correctly!__**

<p align="center">
<a href="https://visualstudio.com"><img src="https://img.shields.io/badge/Visual%20Studio-000000.svg?style=for-the-badge&logo=visual-studio&logoColor=white" /></a>
<a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-000000.svg?style=for-the-badge&logo=.NET&logoColor=white" /></a>
<a href="https://www.gnu.org/"><img src="https://img.shields.io/badge/GNU-000000.svg?style=for-the-badge&logo=GNU&logoColor=white" /></a>
</p>
<p align="center">
  <a href="https://discord.gg/xRtZsmHBVj"><img src="https://img.shields.io/badge/Discord%20Server-000000.svg?style=for-the-badge&logo=Discord&logoColor=white" /></a>
</p>

[EN](README.md) | [简中](docs/README_zh-CN.md) | [繁中](docs/README_zh-TW.md) | [JP](docs/README_ja-JP.md)

## 💡 Function

- [√] **Shop**
- [√] **Formation**
- [√] **Gacha** - Custom probability : )
- [√] **Battle** - Some errors are exist among scene skills
- [√] **Scene** - Walking simulator, interaction, correct loading of entities
- [√] **Basic character development** - Some minor bugs that don't significantly affect the playing experience
- [√] **Quests** - There may be some bugs in some missions, the main story before Penacony is basically playable, and most of the story after Penacony has bugs
- [√] **Friends**
- [√] **Forgotten Hall & Pure Fiction & Apocalyptic Shadow**
- [√] **Simulated Universe & Gold and Gears**
- [√] **Achievements** - Most achievements can be completed.

- [ ] **More**  - Coming soon

Some functions for the game might not support at the first time when new "Anime Game" version drops, please stay tune to our new commit. Since version 2.3, we've created a private fork which supports beta version, and will merge to main branch asap when it's ready.

## 🍗 Use & Installation

### Quick Start

1. Download the executable file from [Action](https://github.com/EggLink/DanhengServer-Public/actions)
2. Open the downloaded `DanhengServer.zip` and extract it to any folder __*preferably an English path__
   (Optional) Download the `certificate.p12` from the WebServer folder of the source code to enable HTTPS mode, ensuring a more secure traffic
3. Download Resources[https://github.com/Dimbreath/StarRailData](https://github.com/Dimbreath/StarRailData) and unzip to the same directory `Resources`, and Turn to [https://github.com/EggLinks/DanhengServer-Resources](https://github.com/EggLinks/DanhengServer-Resources) download only `Resources/Config` folder, ExcelOutput and other should use Dimbreath's，then download Configuration from the project and unzip to the same directory`Config`
4. Run GameServer.exe
5. Run proxy, start the game, and enjoy!

### Build

DanhengServer is built using .NET Framework

**Requirement: **

- [.NET](https://dotnet.microsoft.com/)
- [Git](https://git-scm.com/downloads)

##### Windows

```shell
git clone --recurse-submodules https://github.com/EggLink/DanhengServer-Public.git
cd DanhengServer
dotnet build # compile
```
##### Linux （Ubuntu 20.04）
```shell
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0
```

- Compile and run environment
```shell
git clone --recurse-submodules https://github.com/EggLink/DanhengServer-Public.git
cd DanhengServer
.\dotnet build # compile
./Gameserver
```

## ❓ Help

- Support Android system
- 
## 🔗API Help
[EN](docs/MuipAPI.md) | [简中](docs/MuipAPI_zh-CN.md) | [繁中](docs/MuipAPI_zh-TW.md) | [JP](docs/MuipAPI_ja-JP.md)

## ❕️ Troubleshooting

For solutions to common problems or assistance, please join our Discord server at [https://discord.gg/xRtZsmHBVj](https://discord.gg/xRtZsmHBVj)

## 🙌 Acknowledgements

- Weedwacker - Provides kcp implementation
- [SqlSugar](https://github.com/donet5/SqlSugar) - Provides ORM
- [LunarCore](https://github.com/Melledy/LunarCore) - Some data structures and algorithms
