# FileShooter ![ApplicationIcon](https://raw.githubusercontent.com/TakeAsh/cs-FileShooter/master/Utils/ApplicationIcon128.png)
Moves the files into subfolders according to its name

## Examples

| File name | Folder to be moved |
| --- | --- |
| アニメ・ステラー ラブライブ!音楽特集 r1 2024-09-03 20-05.m4a | アニメ・ステラー |
| ×(かける)クラシック▽第187駅 ゲーム×クラシック(3) fm 2024-05-31 07-25.m4a | ×クラシック |
| 林原めぐみのTokyo Boogie Night CRK 2024-09-07 23-00.m4a | 林原めぐみ |
| Live Music アニおと 林原めぐみ FMおとくに 2023-11-16 22-00.m4a | 林原めぐみ |
| Live Music アニおと 子門真人 FMおとくに 2024-07-04 22-00.m4a | アニソン♪おとのくに |
| 調布FM『アニソンカバーズ』 調布FM 2024-01-01 15-00.m4a | 調布FM |
| RaNi Music♪Morning RN2 2024-08-28 10-00.m4a | ラジオ |
| エフエムさがみ_20240812100028293.asf | アニソンミュージアム |

## How to use

1. Launch the application<br>
![Tab_Run](https://raw.githubusercontent.com/TakeAsh/cs-FileShooter/master/Utils/Tab_Run.png)
1. Open tab `Settings`
1. There are 4 tabs (`Basic`, `Titles`, `Labels`, `Preprocess`) under `Settings`<br>
![Tab_Settings](https://raw.githubusercontent.com/TakeAsh/cs-FileShooter/master/Utils/Tab_Settings.png)
1. Configure each items

    | Item | Meaning |
    | --- | --- |
    | Target Folder | The folder haves the files to should be moved. |
    | Boundary Chars | These characters divide the file name into main title and sub title.<br>Implicitly contains `\s`, `\b`, `-`. |
    | Titles | The main titles are used for the sub folder name.<br>When it starts with 2 spaces, it is a supplemental keyword of above title, and the file contains this keyword is moved to the same title folder.<br>`---` is the separator, and the former group has higher priority. |
    | Labels | The redundant word to be removed. These words are recognized as the regular expression. The special characters must be escaped.|
    | Preprocess | Before moving, the file names are processed, if they are matched to the first line (starts with `- `), replaced with the second line (starts with `  `). |

1. Open tab `Run`
1. Click button `Run`, then start running
1. At first, normalize file names
1. Next, move files into sub folders
1. If there are any troubles, click button `Stop` to stop
1. You can erase `Log` and `Error Log` to click button `Erase`
1. Click button `Exit`
