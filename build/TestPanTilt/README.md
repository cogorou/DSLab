TestPanTilt
===

.NET Framework から DirectShow を使用するサンプルです。  
ここでは、カメラ制御の例を示します。  

## ビルド

予め [DSLab.Core](../DSLab.Core/README.md) をビルドしておく必要があります。  
その後、プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	$ ＿build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	$ ＿clean.bat


## 実行

$(TargetDir) に移動して demo.exe を実行してください。  

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/5/c/5c806253-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/5/c/5c806253.png)  

ウィンドウが起動したら、左上のカメラのアイコンをクリックしてカメラを接続してください。  
カメラの取り込みが始まると TrackBar で下記の操作が行えます。  
※ カメラが対応していない場合は TrackBar が無効化されます。  

1. Focus （焦点調整）
2. Exposure （露出調整）
3. Pan （パン制御）
4. Tilt （チルト制御）
5. Reset （原点復帰）

----

<b>Focus （焦点調整）</b><br/>

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/0/5/05d2c80b-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/0/5/05d2c80b.png)  

<b>Exposure （露出調整）</b><br/>

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/e/5/e5a3d659-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/e/5/e5a3d659.png)  

<b>Pan （パン制御）</b><br/>

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/5/2/522cbe78-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/5/2/522cbe78.png)  

<b>Tilt （チルト制御）</b><br/>

![Preview](http://livedoor.blogimg.jp/cogorou/imgs/f/d/fd4ee935-s.png)  
[拡大表示](http://livedoor.blogimg.jp/cogorou/imgs/f/d/fd4ee935.png)  

<b>Reset （原点復帰）</b><br/>
画面右下の [R] ボタンを押下するとメカニカルリセットを行います。  
