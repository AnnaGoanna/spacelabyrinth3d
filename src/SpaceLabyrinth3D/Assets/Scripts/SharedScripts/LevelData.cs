using System.Collections.Generic;
using UnityEngine;

namespace SharedScripts
{
    public static class LevelData // TODO think if you could merge the two below into one (e.g. filenames would be R, RF ...)
    {
        public static readonly string[] SegmentModelNames =
        {
            "segm0",
            "segm1",
            "segm2a",
            "segm2b",
            "segm3a",
            "segm3b",
            "segm4a",
            "segm4b",
            "segm5",
            "segm6"
        };

        public static readonly string[] SegmentModelDefaultIds =
        {
            "",
            "R",
            "LR",
            "RB",
            "LRB",
            "RUB",
            "LRUD",
            "LRUB",
            "LRUDB",
            "LRUDFB"
        };

        public static readonly Dictionary<string, Segment.Representation> SegmentIdMapping = new Dictionary<string, Segment.Representation>
        {
            {"",       new Segment.Representation(0, Vector3.zero)},

            {"L",      new Segment.Representation(1, new Vector3(  0,   0, 180))},
            {"R",      new Segment.Representation(1, Vector3.zero)},
            {"U",      new Segment.Representation(1, new Vector3(  0,   0,  90))},
            {"D",      new Segment.Representation(1, new Vector3(  0,   0, -90))},
            {"F",      new Segment.Representation(1, new Vector3(  0,  90,   0))},
            {"B",      new Segment.Representation(1, new Vector3(  0, -90,   0))},

            {"LR",     new Segment.Representation(2, Vector3.zero)},
            {"UD",     new Segment.Representation(2, new Vector3(  0,   0,  90))},
            {"FB",     new Segment.Representation(2, new Vector3(  0,  90,   0))},

            {"LU",     new Segment.Representation(3, new Vector3(  0, -90,  90))},
            {"LD",     new Segment.Representation(3, new Vector3(  0, -90, -90))},
            {"LF",     new Segment.Representation(3, new Vector3(  0, -90, 180))},
            {"LB",     new Segment.Representation(3, new Vector3(  0, -90,   0))},
            {"RU",     new Segment.Representation(3, new Vector3(-90,   0,   0))},
            {"RD",     new Segment.Representation(3, new Vector3( 90,   0,   0))},
            {"RF",     new Segment.Representation(3, new Vector3(  0,  90,   0))},
            {"RB",     new Segment.Representation(3, Vector3.zero)},
            {"UB",     new Segment.Representation(3, new Vector3(  0,   0,   90))},
            {"DB",     new Segment.Representation(3, new Vector3(  0,   0,  -90))},
            {"UF",     new Segment.Representation(3, new Vector3(-90,  90,    0))},
            {"DF",     new Segment.Representation(3, new Vector3( 90,  90,    0))},

            {"LRU",    new Segment.Representation(4, new Vector3(-90,   0,   0))},
            {"LRD",    new Segment.Representation(4, new Vector3( 90,   0,   0))},
            {"LRF",    new Segment.Representation(4, new Vector3(  0, 180,   0))},
            {"LRB",    new Segment.Representation(4, Vector3.zero)},
            {"LUD",    new Segment.Representation(4, new Vector3(  0, -90,  90))},
            {"LFB",    new Segment.Representation(4, new Vector3(  0, -90,   0))},
            {"RUD",    new Segment.Representation(4, new Vector3(  0,  90,  90))},
            {"RFB",    new Segment.Representation(4, new Vector3(  0,  90,   0))},
            {"UFB",    new Segment.Representation(4, new Vector3(-90,  90,   0))},
            {"DFB",    new Segment.Representation(4, new Vector3( 90,  90,   0))},
            {"UDF",    new Segment.Representation(4, new Vector3(  0, 180,  90))},
            {"UDB",    new Segment.Representation(4, new Vector3(  0,   0,  90))},

            {"LUF",    new Segment.Representation(5, new Vector3(  0, -90,  90))},
            {"LUB",    new Segment.Representation(5, new Vector3(  0, -90,   0))},
            {"LDF",    new Segment.Representation(5, new Vector3(  0, 180, -90))},
            {"LDB",    new Segment.Representation(5, new Vector3(  0, -90, -90))},
            {"RUF",    new Segment.Representation(5, new Vector3(  0,  90,   0))},
            {"RUB",    new Segment.Representation(5, Vector3.zero)},
            {"RDF",    new Segment.Representation(5, new Vector3(  0,  90, -90))},
            {"RDB",    new Segment.Representation(5, new Vector3( 90,   0,   0))},

            {"LRUD",   new Segment.Representation(6, Vector3.zero)},
            {"LRFB",   new Segment.Representation(6, new Vector3( 90,   0,   0))},
            {"UDFB",   new Segment.Representation(6, new Vector3(  0,  90,   0))},

            {"LRUF",   new Segment.Representation(7, new Vector3(  0, 180,   0))},
            {"LRUB",   new Segment.Representation(7, Vector3.zero)},
            {"LRDF",   new Segment.Representation(7, new Vector3( 90, 180,   0))},
            {"LRDB",   new Segment.Representation(7, new Vector3( 90,   0,   0))},
            {"LUFB",   new Segment.Representation(7, new Vector3(  0, -90,   0))},
            {"LUDF",   new Segment.Representation(7, new Vector3(  0, -90,  90))},
            {"LUDB",   new Segment.Representation(7, new Vector3(  0,   0,  90))},
            {"LDFB",   new Segment.Representation(7, new Vector3( 90, -90,   0))},
            {"RUFB",   new Segment.Representation(7, new Vector3(  0,  90,   0))},
            {"RDFB",   new Segment.Representation(7, new Vector3(  0,  90, 180))},
            {"RUDF",   new Segment.Representation(7, new Vector3(  0, 180,  90))},
            {"RUDB",   new Segment.Representation(7, new Vector3(  0,  90,  90))},

            {"LRUDF",  new Segment.Representation(8, new Vector3(  0,  180,   0))},
            {"LRUDB",  new Segment.Representation(8, Vector3.zero)},
            {"LRUFB",  new Segment.Representation(8, new Vector3(-90,   0,   0))},
            {"LRDFB",  new Segment.Representation(8, new Vector3( 90,   0,   0))},
            {"LUDFB",  new Segment.Representation(8, new Vector3(  0,  -90,   0))},
            {"RUDFB",  new Segment.Representation(8, new Vector3(  0,  90,   0))},

            {"LRUDFB", new Segment.Representation(9, Vector3.zero)},
        };
    }
}
