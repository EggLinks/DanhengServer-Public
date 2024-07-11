[EN](MuipAPI.md) | [简中](MuipAPI_zh-CN.md) | [繁中](MuipAPI_zh-TW.md) | [JP](MuipAPI_ja-JP.md)

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
