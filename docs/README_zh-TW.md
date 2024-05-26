# Danheng S# Danheng Server

**__此專案目前正在開發中!__**

<p align="center">
<a href="https://visualstudio.com"><img src="https://img.shields.io/badge/Visual%20Studio-000000.svg?style=for-the-badge&logo=visual-studio&logoColor=white" /></a>
<a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-000000.svg?style=for-the-badge&logo=.NET&logoColor=white" /></a>
<a href="https://www.gnu.org/"><img src="https://img.shields.io/badge/GNU-000000.svg?style=for-the-badge&logo=GNU&logoColor=white" /></a>
</p>
<p align="center">
  <a href="https://discord.gg/xRtZsmHBVj"><img src="https://img.shields.io/badge/Discord%20Server-000000.svg?style=for-the-badge&logo=Discord&logoColor=white" /></a>
</p>

[EN](../README.md) | [簡中](README_zh-CN.md) | [繁中](README_zh-CN.md) | [JP](README_ja-JP.md)

## 💡功能

- [√] **商店**
- [√] **編隊**
- [√] **抽卡** - 可自訂機率: )
- [√] **戰鬥** - 場景技能中有一些錯誤
- [√] **場景** - 走路模擬器、互動、正確載入實體
- [√] **基本的角色培養** - 一些小bug，影響體驗不大
- [√] **任務** - 已完成男主（穹）的許多任務，若你選擇女主（星）則可能會在某些任務中卡住，需要修復
- [-] **朋友** - 開發中...
- [-] **忘卻之庭與虛構敘事** - 開發中...
- [-] **模擬宇宙** - 開發中...

- [ ] **更多**  - Coming soon...

## 🍗使用&安裝

### 快速啟動

1. 在 [Action](https://github.com/StopWuyu/DanhengServer/actions) 下載可執行文件
2. 打開下載完成的 `DanhengServer.zip` 解解壓至任意資料夹 __*最好是英文路徑*__

> (可選) 在原始碼的WebServer資料夾中下載 `certificate.p12` 使其以HTTPS模式啟動 讓你的傳輸更加安全: )

3. 運行GameServer.exe
4. 運行代理 啟動遊戲 連結，享受！

### 構建

Danhengserver使用Dotnet構建

**前置：**

- [.NET](https://dotnet.microsoft.com/)
- [Git](https://git-scm.com/downloads)

##### Windows

```shell
git clone --recurse-submodules https://github.com/EggLinks/DanhengServer.git
cd DanhengServer
.\dotnet build # 編譯
```
##### Linux （Ubuntu20.04）
```shell
# 添加 Microsoft 包儲存庫
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# 安裝SDK
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0
```

- 編譯並運行環境
```shell
git clone --recurse-submodules https://github.com/EggLinks/DanhengServer.git
cd DanhengServer
.\dotnet build # 編譯
./Gameserver
```
**向 Microsoft 顯示其他系統版本**
- [微軟教學](https://dotnet.microsoft.com/zh-tw/download/dotnet/thank-you/sdk-8.0.204-linux-x64-binaries)

## ❓幫助

- 支持Android系统
- 100040119（無法自動完成）（使用 /mission finish 100040119 進行修復）

## ❕️故障排除

獲取常見問題的解決方案或尋求幫助，請加入[我們的Discord伺服器](https://discord.gg/xRtZsmHBVj)

## 🙌感謝

- Weedwacker - 提供 kcp 實現
- [SqlSugar](https://github.com/donet5/SqlSugar) - 提供 ORM
- [LunarCore](https://github.com/Melledy/LunarCore) - 一些資料結構和算法
erver
