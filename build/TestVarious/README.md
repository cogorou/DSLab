TestVarious
===

.NET Framework から DirectShow を使用するサンプルです。  
ここでは、基本的な使用例を示します。  

## ビルド

予め [DSLab.Core](../DSLab.Core/README.md) をビルドしておく必要があります。  
その後、プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	C:> ＿build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	C:> ＿clean.bat


## 実行

コマンドプロンプトを起動し、$(TargetDir) に移動して実行してください。  

	C:> cd bin\Release  
	C:> demo.exe  


処理の経過を標準出力に表示します。  
出力結果の確認が必要であればリダイレクトを使用してテキストファイルに保存してください。  

	C:> cd bin\Release  
	C:> demo.exe > result.txt  


処理結果の動画や画像は Results ディレクトリに保存されています。  
一部のサンプルは GRF ファイルを実行ディレクトリに保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ (処理結果の動画や画像)
	├ (GRF ファイル)


## 概要

各関数の概要を記載します。  

TOPIC  

- Sample01: 接続されているデバイスの一覧
- Sample11: カメラから画像を取り込む処理
- Sample12: カメラから画像を取り込み、動画ファイル(avi)へ保存する処理
- Sample13: カメラから画像を取り込み、動画ファイル(wmv)へ保存する処理
- Sample14: カメラ制御とビデオ品質制御
- Sample21: AVI 形式のファイルを読み込む処理
- Sample22: AVI 形式のファイルを読み込み、WMV 形式のファイルに保存する処理
- Sample31: WMV 形式のファイルを読み込む処理
- Sample32: WMV 形式のファイルを読み込み、AVI 形式のファイルに保存する処理

----

**Sample01: 接続されているデバイスの一覧**

接続されているデバイス(映像入力、音声入力、音声出力)を列挙するサンプルです。

出力結果  

	Sample01
	Video Input Devices (3)
	|- 0
	|  |- Microsoft LifeCam Cinema
	|  |- {8754B887-3626-4697-AB37-6D835C649C86} (0)
	|  |- Pins (2)
	|  |  |- 0: PINDIR_OUTPUT   = キャプチャ
	|  |  |- 1: PINDIR_INPUT    = ビデオ カメラ端子
	|  |  |- Outpin Details
	|  |  |  |- Outpin[0]: 23 formats
	|  |  |  |  |-  0:  640, 480
	|  |  |  |  |-  1: 1280, 720
	|  |  |  |  : (中略)
	|  |  |  |  |- 22:  160, 120
	|- 1
	|  |- Logicool Qcam Orbit/Sphere AF
	|  |- {17CCA71B-ECD7-11D0-B908-00A0C9223196} (0)
	|  |- Pins (3)
	|  |  |- 0: PINDIR_OUTPUT   = キャプチャ
	|  |  |- 1: PINDIR_INPUT    = ビデオ カメラ端子
	|  |  |- 2: PINDIR_OUTPUT   = 静止画像
	|  |  |- Outpin Details
	|  |  |  |- Outpin[0]: 46 formats
	|  |  |  |  |-  0:  640, 480
	|  |  |  |  |-  1:  160,  90
	|  |  |  |  : (中略)
	|  |  |  |  |- 45: 1600,1200
	|  |  |  |- Outpin[1]: 58 formats
	|  |  |  |  |-  0:  320, 240
	|  |  |  |  |-  1:  160, 120
	|  |  |  |  : (中略)
	|  |  |  |  |- 57: 3264,2448
	|- 2
	|  |- USB2.0 Camera
	|  |- {17CCA71B-ECD7-11D0-B908-00A0C9223196} (0)
	|  |- Pins (3)
	|  |  |- 0: PINDIR_OUTPUT   = キャプチャ
	|  |  |- 1: PINDIR_INPUT    = ビデオ カメラ端子
	|  |  |- 2: PINDIR_OUTPUT   = 静止画像
	|  |  |- Outpin Details
	|  |  |  |- Outpin[0]: 46 formats
	|  |  |  |  |-  0:  640, 480
	|  |  |  |  |-  1:  160,  90
	|  |  |  |  : (中略)
	|  |  |  |  |- 45: 1600,1200
	|  |  |  |- Outpin[1]: 58 formats
	|  |  |  |  |-  0:  320, 240
	|  |  |  |  |-  1:  160, 120
	|  |  |  |  : (中略)
	|  |  |  |  |- 57: 3264,2448
	Audio Input Devices (3)
	|- 0
	|  |- マイク (Orbit/Sphere AF)
	|  |- {E30629D2-27E5-11CE-875D-00608CB78066} (0)
	|  |- Pins (2)
	|  |  |- 0: PINDIR_OUTPUT   = Capture
	|  |  |- 1: PINDIR_INPUT    = マスター音量
	|- 1
	|  |- デスクトップ マイク (Cinema - Microsoft 
	|  |- {E30629D2-27E5-11CE-875D-00608CB78066} (0)
	|  |- Pins (2)
	|  |  |- 0: PINDIR_OUTPUT   = Capture
	|  |  |- 1: PINDIR_INPUT    = マスター音量
	|- 2
	|  |- マイク (Realtek High Definition Au
	|  |- {E30629D2-27E5-11CE-875D-00608CB78066} (0)
	|  |- Pins (2)
	|  |  |- 0: PINDIR_OUTPUT   = Capture
	|  |  |- 1: PINDIR_INPUT    = マスター音量
	Audio Output Devices (8)
	|- 0
	|  |- PHL 224E5-1 (NVIDIA High Defini
	|  |- {E30629D1-27E5-11CE-875D-00608CB78066} (0)
	|  |- Pins (1)
	|  |  |- 0: PINDIR_INPUT    = Audio Input pin (rendered)
	|- 1
	|  |- Default DirectSound Device
	|  |- {79376820-07D0-11CF-A24D-0020AFD79767} (0)
	|  |- Pins (1)
	|  |  |- 0: PINDIR_INPUT    = Audio Input pin (rendered)
	: (中略)
	|- 7
	|  |- スピーカー / ヘッドフォン (Realtek High De
	|  |- {E30629D1-27E5-11CE-875D-00608CB78066} (0)
	|  |- Pins (1)
	|  |  |- 0: PINDIR_INPUT    = Audio Input pin (rendered)

----

**Sample11: カメラから画像を取り込む処理**

UVC 対応カメラの露光を開始し、画像オブジェクト (System.Drawing.Bitmap) に変換する処理です。  

出力結果  

	Sample11
	Run ...
	Running ... 241.008 msec
	0: SampleTime=0.137376
	1: SampleTime=0.170710
	: (中略)
	78: SampleTime=2.737374
	Stop ... 3000.465 msec
	Save ... 3233.804 msec
	Completed. 5604.801 msec


下記ディレクトリに取り込んだ画像を保存しています。  
※ 3,000 msec 経過すると露光を停止しています。取り込み枚数は不定です。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ Sample11    … 画像ファイルが格納されたディレクトリ
	││├ image0.png
	││├ image1.png
	││：(中略)
	││├ image78.png

----

**Sample12: カメラから画像を取り込み、動画ファイル(avi)へ保存する処理**

UVC 対応カメラの露光を開始し、AVI 形式の動画ファイルに保存する処理です。  

出力結果  

	Sample12
	Run ...
	Running ... 232.411 msec
	Stop ... 3000.730 msec
	Completed. 3269.791 msec


下記ディレクトリに動画ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ Sample12.avi … 動画ファイル

----

**Sample13: カメラから画像を取り込み、動画ファイル(wmv)へ保存する処理**

UVC 対応カメラの露光を開始し、WMV 形式の動画ファイルに保存する処理です。  

出力結果  

	Sample13
	Run ...
	Running ... 273.470 msec
	Stop ... 5000.198 msec
	Completed. 5211.055 msec


下記ディレクトリに動画ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ Sample13.wmv … 動画ファイル

----

**Sample14: カメラ制御とビデオ品質制御**

UVC 対応カメラのカメラ制御とビデオ品質制御のプロパティを取得する処理です。  

下記出力結果の CameraControlProperty は IAMCameraControl インターフェースを介してカメラ制御プロパティを取得したものです。
VideoProcAmpProperty は IAMVideoProcAmp インターフェースを介してビデオ品質制御プロパティを取得したものです。
各プロパティの行頭のマーク ([O]/[-]) は、対応/非対応 を示しています。

出力結果  

	Sample14
	
	CameraControlProperty
	Completed. 34.594 msec
	--- Name                  : Value,   Def,   Flag, Step, Lower ~ Upper
	[O] Pan                   :     0,     0, 0x0002,    1,   -56 ~    56
	[O] Tilt                  :     0,     0, 0x0002,    1,   -56 ~    56
	[-] Roll                  :
	[O] Zoom                  :     0,     0, 0x0002,    1,     0 ~    10
	[O] Exposure              :    -8,    -6, 0x0003,    1,   -11 ~     1
	[-] Iris                  :
	[O] Focus                 :    11,     0, 0x0003,    1,     0 ~    40
	
	VideoProcAmpProperty
	Completed. 26.618 msec
	--- Name                  : Value,   Def,   Flag, Step, Lower ~ Upper
	[O] Brightness            :    30,   133, 0x0002,    1,    30 ~   255
	[O] Contrast              :     5,     5, 0x0002,    1,     0 ~    10
	[-] Hue                   :
	[O] Saturation            :    83,    83, 0x0002,    1,     0 ~   200
	[O] Sharpness             :    25,    25, 0x0002,    1,     0 ~    50
	[-] Gamma                 :
	[-] ColorEnable           :
	[O] WhiteBalance          :  6400,  4500, 0x0003,    1,  2800 ~ 10000
	[O] BacklightCompensation :     0,     0, 0x0002,    1,     0 ~    10
	[-] Gain                  :

----

**Sample21: AVI 形式のファイルを読み込む処理**

AVI 形式の動画ファイルを読み込み、各フレームを画像オブジェクト (System.Drawing.Bitmap) に変換する処理です。  

出力結果  

	Sample21
	Run ...
	0: SampleTime=0.133333
	Running ... 130.043 msec
	1: SampleTime=0.166667
	2: SampleTime=0.200000
	: (中略)
	55: SampleTime=1.966665
	Save ... 2139.486 msec
	Completed. 2651.408 msec

下記ディレクトリに GRF ファイルと画像ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Sample21.GRF … GRF ファイル
	├ Results
	│├ Sample21    … 画像ファイルが格納されたディレクトリ
	││├ image0.png
	││├ image1.png
	││： (中略)
	││├ image55.png

下図は、保存した画像ファイル (全 56 ファイル) の一部のプレビューです。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/1/f/1fe5b877-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/1/f/1fe5b877.png)  

GRF ファイルは graphedt.exe (Windows SDK 同梱) で開くと下図のように視覚的に確認できます。  
GRF ファイルを開く際、動画ファイルとの相対位置が異なるとエラーが発生して起動しませんのでご注意ください。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/b/0/b0c162c9-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/b/0/b0c162c9.png)  

----

**Sample22: AVI 形式のファイルを読み込み、WMV 形式のファイルに保存する処理**

AVI 形式の動画ファイルを WMV 形式の動画ファイルに変換する処理です。  

出力結果  

	Sample22
	Run ...
	Running ... 14.243 msec
	Completed. 926.238 msec

下記ディレクトリに GRF ファイルと動画ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Sample22.GRF … GRF ファイル
	├ Results
	│├ Sample22.wmv … 動画ファイル

下図は、保存した動画ファイルを Windows Media Player で再生した例です。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/8/5/8531d32f-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/8/5/8531d32f.png)  

GRF ファイルは graphedt.exe (Windows SDK 同梱) で開くと下図のように視覚的に確認できます。  
GRF ファイルを開く際、動画ファイルとの相対位置が異なるとエラーが発生して起動しませんのでご注意ください。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/2/8/282b3086-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/2/8/282b3086.png)  

----

**Sample31: WMV 形式のファイルを読み込む処理**

WMV 形式の動画ファイルを読み込み、各フレームを画像オブジェクト (System.Drawing.Bitmap) に変換する処理です。  

出力結果  

	Sample31
	Run ...
	0: SampleTime=0.140000
	Running ... 14.422 msec
	1: SampleTime=0.173000
	2: SampleTime=0.206000
	: (中略)
	55: SampleTime=1.973000
	Save ... 2088.671 msec
	Completed. 2475.479 msec

下記ディレクトリに GRF ファイルと画像ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Sample31.GRF … GRF ファイル
	├ Results
	│├ Sample31    … 画像ファイルが格納されたディレクトリ
	││├ image0.png
	││├ image1.png
	││： (中略)
	││├ image55.png

下図は、保存した画像ファイル (全 56 ファイル) の一部のプレビューです。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/a/f/af66195f-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/a/f/af66195f.png)  

GRF ファイルは graphedt.exe (Windows SDK 同梱) で開くと下図のように視覚的に確認できます。  
GRF ファイルを開く際、動画ファイルとの相対位置が異なるとエラーが発生して起動しませんのでご注意ください。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/3/9/392438f8-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/3/9/392438f8.png)  

----

**Sample32: WMV 形式のファイルを読み込み、AVI 形式のファイルに保存する処理**

WMV 形式の動画ファイルを AVI 形式の動画ファイルに変換する処理です。  

出力結果  

	Sample32
	Run ...
	Running ... 8.176 msec
	Completed. 317.305 msec

下記ディレクトリに GRF ファイルと動画ファイルを保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Sample32.GRF … GRF ファイル
	├ Results
	│├ Sample32.avi … 動画ファイル

下図は、保存した動画ファイルを Windows Media Player で再生した例です。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/6/1/614d93f2-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/6/1/614d93f2.png)  

GRF ファイルは graphedt.exe (Windows SDK 同梱) で開くと下図のように視覚的に確認できます。  
GRF ファイルを開く際、動画ファイルとの相対位置が異なるとエラーが発生して起動しませんのでご注意ください。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/8/d/8d0908ce-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/8/d/8d0908ce.png)  
