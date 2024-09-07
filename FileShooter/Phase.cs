using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileShooter {

    public enum Phase {
        [Description("Get Files")]
        GetFiles,
        [Description("Normalize Files")]
        NormalizeFiles,
        [Description("Move Files")]
        MoveFiles,
        [Description("Complete")]
        Complete,
    }

    public static class PhaseHelper {

        public static Phase Next(this Phase en) {
            return (Phase)(((int)en + 1) % ((int)Phase.Complete + 1));
        }
    }
}
