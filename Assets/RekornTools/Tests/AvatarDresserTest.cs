using NUnit.Framework;
using RekornTools.Avatar;

namespace Tests
{
    public sealed class AvatarDresserTest
    {
        [Test]
        public void TestNamingConverterA()
        {
            var src = new RigNamingConvention(ModifierPosition.Front, ".", "L", "R");
            var dst = new RigNamingConvention(ModifierPosition.Front, "_", "l", "r");

            var name1A = "L_Arm";
            var name1B = "L_Arm";
            var name2A = "L.Leg";
            var name2B = "l_Leg";
            var name3A = "Head.L";
            var name3B = "Head.L";
            var name4A = "R.Torso";
            var name4B = "r_Torso";

            Assert.AreEqual(name1B, RigNamingConvention.Convert(name1A, src, dst));
            Assert.AreEqual(name2B, RigNamingConvention.Convert(name2A, src, dst));
            Assert.AreEqual(name3B, RigNamingConvention.Convert(name3A, src, dst));
            Assert.AreEqual(name4B, RigNamingConvention.Convert(name4A, src, dst));
        }

        [Test]
        public void TestNamingConverterB()
        {
            var src = new RigNamingConvention(ModifierPosition.End,   ".", "L", "R");
            var dst = new RigNamingConvention(ModifierPosition.Front, "_", "l", "r");

            var name1A = "L_Arm";
            var name1B = "L_Arm";
            var name2A = "Leg.L";
            var name2B = "l_Leg";
            var name3A = "L.Head";
            var name3B = "L.Head";
            var name4A = "Torso.R";
            var name4B = "r_Torso";

            Assert.AreEqual(name1B, RigNamingConvention.Convert(name1A, src, dst));
            Assert.AreEqual(name2B, RigNamingConvention.Convert(name2A, src, dst));
            Assert.AreEqual(name3B, RigNamingConvention.Convert(name3A, src, dst));
            Assert.AreEqual(name4B, RigNamingConvention.Convert(name4A, src, dst));
        }

        [Test]
        public void TestNamingConverterC()
        {
            var src = new RigNamingConvention(ModifierPosition.End,   ".", "L", "R");
            var dst = new RigNamingConvention(ModifierPosition.Front, "_", "l", "r");

            var name1A = "L_Arm.001";
            var name1B = "L_Arm.001";
            var name2A = "Leg.L.001";
            var name2B = "l_Leg.001";
            var name3A = "L.Head.001";
            var name3B = "L.Head.001";
            var name4A = "Torso.R.001";
            var name4B = "r_Torso.001";

            Assert.AreEqual(name1B, RigNamingConvention.Convert(name1A, src, dst));
            Assert.AreEqual(name2B, RigNamingConvention.Convert(name2A, src, dst));
            Assert.AreEqual(name3B, RigNamingConvention.Convert(name3A, src, dst));
            Assert.AreEqual(name4B, RigNamingConvention.Convert(name4A, src, dst));
        }

        [Test]
        public void TestNamingConverterD()
        {
            var src = new RigNamingConvention(ModifierPosition.End,   ".", "L", "R");
            var dst = new RigNamingConvention(ModifierPosition.Front, "_", "l", "r");

            var name1A = "L_Arm_R";
            var name1B = "L_Arm_R";
            var name2A = "L.Leg.L";
            var name2B = "l_Leg";
            var name3A = "Head.L.R";
            var name3B = "Head.L.R";
            var name4A = "Torso.R.L";
            var name4B = "Torso.R.L";

            Assert.AreEqual(name1B, RigNamingConvention.Convert(name1A, src, dst));
            Assert.AreEqual(name2B, RigNamingConvention.Convert(name2A, src, dst));
            Assert.AreEqual(name3B, RigNamingConvention.Convert(name3A, src, dst));
            Assert.AreEqual(name4B, RigNamingConvention.Convert(name4A, src, dst));
        }
    }
}
