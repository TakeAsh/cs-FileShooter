﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileShooter.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WindowPlacement {
            get {
                return ((string)(this["WindowPlacement"]));
            }
            set {
                this["WindowPlacement"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string TargetFolder {
            get {
                return ((string)(this["TargetFolder"]));
            }
            set {
                this["TargetFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("!#()_~▽「」『』【】〔〕・＂＜＞")]
        public string BoundaryChars {
            get {
                return ((string)(this["BoundaryChars"]));
            }
            set {
                this["BoundaryChars"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"アニメ・ステラー
---
アニソンポッド
アニソンミュージアム
  エフエムさがみ
Anime & Seiyu Music Night
NHK MUSIC
アニソン・アカデミー
アニソンプレミアムRADIO
アニソン89秒の世界
歌謡スクランブル
青春ラジメニア
twilight Club DJ MIX
Hits 200
こだわりセットリスト
クラシックの迷宮
ミュージックライン
名曲ヒットパレード
音楽遊覧飛行
×クラシック
ふたりはプリキュア
ふたりはプリキュアMaxHeart
ふたりはプリキュアSplash☆Star
Yes!プリキュア5
Yes!プリキュア5GoGo!
フレッシュプリキュア!
ハートキャッチプリキュア!
スイートプリキュア♪
スマイルプリキュア!
ドキドキ!プリキュア
ハピネスチャージプリキュア!
Go!プリンセスプリキュア
魔法つかいプリキュア!
キラキラ☆プリキュアアラモード
HUGっと!プリキュア
スター☆トゥインクルプリキュア
ヒーリングっど・プリキュア
トロピカル～ジュ!プリキュア
デリシャスパーティ♡プリキュア
  デリシャスパーティ・プリキュア
ひろがるスカイ!プリキュア
わんだふるぷりきゅあ!
angela
  angelaのsparking! talking! show!
水樹奈々
  水樹奈々スマイル・ギャング
谷山浩子
  谷山浩子ノセカイ
林原めぐみ
  林原めぐみのTokyo Boogie Night
井澤詩織
  井澤詩織のしーちゃんねる
○○!ゲームおんがく きこうぜ!
---
ラジオ
  RaNi Music♪
アニソン♪おとのくに
  Live Music
  FMおとくに
ラジオ深夜便
調布FM
")]
        public string Titles {
            get {
                return ((string)(this["Titles"]));
            }
            set {
                this["Titles"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\(生放送\\)\r\n生放送・\r\n配信限定\r\nTVアニメ\r\n^特撮ドラマ\\s+\r\n^アニメ\\s+")]
        public string Labels {
            get {
                return ((string)(this["Labels"]));
            }
            set {
                this["Labels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("- RaNi Music♪Start\r\n  RaNi Music♪ Start\r\n- RaNi Music♪Morning\r\n  RaNi Music♪ Morn" +
            "ing\r\n- RaNi Music♪Day\r\n  RaNi Music♪ Day\r\n- RaNi Music♪Evening\r\n  RaNi Music♪ Ev" +
            "ening\r\n- ×(かける)クラシック\r\n  ×クラシック\r\n- トロピカル~ジュ!プリキュア\r\n  トロピカル～ジュ!プリキュア")]
        public string Preprocess {
            get {
                return ((string)(this["Preprocess"]));
            }
            set {
                this["Preprocess"] = value;
            }
        }
    }
}
