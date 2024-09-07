using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter {

    public static class TargetFolderHelper {

        public static string ToTargetFolder(this string text) {
            return String.IsNullOrEmpty(text) || !Directory.Exists(text)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                : text;
        }
    }
}
