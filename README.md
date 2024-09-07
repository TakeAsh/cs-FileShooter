# FileShooter 
Moves the files into subfolders according to its name

![ApplicationIcon](https://raw.githubusercontent.com/TakeAsh/cs-FileShooter/master/Utils/ApplicationIcon128.png)

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
