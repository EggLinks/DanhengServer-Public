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
- [√] **タスク＃タスク＃** - 一部のタスクにはエラーがある可能性があります。ベロブルグの主要なタスクはすべて完了しています。男性主人公と女性主人公にはまだテストされていません
- [-] **交友機能**
- [-] **忘却の庭 & 虚構叙事**
- [-] **模擬宇宙**

- [ ] **詳細**  - Coming soon...

「アニメーション ゲーム」の新しいバージョンがリリースされると、最初は一部の機能がサポートされなくなります。引き続き、私たちの投稿にご注意ください。  バージョン 2.3 以降、ベータ版に適したプライベート ブランチを設立し、準備が完了次第、メイン ウェアハウスにマージされます。

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

##💡API支援です

-バージョン2.3から、外部API呼び出しインタフェースをサポートします。
-全体のインタフェースはDispatchインタフェースに入口を加えます。例えば、Dispatchはhttp://127.0.0.1:8080、要求パラメータとリターンはjson形式です。
-(1)ライセンスインタフェース:http://127.0.0.1:8080/muip/auth_admin(支持ポスト/ get)
- -必須引数1:admin_key (config.phpでのMuipServer/AdminKey構成)
- -必須パラメータ2:key_type(タイプ、例えばPEM)です。
- -リターン例です:
```json
  {
    "code": 0,
    "message": "Authorized admin key successfully!",
    "data": {
        "rsaPublicKey": "***",
        "sessionId": "***",
        "expireTimeStamp": ***
    }
  }
```
—(2)提出命令インタフェース:http://127.0.0.1:8080/muip/exec_cmd(支持ポスト/ get)
- -必伝パラメータ1:SessionId(ライセンスインターフェース要求後に取得します)
- -必須引数2:Command(実行するコマンドをrsaPublicKey[ライセンスインターフェース取得]でRSA[pacs#1]で暗号化します)
- -必伝パラメータ3:TargetUid(コマンドを実行するプレイヤーUID)です
- -リターン例です:
```json
    {
      "code": 0,
      "message": "Success",
      "data": {
          "sessionId": "***",
          "message": "*** //base64编码后
      }
    }
```
—(3)サーバーの状態をインタフェース:http://127.0.0.1:8080/muip/server_information(支持get)だけ
- -必伝パラメータ1:SessionId(ライセンスインターフェース要求後に取得します)
- -リターン例です:
```json
    {
      "code": 0,
      "message": "Success",
      "data": {
          "onlinePlayers": [
              {
                  "uid": 10001,
                  "name": "KEVIN",
                  "headIconId": 208001
              },
              ....
          ],
          "serverTime": 1720626191,
          "maxMemory": 16002.227,
          "usedMemory": 7938.5547,
         "programUsedMemory": 323
      }
    }
```
—(4)プレイヤー情報を盗み出すインタフェース:http://127.0.0.1:8080/muip/player_information(支持get)だけ
- -必伝パラメータ1:SessionId(ライセンスインターフェース要求後に取得します)
- -必伝パラメーター2:Uid(プレイヤーUid)
- -リターン例です:
```json
    {
      "code": 0,
      "message": "Success",
      "data": {
          "uid": 10001,
          "name": "KEVIN",
          "signature": "",
          "headIconId": 208001,
          "curPlaneId": 10001,
          "curFloorId": 10001001,
          "playerStatus": "Explore",
          "stamina": 182,
          "recoveryStamina": 4,
          "assistAvatarList": Array[0],
          "displayAvatarList": Array[0],
          "finishedMainMissionIdList": Array[38],
          "finishedSubMissionIdList": Array[273],
          "acceptedMainMissionIdList": Array[67],
          "acceptedSubMissionIdList": Array[169]
      }
  }
```

## ❕️トラブルシューティング

一般的な問題の解決策を入手するか、ヘルプを参照してください。[デルのDiscordサーバ](https://discord.gg/xRtZsmHBVj)

## 🙌に感謝

- Weedwacker - kcp実装の提供
- [SqlSugar](https://github.com/donet5/SqlSugar) - ORMの提供
- [LunarCore](https://github.com/Melledy/LunarCore) - いくつかのデータ構造とアルゴリズム
