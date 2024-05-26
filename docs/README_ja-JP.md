# Danheng Server

**__このプロジェクトは開発中です！__**

<p align="center">
<a href="https://visualstudio.com"><img src="https://img.shields.io/badge/Visual%20Studio-000000.svg?style=for-the-badge&logo=visual-studio&logoColor=white" /></a>
<a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-000000.svg?style=for-the-badge&logo=.NET&logoColor=white" /></a>
<a href="https://www.gnu.org/"><img src="https://img.shields.io/badge/GNU-000000.svg?style=for-the-badge&logo=GNU&logoColor=white" /></a>
</p>
<p align="center">
  <a href="https://discord.gg/xRtZsmHBVj"><img src="https://img.shields.io/badge/Discord%20Server-000000.svg?style=for-the-badge&logo=Discord&logoColor=white" /></a>
</p>

[EN](../README.md) | [簡中](README_zh-CN.md) | [繁中](README_zh-CN.md) | [JP](README_ja-JP.md)

## 💡機能

- [√] **ストア機能**
- [√] **チームを編成する**
- [√] **ドロー** - カスタム可能な確率: )
- [√] **と戦う** - シーンスキルにいくつかのエラーがあります
- [√] **シーン** - 歩行シミュレータ、インタラクティブ、正確なエンティティロード
- [√] **基本的な役割育成** - ちょっとバグがありますが、影響はあまりありません
- [√] **タスク＃タスク＃** - 男性の多くのタスクを完了しています。女性を選択すると、特定のタスクに引っかかり、修復が必要になる可能性があります
- [-] **交友機能** - 开发中...
- [-] **模擬宇宙** - 开发中...

- [ ] **詳細**  - Comming soon...

## 🍗使用&インストール

### クイックスタート

1. [Action](https://github.com/StopWuyu/DanhengServer/actions) で実行可能ファイルをダウンロードする
2. ダウンロードが完了した` DanhengServer.zip `を開いて任意のフォルダに解凍します __*英文パスが望ましい*__

> (オプション) ソースコードのWebServerフォルダに` certificate.p 12 `をダウンロードすることで、HTTPSモードで起動して転送をより安全にすることができます: )

3. GameServer.exeの実行
4. エージェント起動ゲームの実行

### 構築＃コウチク＃

Danhengserver Dotnetを使用した構築

**プリソフトウェア：**

- [.NET](https://dotnet.microsoft.com/)
- [Git](https://git-scm.com/downloads)

##### Windows

```shell
git clone --recurse-submodules https://github.com/StopWuyu/DanhengServer.git
cd DanhengServer
.\dotnet build # コンパイル
```

##### Linux （Ubuntu20.04）
- インストール環境
```shell
# Microsoftパッケージリポジトリの追加
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# SDKのインストール
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0
```

- 環境をコンパイルして実行する
```shell
git clone --recurse-submodules https://github.com/StopWuyu/DanhengServer.git
cd DanhengServer
.\dotnet build # コンパイル
./Gameserver
```
**他のシステムバージョンはMicrosoftに表示してください**
- [Microsoftのチュートリアル](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/sdk-8.0.204-linux-x64-binaries)

## ❓ヘルプ

- アンドロイドシステムのサポート
- 100040119（自動完了できません）（/mission finish 100040119 を使用した修正）

## ❕️トラブルシューティング

一般的な問題の解決策を入手するか、ヘルプを参照してください。[デルのDiscordサーバ](https://discord.gg/xRtZsmHBVj)

## 🙌に感謝

- Weedwacker - kcp実装の提供
- [SqlSugar](https://github.com/donet5/SqlSugar) - ORMの提供
- [LunarCore](https://github.com/Melledy/LunarCore) - いくつかのデータ構造とアルゴリズム
