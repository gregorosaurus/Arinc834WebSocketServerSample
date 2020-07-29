using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ARINC834WebSocketsSample
{
    /// <summary>
    /// Just emulates a data request to avionics.
    /// For this example, we just iterate through an array of data.
    /// Really, this class isn't important for the demo purpose.
    /// Focus on the WebSocketsDataStream class instead.
    /// </summary>
    public class SimpleDataSource : IARINCDataSource
    {
        #region TEST DATA, not important
        private class TestDataStoreObject
        {
            public double? LatitudeHighPrecision { get; set; }
            public double? LongitudeHightPrecision { get; set; }
            public double? IrsTrueHeading { get; set; }
            public double? GnssTrackAngle { get; set; }
            public double? BaroCorrectedAltitude { get; set; }
            public double? ComputedAirspeed { get; set; }
            public double? IrsGroundSpeed { get; set; }
            public double? StaticAirTemp { get; set; }
            public double? DistanceToDestination { get; set; }
            public double? TimeToDestination { get; set; }
            public double? WindSpeed { get; set; }
            public double? WindDirection { get; set; }

            public TestDataStoreObject(
                double? latitudeHighPrecision,
                double? longitudeHighPrecision,
                double? irsTrueHeading,
                double? gnssTrackAngle,
                double? baroCorrectedAltitude,
                double? computedAirspeed,
                double? irsGroundSpeed,
                double? staticAirTemp,
                double? distanceToDestination,
                double? timeToDestination,
                double? windSpeed,
                double? windDirection
                )
            {
                this.LatitudeHighPrecision = latitudeHighPrecision;
                this.LongitudeHightPrecision = longitudeHighPrecision;
                this.IrsTrueHeading = irsTrueHeading;
                this.GnssTrackAngle = gnssTrackAngle;
                this.BaroCorrectedAltitude = baroCorrectedAltitude;
                this.ComputedAirspeed = computedAirspeed;
                this.IrsGroundSpeed = irsGroundSpeed;
                this.StaticAirTemp = staticAirTemp;
                this.DistanceToDestination = distanceToDestination;
                this.TimeToDestination = timeToDestination;
                this.WindSpeed = windSpeed;
                this.WindDirection = windDirection;

            }
        }

        #endregion

        private TestDataStoreObject[] TestDataStore = new TestDataStoreObject[]
        {
            new TestDataStoreObject(48.64196777,-123.4300232,313.5813904,313.59375,54,null,0,15,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.5498047,313.59375,54,null,0,15.25,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.5566711,313.59375,58,null,0,15.5,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.6122894,313.6816406,58,null,0,16,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.6308289,313.6816406,58,null,0,16,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.6528015,313.6816406,58,null,0,15.75,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.6562347,313.6816406,58,null,0,15.75,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.680954,313.7695313,54,null,0,15.75,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4300232,313.680954,313.7695313,52,null,0,16,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4307098,285.2881622,289.5996094,48,null,9.75,15.5,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4307098,279.1378784,279.4921875,48,null,9.25,15.5,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4341431,280.2371979,280.546875,52,null,14.5,15.5,null,null,null,null),
            new TestDataStoreObject(48.64196777,-123.4348297,290.206604,284.2382813,52,null,14.75,15.5,null,null,null,null),
            new TestDataStoreObject(48.64471436,-123.4320831,45.76629639,45.52734375,48,null,20.25,15.5,null,null,null,null),
            new TestDataStoreObject(48.64471436,-123.4313965,45.65231323,45.43945313,48,null,17.5,15.5,null,null,null,null),
            new TestDataStoreObject(48.64746094,-123.4313965,334.0585327,334.1601563,48,null,17.75,15.25,null,null,null,null),
            new TestDataStoreObject(48.64746094,-123.4320831,334.6847534,334.5117188,48,null,18.5,15.5,null,null,null,null),
            new TestDataStoreObject(48.64952087,-123.4348297,287.1118927,287.2265625,52,null,18.5,15.25,null,null,null,null),
            new TestDataStoreObject(48.64952087,-123.4355164,287.5252533,287.2265625,52,null,18.5,15.5,null,null,null,null),
            new TestDataStoreObject(48.65089417,-123.4389496,331.7857361,331.6992188,52,null,10.25,15.5,null,null,null,null),
            new TestDataStoreObject(48.65089417,-123.4396362,331.7205048,331.6992188,52,null,12,15.5,null,null,null,null),
            new TestDataStoreObject(48.65020752,-123.4334564,107.2348022,107.2265625,100,94.49677002,83.25,14.75,null,null,null,null),
            new TestDataStoreObject(48.64952087,-123.4300232,107.047348,106.7871094,120,120.3708856,106.75,14.5,null,null,null,null),
            new TestDataStoreObject(48.64677429,-123.3901978,76.43943787,73.125,1174,162.2444543,161.5,13.75,null,null,12,142.03125),
            new TestDataStoreObject(48.64814758,-123.3847046,74.87113953,71.80664063,1370,163.6194073,165.75,13.75,null,null,12,147.65625),
            new TestDataStoreObject(48.65844727,-123.3345795,77.79693604,75.234375,2604,182.6187579,202.25,12.25,null,null,14,222.1875),
            new TestDataStoreObject(48.65982056,-123.327713,77.49824524,74.97070313,2742,190.2434973,205.75,11.75,null,null,16,225),
            new TestDataStoreObject(48.67492676,-123.2714081,62.33161926,60.46875,4268,207.8678949,222.75,11,null,null,18,222.1875),
            new TestDataStoreObject(48.67767334,-123.2652283,61.28242493,59.58984375,4478,208.6178693,223.5,10.75,null,null,18,219.375),
            new TestDataStoreObject(48.69895935,-123.2123566,62.18673706,57.48046875,6386,207.1179205,213.5,8.5,null,null,14,184.21875),
            new TestDataStoreObject(48.70170593,-123.2061768,62.65296936,57.56835938,6678,204.4930103,210.25,8,null,null,16,175.78125),
            new TestDataStoreObject(48.7223053,-123.1567383,60.28129578,58.359375,8490,193.3683905,208.25,5.5,null,null,14,195.46875),
            new TestDataStoreObject(48.72436523,-123.1505585,60.26756287,58.44726563,8664,193.618382,210.5,5.5,null,null,14,203.90625),
            new TestDataStoreObject(48.7449646,-123.0976868,60.92193604,59.765625,9996,197.6182452,229,3.5,null,null,26,232.03125),
            new TestDataStoreObject(48.74771118,-123.0908203,60.32730103,59.94140625,10144,198.7432068,232,3.5,null,null,28,233.4375),
            new TestDataStoreObject(48.77037048,-123.0324554,60.1171875,59.94140625,11430,206.2429504,246,2,null,null,38,240.46875),
            new TestDataStoreObject(48.77311707,-123.0249023,60.28678894,59.85351563,11606,205.8679633,247,1.75,null,null,38,239.0625),
            new TestDataStoreObject(48.79714966,-122.961731,61.54403687,60.55664063,12418,239.2418225,282.5,-1.5,null,null,42,236.25),
            new TestDataStoreObject(48.80058289,-122.9528046,61.61956787,60.64453125,12476,245.3666132,289.25,-1.75,null,null,44,236.25),
            new TestDataStoreObject(48.82873535,-122.8779602,58.62579346,58.79882813,13130,274.6156134,319.25,-3,null,null,44,239.0625),
            new TestDataStoreObject(48.83285522,-122.8690338,58.04969788,58.27148438,13240,277.8655023,321.75,-3.25,null,null,44,239.0625),
            new TestDataStoreObject(48.86375427,-122.7893829,66.2084198,64.16015625,14156,284.9902588,334.25,-4.75,null,null,48,239.0625),
            new TestDataStoreObject(48.86650085,-122.7783966,69.18914795,67.32421875,14268,286.3652118,335.25,-4.75,null,null,48,239.0625),
            new TestDataStoreObject(48.88710022,-122.6891327,74.2023468,71.98242188,15242,290.1150836,338.75,-6.75,null,null,48,240.46875),
            new TestDataStoreObject(48.88916016,-122.6781464,73.89060974,71.71875,15364,292.1150153,339.25,-7,null,null,48,240.46875),
            new TestDataStoreObject(48.90907288,-122.5868225,73.80821228,71.3671875,16338,292.1150153,342.75,-9,null,null,50,239.0625),
            new TestDataStoreObject(48.91181946,-122.5758362,73.76976013,71.19140625,16432,291.2400452,343.25,-9,null,null,50,239.0625),
            new TestDataStoreObject(48.93241882,-122.4838257,74.64179993,71.45507813,17342,291.6150323,347,-10.25,null,null,56,236.25),
            new TestDataStoreObject(48.93447876,-122.4721527,74.56352234,71.3671875,17444,291.7400281,347.25,-10.5,null,null,56,236.25),
            new TestDataStoreObject(48.95507813,-122.3794556,74.43443298,71.19140625,18326,292.6149982,348.25,-12,null,null,58,236.25),
            new TestDataStoreObject(48.95782471,-122.3677826,74.78187561,71.27929688,18434,293.8649554,348.25,-12.25,null,null,56,236.25),
            new TestDataStoreObject(48.97842407,-122.2743988,74.35272217,71.45507813,18970,302.8646478,360,-14.25,null,null,58,236.25),
            new TestDataStoreObject(48.9818573,-122.2606659,74.0574646,71.54296875,18988,307.1145026,363.75,-14.5,null,null,58,237.65625),
            new TestDataStoreObject(49.00314331,-122.1624756,73.43330383,71.3671875,19012,332.1136481,386.75,-14.75,null,null,56,237.65625),
            new TestDataStoreObject(49.00657654,-122.1473694,73.09135437,71.10351563,18996,333.1136139,388.75,-14.5,null,null,56,239.0625),
            new TestDataStoreObject(49.02923584,-122.0443726,74.26963806,71.71875,18996,341.2383362,398.75,-14.5,null,null,56,237.65625),
            new TestDataStoreObject(49.03266907,-122.0285797,74.67407227,71.98242188,18998,341.7383191,399.5,-14.5,null,null,58,237.65625),
            new TestDataStoreObject(49.05601501,-121.9228363,73.61251831,71.19140625,19008,348.8630756,406.25,-15.25,null,null,58,240.46875),
            new TestDataStoreObject(49.05944824,-121.9070435,73.54660034,71.45507813,18998,348.6130841,406.5,-15.25,null,null,58,239.0625),
            new TestDataStoreObject(49.08210754,-121.8006134,73.93592834,71.54296875,18992,352.6129474,407.5,-15.25,null,null,58,239.0625),
            new TestDataStoreObject(49.08554077,-121.7848206,74.12200928,71.54296875,18996,350.6130157,407.75,-15.25,null,null,58,239.0625),
            new TestDataStoreObject(49.10888672,-121.6777039,74.42962646,71.98242188,19002,351.2379944,407.75,-15,null,null,58,240.46875),
            new TestDataStoreObject(49.11231995,-121.661911,74.07394409,71.89453125,19002,349.6130499,407.5,-14.75,null,null,58,240.46875),
            new TestDataStoreObject(49.13497925,-121.5582275,73.89404297,71.80664063,19872,329.863725,384.75,-14.75,null,null,56,240.46875),
            new TestDataStoreObject(49.13772583,-121.5431213,74.30603027,72.0703125,19992,327.8637933,382.25,-14.75,null,null,56,240.46875),
            new TestDataStoreObject(49.1583252,-121.4456177,72.88673401,71.89453125,20842,317.9891309,362,-15.75,null,null,44,243.28125),
            new TestDataStoreObject(49.16175842,-121.4318848,73.04878235,71.98242188,20912,318.364118,361.75,-15.75,null,null,44,244.6875),
            new TestDataStoreObject(49.18167114,-121.3357544,72.82493591,72.15820313,21004,324.6139044,371,-16.75,null,null,44,248.90625),
            new TestDataStoreObject(49.18510437,-121.3206482,72.5592041,72.15820313,21006,325.8638617,372,-16.75,null,null,46,248.90625),
            new TestDataStoreObject(49.20570374,-121.2217712,72.48504639,72.24609375,20994,331.8636566,378.25,-17,null,null,46,250.3125),
            new TestDataStoreObject(49.20913696,-121.206665,72.65327454,72.33398438,21004,332.1136481,378.5,-17,null,null,46,251.71875),
            new TestDataStoreObject(49.22973633,-121.1064148,71.90620422,71.98242188,21000,336.4884985,384.75,-17.25,null,null,48,251.71875),
            new TestDataStoreObject(49.23316956,-121.0913086,71.89247131,71.98242188,20994,336.73849,385.5,-17.25,null,null,48,253.125),
            new TestDataStoreObject(49.25445557,-120.9889984,71.89178467,72.0703125,21006,340.6133575,386.5,-17.5,null,null,46,254.53125),
            new TestDataStoreObject(49.25788879,-120.9745789,71.98104858,72.0703125,21000,340.7383533,387.25,-17.5,null,null,46,253.125),
            new TestDataStoreObject(49.2791748,-120.871582,72.85652161,72.68554688,21010,343.3632635,389.75,-17.75,null,null,46,253.125),
            new TestDataStoreObject(49.28192139,-120.8564758,72.87849426,72.59765625,21000,344.1132379,390,-17.75,null,null,46,253.125),
            new TestDataStoreObject(49.30389404,-120.753479,72.61138916,72.50976563,20996,347.3631268,391.25,-18,null,null,44,251.71875),
            new TestDataStoreObject(49.30664063,-120.7383728,72.8427887,72.68554688,20994,347.3631268,391.25,-18,null,null,44,251.71875),
            new TestDataStoreObject(49.32792664,-120.6346893,72.6134491,72.24609375,21004,348.3630927,392,-18,null,null,44,250.3125),
            new TestDataStoreObject(49.33135986,-120.6188965,72.60658264,72.33398438,21000,348.6130841,392.5,-18,null,null,44,250.3125),
            new TestDataStoreObject(49.35264587,-120.515213,72.64915466,72.59765625,21004,348.2380969,391.75,-18,null,null,42,253.125),
            new TestDataStoreObject(49.35539246,-120.5001068,72.2694397,72.33398438,21006,347.8631097,392,-17.75,null,null,42,253.125),
            new TestDataStoreObject(49.37805176,-120.3964233,62.96676636,64.6875,20976,348.2380969,390,-18,null,null,42,251.71875),
            new TestDataStoreObject(49.38354492,-120.3826904,54.40361023,56.953125,20994,348.3630927,387.5,-18,null,null,42,253.125),
            new TestDataStoreObject(49.4329834,-120.3092194,33.19107056,37.96875,20508,347.2381311,381.75,-17.5,null,null,40,253.125),
            new TestDataStoreObject(49.44122314,-120.2996063,31.06384277,36.03515625,20396,347.8631097,379.75,-17.25,null,null,42,254.53125),
            new TestDataStoreObject(49.49752808,-120.2384949,30.12863159,35.859375,19398,344.4882251,383,-15.75,null,null,50,257.34375),
            new TestDataStoreObject(49.50576782,-120.2288818,30.33943176,36.03515625,19248,345.4881909,383.5,-15.5,null,null,50,257.34375),
            new TestDataStoreObject(49.56138611,-120.1643372,32.45429993,38.3203125,18250,346.4881567,385,-14,null,null,52,260.15625),
            new TestDataStoreObject(49.56962585,-120.1540375,32.49275208,38.14453125,18094,346.2381653,385.75,-13.5,null,null,52,260.15625),
            new TestDataStoreObject(49.6245575,-120.0888062,31.87889099,37.08984375,17066,346.7381482,388.5,-11.5,null,null,54,255.9375),
            new TestDataStoreObject(49.63348389,-120.0785065,32.00248718,37.44140625,16906,347.8631097,388.75,-11.25,null,null,54,255.9375),
            new TestDataStoreObject(49.68910217,-120.0132751,32.47146606,37.6171875,15886,345.6131866,384.25,-8.5,null,null,50,257.34375),
            new TestDataStoreObject(49.69734192,-120.0029755,32.59300232,37.44140625,15724,343.9882422,383.25,-8.5,null,null,50,255.9375),
            new TestDataStoreObject(49.75021362,-119.9384308,33.50967407,38.49609375,14718,327.4888062,362,-6,null,null,46,254.53125),
            new TestDataStoreObject(49.75776672,-119.9295044,32.76878357,38.05664063,14580,324.363913,357.25,-6,null,null,46,255.9375),
            new TestDataStoreObject(49.80789185,-119.8718262,31.46690369,37.17773438,13664,300.9897119,331.5,-4.25,null,null,42,260.15625),
            new TestDataStoreObject(49.8147583,-119.8628998,31.01921082,36.82617188,13546,297.1148444,327.5,-3.75,null,null,42,260.15625),
            new TestDataStoreObject(49.86351013,-119.8168945,12.65762329,20.390625,12664,276.2405579,301,-2.5,null,null,44,255.9375),
            new TestDataStoreObject(49.87106323,-119.8127747,10.76385498,18.45703125,12568,272.8656732,296.25,-2.5,null,null,44,255.9375),
            new TestDataStoreObject(49.91912842,-119.8052216,326.8466949,338.7304688,11734,257.1162115,245.5,-0.75,null,null,44,257.34375),
            new TestDataStoreObject(49.92530823,-119.8107147,315.3014374,326.8652344,11614,251.7413953,233,-0.5,null,null,44,257.34375),
            new TestDataStoreObject(49.93011475,-119.8615265,233.4175873,230.0976563,11046,239.8668011,202.5,0.5,null,null,42,258.75),
            new TestDataStoreObject(49.92599487,-119.8670197,220.2051544,217.3535156,11026,238.7418396,206.75,0.5,null,null,42,258.75),
            new TestDataStoreObject(49.88754272,-119.8526001,138.5863495,132.2753906,11010,236.741908,264.75,0.5,null,null,40,265.78125),
            new TestDataStoreObject(49.88342285,-119.8436737,127.1303558,122.5195313,11010,237.8668695,270.5,0.5,null,null,40,267.1875),
            new TestDataStoreObject(49.88616943,-119.769516,43.8924408,52.03125,10980,240.866767,276,0.75,null,null,44,270),
            new TestDataStoreObject(49.8916626,-119.7619629,31.79031372,40.78125,10976,240.4917798,270,0.75,null,null,44,270),
            new TestDataStoreObject(49.93217468,-119.7585297,311.3209534,322.8222656,10980,241.6167413,218,0.75,null,null,46,261.5625),
            new TestDataStoreObject(49.93629456,-119.7653961,299.6575928,309.1113281,10976,242.3667157,211.75,0.75,null,null,44,261.5625),
            new TestDataStoreObject(49.93080139,-119.8175812,224.8928833,219.7265625,10668,252.1163824,223,1,null,null,40,262.96875),
            new TestDataStoreObject(49.92530823,-119.8223877,213.4774017,207.2460938,10606,252.9913525,230.5,1,null,null,38,264.375),
            new TestDataStoreObject(49.88342285,-119.8086548,140.0289917,137.3730469,10248,248.1165192,270.75,1.5,null,null,36,268.59375),
            new TestDataStoreObject(49.87861633,-119.7990417,128.3924103,126.7382813,10196,247.2415491,275.5,1.5,null,null,36,270),
            new TestDataStoreObject(49.88067627,-119.7255707,47.34901428,53.87695313,9878,241.7417371,273,2,null,null,38,268.59375),
            new TestDataStoreObject(49.88548279,-119.7173309,35.32859802,42.36328125,9848,240.2417883,266.25,2,null,null,36,268.59375),
            new TestDataStoreObject(49.92805481,-119.6994781,352.3713684,0.615234375,9688,235.366955,237.5,1.5,null,null,32,264.375),
            new TestDataStoreObject(49.93423462,-119.7001648,350.3031921,358.59375,9658,235.2419592,235.5,1,null,null,32,264.375),
            new TestDataStoreObject(49.96856689,-119.7241974,285.2153778,292.0605469,9490,231.6170831,204.25,0.75,null,null,34,257.34375),
            new TestDataStoreObject(49.96925354,-119.7324371,272.2824097,279.2285156,9490,230.4921216,201.5,1,null,null,34,255.9375),
            new TestDataStoreObject(49.94522095,-119.7681427,184.7117615,179.8242188,9498,232.2420618,225.5,0.75,null,null,28,257.34375),
            new TestDataStoreObject(49.93904114,-119.7667694,171.9806671,166.5527344,9494,231.3670917,233.75,0.75,null,null,28,260.15625),
            new TestDataStoreObject(49.91500854,-119.7111511,86.48643494,87.97851563,9490,236.6169122,269.75,1,null,null,30,265.78125),
            new TestDataStoreObject(49.91638184,-119.7001648,73.54248047,76.37695313,9490,237.2418909,270.25,1.25,null,null,32,267.1875),
            new TestDataStoreObject(49.95140076,-119.6603394,349.4812775,359.6484375,9488,239.3668182,241.75,0.75,null,null,36,264.375),
            new TestDataStoreObject(49.95758057,-119.661026,343.3165741,352.6171875,9488,238.6168439,237.25,0.75,null,null,36,262.96875),
            new TestDataStoreObject(49.99740601,-119.6795654,332.7552795,340.9277344,9480,232.4920532,224.5,1.5,null,null,32,261.5625),
            new TestDataStoreObject(50.00358582,-119.6829987,331.8516541,340.6640625,9474,231.8670746,223.5,1.5,null,null,32,260.15625),
            new TestDataStoreObject(50.04203796,-119.7042847,334.4533539,340.2246094,9480,232.117066,227.75,1.25,null,null,22,258.75),
            new TestDataStoreObject(50.04753113,-119.7077179,334.3826294,340.4882813,9490,231.1171002,227.75,1.5,null,null,22,258.75),
            new TestDataStoreObject(50.08872986,-119.7077179,27.79197693,29.97070313,9386,233.1170319,258.5,1.5,null,null,24,244.6875),
            new TestDataStoreObject(50.09490967,-119.7008514,35.8367157,37.70507813,9294,236.3669208,265.5,1.5,null,null,26,241.875),
            new TestDataStoreObject(50.12512207,-119.6411133,56.98059082,57.3046875,8480,236.4919165,267.75,1.75,null,null,30,240.46875),
            new TestDataStoreObject(50.12924194,-119.6315002,56.98883057,57.3046875,8362,234.2419934,264.5,2,null,null,30,240.46875),
            new TestDataStoreObject(50.15464783,-119.5717621,58.17192078,56.07421875,7588,219.1175104,243,3,null,null,26,226.40625),
            new TestDataStoreObject(50.15808105,-119.5635223,58.01605225,55.98632813,7470,216.2426086,239.5,2.75,null,null,26,223.59375),
            new TestDataStoreObject(50.17799377,-119.5065308,78.54263306,72.50976563,6708,204.2430188,223,1.75,null,null,26,213.75),
            new TestDataStoreObject(50.17936707,-119.4976044,84.08866882,77.51953125,6594,204.1180231,222,2,null,null,26,213.75),
            new TestDataStoreObject(50.1738739,-119.4385529,124.8863983,116.2792969,5842,206.6179376,204.75,3.25,null,null,28,208.125),
            new TestDataStoreObject(50.17181396,-119.4309998,129.7615814,121.640625,5734,202.1180914,200.25,3.5,null,null,28,208.125),
            new TestDataStoreObject(50.14572144,-119.3959808,158.4812164,153.5449219,5084,187.7435828,181,3.25,null,null,16,217.96875),
            new TestDataStoreObject(50.14160156,-119.3932343,162.6525879,157.9394531,4980,187.618587,179.75,4,null,null,16,217.96875),
            new TestDataStoreObject(50.11070251,-119.3870544,187.0793152,185.4492188,4370,178.8688861,169.75,5.5,null,null,10,215.15625),
            new TestDataStoreObject(50.105896,-119.3877411,190.0140381,189.140625,4296,178.1189117,168.75,5.5,null,null,10,210.9375),
            new TestDataStoreObject(50.07705688,-119.4007874,184.7481537,186.4160156,3722,172.8690912,162.5,7,null,null,8,182.8125),
            new TestDataStoreObject(50.07293701,-119.4007874,180.844574,182.4609375,3644,171.6191339,162,7.25,null,null,10,178.59375),
            new TestDataStoreObject(50.0440979,-119.3966675,173.2042694,172.0898438,3100,152.7447791,151.25,8,null,null,4,208.125),
            new TestDataStoreObject(50.03997803,-119.3952942,174.212265,172.0898438,2994,146.1200055,146,8.25,null,null,4,226.40625),
            new TestDataStoreObject(50.01525879,-119.389801,171.206131,171.1230469,2482,129.2455823,134.25,9,null,null,4,324.84375),
            new TestDataStoreObject(50.01182556,-119.3891144,171.0440826,171.1230469,2410,129.2455823,134.5,9,null,null,4,338.90625),
            new TestDataStoreObject(49.98779297,-119.3829346,168.6909485,168.2226563,1888,133.3704413,136.75,9.75,null,null,4,352.96875),
            new TestDataStoreObject(49.9836731,-119.3822479,172.0108795,169.5410156,1806,131.7454968,136,9.75,null,null,4,347.34375),
            new TestDataStoreObject(49.9596405,-119.3788147,175.2422333,175.2539063,1376,122.7458044,123,11.25,null,null,4,319.21875),
            new TestDataStoreObject(49.95758057,-119.3781281,176.0002899,174.6386719,1374,93.24681274,92,11.75,null,null,2,312.1875),
            new TestDataStoreObject(49.95140076,-119.3774414,233.9620972,221.2207031,1358,null,11.5,12,null,null,null,null),
            new TestDataStoreObject(49.95140076,-119.3781281,265.174942,264.6386719,1358,null,12,11.75,null,null,null,null),
            new TestDataStoreObject(49.95002747,-119.3795013,252.3360443,247.2363281,1360,null,6.75,11.75,null,null,null,null),
            new TestDataStoreObject(49.95002747,-119.380188,267.1112823,263.9355469,1360,null,7.5,12,null,null,null,null),
            new TestDataStoreObject(49.95071411,-119.380188,306.1347198,306.1230469,1360,null,0,12,null,null,null,null),
            new TestDataStoreObject(49.95071411,-119.380188,306.1566925,306.2109375,1360,null,0,12,null,null,null,null),
            new TestDataStoreObject(49.95071411,-119.380188,306.3228607,306.3867188,1358,null,0,12.25,null,null,null,null)
        };

        private int currentIndex = 0;

        public async Task<List<A834DataParameter>> GetCurrentDataAsync()
        {
            //get the current data in our test data store.
            TestDataStoreObject currentTestObject = TestDataStore[currentIndex];
            currentIndex += 1;
            //check for rollover, for next time it's called.
            if (currentIndex >= TestDataStore.Length)
            {
                currentIndex = 0; 
            }

            List<A834DataParameter> dataToSend = new List<A834DataParameter>();
            //iterate through the properties and create a parameter to send 
            foreach(PropertyInfo property in typeof(TestDataStoreObject).GetProperties())
            {
                A834DataParameter parameter = new A834DataParameter();
                parameter.ParameterName = property.Name;
                parameter.Value = property.GetValue(currentTestObject)?.ToString() ?? "";
                parameter.TimeStamp = DateTime.UtcNow; //faux timestamp
                dataToSend.Add(parameter);
            }

            return await Task.FromResult(dataToSend);
        }
    }
}
